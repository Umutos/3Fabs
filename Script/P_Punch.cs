using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks.Sources;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class P_Punch : MonoBehaviour
{
    [SerializeField] private float playerPunchForce;

    [SerializeField] private Collider2D triggerTransform;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    [SerializeField] private Spawnpoint sp;

    public List<Room_Manager> rooms;
    private ScriptCam scriptCam;
    private Camera cam;
    private Rigidbody2D[] rbPlayers;
    private Rigidbody2D   playerInTrigger;
    private int index = 0;

    private bool inputPunch;
    public static bool bossDead;
    private bool cooldown = true;
    private bool isKick = false;
    [SerializeField] private GameObject particule;
    private ParticleSystem playParticule;

    public void SetInputBoolPunch(bool _inputPunch)
    {
        inputPunch = _inputPunch;
    }

    private void Start()
    {
        rbPlayers = FindObjectsOfType<Rigidbody2D>();
        cam = Camera.main;
        scriptCam = cam.GetComponent<ScriptCam>();
        playParticule = particule.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        Punch();
        FlipTrigger();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player-Grabber" || collision.gameObject.tag == "Player-Jumper" || collision.gameObject.tag == "Object")
            playerInTrigger = collision.gameObject.GetComponent<Rigidbody2D>();
        else if(collision.gameObject.tag == "Boss")
        {
            playerInTrigger = collision.gameObject.GetComponent<Rigidbody2D>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player-Grabber" || collision.gameObject.tag == "Player-Jumper" || collision.gameObject.tag == "Boss" || collision.gameObject.tag == "Object")
            playerInTrigger = null;
        if (collision.gameObject.tag == "Boss")
        {
            cam.transform.position = Vector2.Lerp(new Vector2(cam.transform.position.x, cam.transform.position.y), new Vector2(scriptCam.Rooms[scriptCam.indexRoom].transform.position.x, scriptCam.Rooms[scriptCam.indexRoom].transform.position.y), 0.5f);
            cam.orthographicSize = 8;
        }
    }

    void FlipTrigger()
    {
        if (spriteRenderer.flipX == false)
        {
            triggerTransform.offset = new Vector2(1.5f, 0);
        }
        else if (spriteRenderer.flipX == true)
        {
            triggerTransform.offset = new Vector2(-1.5f, 0);
        }
    }

    void Punch()
    {
        if (inputPunch && playerInTrigger != null && cooldown)
        {
            StartCoroutine(Cooldown());
            animator.SetTrigger("Kick");
            Instantiate<ParticleSystem>(playParticule, playerInTrigger.transform);
            for (int i = 0; rbPlayers.Length > i; i++)
            {
                if (playerInTrigger == rbPlayers[i])
                {
                    if (rbPlayers[i].gameObject.tag != "Boss" && rbPlayers[i] != null && rbPlayers[i].gameObject.tag != "Object")
                    {
                            FMODUnity.RuntimeManager.PlayOneShot("event:/CHARA/PUNCHER/punch_chara");
                        if (playerInTrigger.tag == "Player-Grabber")
                                FMODUnity.RuntimeManager.PlayOneShot("event:/CHARA/GRABER/graber_punch");
                        else if (playerInTrigger.tag == "Player-Jumper")
                                    FMODUnity.RuntimeManager.PlayOneShot("event:/CHARA/JUMPER/jumper_punch");
                        if (spriteRenderer.flipX == false)
                        {
                            rbPlayers[i].GetComponent<P_Controller>().punchVelocity = playerPunchForce;
                        }
                        else if (spriteRenderer.flipX == true)
                        {
                            rbPlayers[i].GetComponent<P_Controller>().punchVelocity = -playerPunchForce;

                        }
                    }
                    else if (rbPlayers[i].gameObject.tag == "Boss")
                    {
                        GameObject.FindObjectOfType<Shake>().start = true;
                        if (cooldown)
                            FMODUnity.RuntimeManager.PlayOneShot("event:/CHARA/BOSS/boss_death");
                        scriptCam.switchRoom = true;
                        bossDead = true;
                        sp.hasSwitchedRoom = false;
                        rooms[scriptCam.indexRoom].finish = true;
                        rooms[scriptCam.indexRoom].Win = true;
                        Destroy(rbPlayers[i].gameObject);
                    }
                    else if(rbPlayers[i].gameObject.tag == "Object")
                    {
                        FMODUnity.RuntimeManager.PlayOneShot("event:/CHARA/PUNCHER/punch_box");
                        index = i;
                        isKick = true;
                        
                    }
                }
            }
        }
        if(isKick)
            StartCoroutine(ChangeMass(index));
    }

    IEnumerator ChangeMass(int i)
    {
       
        rbPlayers[i].GetComponent<Rigidbody2D>().mass = 1;
        if (spriteRenderer.flipX == false)
        {
            rbPlayers[i].AddForce(new Vector2(playerPunchForce, rbPlayers[i].velocity.y));
        }
        else if (spriteRenderer.flipX == true)
        {
            rbPlayers[i].AddForce(new Vector2(-playerPunchForce, rbPlayers[i].velocity.y));
        }
        yield return new WaitForSeconds(0.1f);
        isKick= false;
        rbPlayers[i].GetComponent<Rigidbody2D>().mass = 1000;
    }

    IEnumerator Cooldown()
    {
        cooldown = false;
        yield return new WaitForSeconds(2);
        cooldown = true;
    }
}
