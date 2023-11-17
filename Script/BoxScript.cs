using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    // Start is called before the first frame update
    private P_Grounded check;
    private bool notGrounded = false;
    private bool isPlay = false;
    void Start()
    {
        check = GetComponent<P_Grounded>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!check.isGrounded)
        {
            notGrounded = true;
            if (!isPlay)
            {
                isPlay = true;
                FMODUnity.RuntimeManager.PlayOneShot("event:/ENVIRO/box_jump");
            }
        }
        if (check.isGrounded && notGrounded)
        {
            notGrounded = false;
            isPlay = false;
            FMODUnity.RuntimeManager.PlayOneShot("event:/ENVIRO/box_land");
        }
    }
}
