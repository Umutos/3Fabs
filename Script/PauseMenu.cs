using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;

    public static bool gamePause = false;
    public static bool isPaused = false;

    public void SetBoolPause(bool _gamePause)
    {
        isPaused = !isPaused;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused)
        {
            Pause();
        }
        else
        {
            Continue();
        }
    }

    public void Pause()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;

    }

    public void Continue()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;

    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        isPaused = false;
    }

    public void Leave(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        isPaused = false;
    }
}