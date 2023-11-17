using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score_manager : MonoBehaviour
{
    public static Score_manager instance;
    // Start is called before the first frame update

    public List<float> time;
    public List<bool> Win;
    public List<int> room;

    private void Awake()
    {
        instance = this;

        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
    }

    // Update is called once per frame
    public void sendScore()
    {
        
    }
}
