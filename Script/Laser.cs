using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Laser : MonoBehaviour
{
    //public LayerMask layerToHit;

    public LineRenderer line;
    public Transform laserPOsition; 
    public Transform EndWaterPos;
    public Transform StartWaterPos;

    public Material Water1;
    public Material Water2;
    private bool Water = true;
    public bool vertical;

    public float coolDownAnimWater;
    private float timercoolDown;

    [SerializeField] private float endWaterOffset;
    [SerializeField] private FMODUnity.StudioEventEmitter sound;


    private void Awake()
    {
        timercoolDown = coolDownAnimWater;
    }

    // Update is called once per frame
    void Update()
    {
        if(!sound.IsPlaying())
            sound.Play();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right); 
        line.SetPosition(0, laserPOsition.position);
        if(hit)
        {
            line.SetPosition(1, hit.point);
            if(vertical)
            {
                Vector2 distance = hit.point - new Vector2( StartWaterPos.transform.position.x, StartWaterPos.transform.position.y);
                if(distance.y > 0)
                    EndWaterPos.position = hit.point + new Vector2(0, -endWaterOffset);
                else
                    EndWaterPos.position = hit.point + new Vector2(0, endWaterOffset);
            }
            else
            {
                Vector2 distance = hit.point - new Vector2(StartWaterPos.transform.position.x, StartWaterPos.transform.position.y);
                if (distance.x > 0)
                    EndWaterPos.position = hit.point + new Vector2(-endWaterOffset, 0);
                else
                    EndWaterPos.position = hit.point + new Vector2(endWaterOffset, 0);
            }

        }
        else
        {
            line.SetPosition(1, transform.right * 100);
        }

        if (hit.collider.tag == "Player-Jumper" || hit.collider.tag == "Player-Puncher" || hit.collider.tag == "Player-Grabber")
        {
            GameObject.FindObjectOfType<Spawnpoint>().hasSwitchedRoom = false;
            var Players = GameObject.FindObjectsOfType<P_Controller>();
            for (int i = 0; Players.Length > i; i++)
            {
                Players[i].IsDead = true;
            }
        }

        timercoolDown -= Time.deltaTime;

        if(timercoolDown < 0)
        {
            if (Water)
            {
                line.material = Water2;
                Water = false;
            } 
            else
            {
                line.material = Water1;
                Water = true;
            }

            timercoolDown = coolDownAnimWater;
        }


        /*float angle = transform.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 50f, layerToHit);
        if(hit.collider == null)
        {
            transform.localScale = new Vector3(50f, transform.localScale.y, 1);
            return;
        }
        transform.localScale = new Vector3(hit.distance, transform.localScale.y, 1);
        Debug.Log(hit.collider.gameObject.name);
        if(hit.collider.tag == "Player-Jumper" || hit.collider.tag == "Player-Puncher")
        {
            Destroy(hit.collider.gameObject);
        }*/
    }
}
