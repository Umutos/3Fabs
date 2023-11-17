using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TotalScore : MonoBehaviour
{
    // Start is called before the first frame update
    public FinalScore[] finalScores;
    private TMP_Text time;
    private float total;
    void Start()
    {
        time = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < finalScores.Length; i++)
        {
            total += finalScores[i].score_Manager.time[finalScores[i].lvl]; 
        }
        time.text = string.Format(" {000} ", total);
    }
}
