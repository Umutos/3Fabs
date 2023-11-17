using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Manager : MonoBehaviour
{
    private TimeScript time;
    private Score_manager score_Manager;
    public bool finish = false;
    public bool Win = false;
    public bool debugCoroutine = false;
    public int roomNum = 0;
    public float roomTime = 0;

    void Start()
    {
        time = GetComponent<TimeScript>();
        score_Manager = FindObjectOfType<Score_manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (finish)
        {
            time.timeOn = false;
            if (roomNum == Camera.main.GetComponent<ScriptCam>().indexRoom)
            {
                roomTime = time.timeLeft;
                score_Manager.time.Add(roomTime);
                score_Manager.room.Add(roomNum);
                score_Manager.Win.Add(Win);
            }

        }
    }
}
