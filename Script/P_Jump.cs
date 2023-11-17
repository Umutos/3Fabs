using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks.Sources;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class P_Jump : MonoBehaviour
{
    [SerializeField] private float          playerJumpForce;
    [SerializeField] private float          helpingJumpForce;
    [SerializeField] private Collider2D     triggerTransform;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator       animator;

    private Rigidbody2D[] rbPlayers;
    private Rigidbody2D   rb;
    private Rigidbody2D   playerInTrigger;
    private P_Grounded    checkGround;
    private int           index;

    private bool inputJump;
    private bool inputHelp;
    private bool cooldown = true;
    private bool isKick = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        checkGround = GetComponent<P_Grounded>();
        rbPlayers = FindObjectsOfType<Rigidbody2D>();

    }

    public void SetInputBool(bool _inputJump)
    {
        inputJump = _inputJump;
    }

    public void SetInputHelingJump(bool _inputHelp)
    {
        inputHelp = _inputHelp;
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        HelpingJump();
        FlipTrigger();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player-Grabber" || collision.gameObject.tag == "Player-Puncher" || collision.gameObject.tag == "Boss" || collision.gameObject.tag == "Object")
            playerInTrigger = collision.gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player-Grabber" || collision.gameObject.tag == "Boss" || collision.gameObject.tag == "Player-Puncher" || collision.gameObject.tag == "Object")
            playerInTrigger = null;
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

    void HelpingJump()
    {
        if (checkGround.isGrounded && cooldown) 
        {
            StartCoroutine(Cooldown());

            if (inputHelp && playerInTrigger != null)
            {
                animator.SetTrigger("Raise");
                if (playerInTrigger.tag == "Player-Grabber")
                    StartCoroutine(playSoundJumpGrabber());
                else if (playerInTrigger.tag == "Player-Puncher")
                    StartCoroutine(playSoundJumpPuncher());

                for (int i = 0; rbPlayers.Length > i; i++)
                    {
                        if (playerInTrigger == rbPlayers[i])
                        {
                            rbPlayers[i].velocity = new Vector2(rbPlayers[i].velocity.x, helpingJumpForce);

                            if (rbPlayers[i].gameObject.tag == "Object")
                            {
                                index = i;  
                                isKick = true;
                            }
                        }
                    }
            }
        }
        if(isKick)
            StartCoroutine(ChangeMass(index));

    }

    void Jump()
    {

        if (inputJump)
        {
            if(checkGround.isGrounded && cooldown)
            {
                animator.SetTrigger("Jump");
                FMODUnity.RuntimeManager.PlayOneShot("event:/CHARA/JUMPER/power_jump");
                rb.velocity = new Vector2(rb.velocity.x, playerJumpForce);
            }
        }
    }

    IEnumerator ChangeMass(int i)
    {
        rbPlayers[i].GetComponent<Rigidbody2D>().mass = 1;
        rbPlayers[i].AddForce(new Vector2(rbPlayers[i].velocity.y, helpingJumpForce));
        yield return new WaitForSeconds(4f);
        rbPlayers[i].GetComponent<Rigidbody2D>().mass = 1000;
    }

    IEnumerator playSoundJumpGrabber()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/CHARA/GRABER/graber_jump");
        yield return new WaitForSeconds(4);
    }
    IEnumerator playSoundJumpPuncher()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/CHARA/PUNCHER/puncher_jump");
        yield return new WaitForSeconds(4);
    }
    IEnumerator Cooldown()
    {
        cooldown = false;
        yield return new WaitForSeconds(0.1f);
        cooldown = true;
    }
}
