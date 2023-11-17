using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class P_Motor : MonoBehaviour
{
    private P_Jump jumper;
    private P_Punch puncher;
    private PlayerInput  playerInput;
    private P_grabe grabber;
    private P_Controller controller;
    private Lever lever;
    private PauseMenu pauseMenu;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        var controllers = FindObjectsOfType<P_Controller>();
        jumper = FindObjectOfType<P_Jump>();
        grabber = FindObjectOfType<P_grabe>();
        puncher = FindObjectOfType<P_Punch>();
        lever = FindObjectOfType<Lever>();
        pauseMenu = FindObjectOfType<PauseMenu>();
        var index = playerInput.playerIndex;
        controller = controllers.FirstOrDefault(c => c.GetPlayerIndex() == index);
    }

    public void OnMove(CallbackContext context)
    {
        if (controller != null)
            controller.SetInputVector(context.ReadValue<Vector2>());

    }

    public void OnJump(CallbackContext context)
    {
        if (jumper != null && controller.gameObject.tag == "Player-Jumper")
            jumper.SetInputBool(context.ReadValue<Single>() > 0);

    }

    public void OnHelpingJump(CallbackContext context)
    {
        if (jumper != null && controller.gameObject.tag == "Player-Jumper")
            jumper.SetInputHelingJump(context.ReadValue<Single>() > 0);
    }

    public void OnPause(CallbackContext context)
    {
        if(pauseMenu != null)
            pauseMenu.SetBoolPause(context.ReadValue<Single>() > 0);
            
    }

    public void OnPunch(CallbackContext context)
    {
        if(puncher  != null && controller.gameObject.tag == "Player-Puncher")
            puncher.SetInputBoolPunch(context.ReadValue<Single>() > 0);

    }

    public void OnGrabe(CallbackContext context)
    {
        if (grabber != null && controller.gameObject.tag == "Player-Grabber")
        {
            grabber.setBool(context.ReadValue<Single>() > 0);
        }
    }

    public void GrabDir(CallbackContext context)
    {
        if(grabber != null && controller.gameObject.tag == "Player-Grabber")
        {
            grabber.grabDir(context.ReadValue<Vector2>());
        }
    }

    public void LeverActive(CallbackContext context)
    {
        if (lever != null)
        {
            lever.SetActiveLever(context.ReadValue<Single>() > 0);
        }
    }
}
