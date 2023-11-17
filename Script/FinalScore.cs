using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinalScore : MonoBehaviour
{
    public Score_manager score_Manager;
    public GameObject win;
    public GameObject loose;
    public TMP_Text time;
    public int lvl;
    // Start is called before the first frame update
    void Start()
    {
        score_Manager = FindObjectOfType<Score_manager>();
    }

    // Update is called once per frame
    void Update()
    {
        time.text = string.Format(" {000} ", score_Manager.time[lvl]);
        if (score_Manager.Win[lvl])
            win.SetActive(true);
        else
            loose.SetActive(true);
    }
}
