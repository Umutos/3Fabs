using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchLevelUI : MonoBehaviour
{
    public Image image;
    public Sprite[] m_Sprite;
    public ScriptCam scriptCam;

    void Start()
    {
        //Fetch the Image from the GameObject
        image = GetComponent<Image>();
    }

    void Update()
    {
        //Press space to change the Sprite of the Image
        if (scriptCam.switchRoom)
        {
            image.sprite = m_Sprite[scriptCam.indexRoom + 1];
        }


    }
}
