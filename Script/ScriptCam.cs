using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptCam : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> Rooms;
    public bool switchRoom = false;
    public float lerpTime;
    private bool switchScene = false;
    private P_Punch punch;
    public bool start = false; 

    public int indexRoom;

    // Start is called before the first frame update
    void Start()
    {
        punch = FindObjectOfType<P_Punch>();
    }

    // Update is called once per frame
    void Update()
    {
        if (switchRoom) 
        {
            if(indexRoom == 10)
            {
                SceneManager.LoadScene("Recap-Scene");
                switchScene = true;
            }
            if (!switchScene)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/whoosh_camera");
                indexRoom += 1;
            }
                   
            switchRoom = false;
        }
        if(!switchScene)
            transform.position = Vector3.Lerp(transform.position, Rooms[indexRoom].transform.position, lerpTime * Time.deltaTime);
    }
}
    
