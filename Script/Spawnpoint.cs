using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject player2;
    [SerializeField] private GameObject player3;
    [SerializeField] public List<Spawn> spawnPoints;

    public ScriptCam scriptCam;

    public bool hasSwitchedRoom = false;

    private void Start()
    {
        GameObject.FindObjectOfType<ScriptCam>().indexRoom = GameManager.instance.rooms;
        player.transform.position  = spawnPoints[scriptCam.indexRoom - 1].spawn1.transform.position;
        player2.transform.position = spawnPoints[scriptCam.indexRoom - 1].spawn2.transform.position;
        player3.transform.position = spawnPoints[scriptCam.indexRoom - 1].spawn3.transform.position;
    }

    void Update()
    {
        if (P_Punch.bossDead)
        {
            if (!hasSwitchedRoom)
            {
                hasSwitchedRoom = true;
                player.transform.position  = spawnPoints[scriptCam.indexRoom].spawn1.transform.position;
                player2.transform.position = spawnPoints[scriptCam.indexRoom].spawn2.transform.position;
                player3.transform.position = spawnPoints[scriptCam.indexRoom].spawn3.transform.position;
            }
        }

        if (scriptCam.switchRoom)
        {
            if (!hasSwitchedRoom)
            {
                hasSwitchedRoom = true;
                player.transform.position = spawnPoints[scriptCam.indexRoom].spawn1.transform.position;
                player2.transform.position = spawnPoints[scriptCam.indexRoom].spawn2.transform.position;
                player3.transform.position = spawnPoints[scriptCam.indexRoom].spawn3.transform.position;
            }
        }
    }
}
