using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TimeScript : MonoBehaviour
{
     private FMODUnity.StudioEventEmitter sound;
    [SerializeField] private GameObject gameObjectAnimation;
    public float timeLeft;
    public bool timeOn = false;
    private bool startCounter = false;
    private Animator startAnimation;

    private ScriptCam cam;
    private Room_Manager room;
    
    public TMP_Text TimeTxt;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.GetComponent<ScriptCam>();
        room = GetComponent<Room_Manager>();
        startAnimation = Camera.main.GetComponentInChildren<Animator>();
        sound = Camera.main.GetComponent<FMODUnity.StudioEventEmitter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (room.roomNum == cam.indexRoom && !startCounter)
        {
            gameObjectAnimation.SetActive(true);
            StartCoroutine(startChrono());
        }
        if (timeOn)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                UpdateTime(timeLeft);
            }
            else
            {
                GameObject.FindObjectOfType<Spawnpoint>().hasSwitchedRoom = false;
                cam.switchRoom = true;
                cam.start = false;
                startCounter = false;
                timeOn = false;
                room.finish = true;
                room.Win = false;
                timeLeft = 0;
            }
        }
    }

    void UpdateTime(float currentTime)
    {
        currentTime += 1;

        float seconde = Mathf.FloorToInt(currentTime);

        TimeTxt.text = string.Format(" {000} ", seconde);
    }

    private IEnumerator startChrono()
    {
        startAnimation.SetBool("start", true);
        yield return new WaitForSeconds(0.45f);
        if (!sound.IsPlaying())
            sound.Play();
        yield return new WaitForSeconds(3);
        gameObjectAnimation.SetActive(false);
        if (sound.IsPlaying())
            sound.Stop();
        timeOn = true;
        startAnimation.SetBool("start", false);
        cam.start = true;
        startCounter = true;
    }
}
