using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject TitlePanel;
    public GameObject SelectLevelPanel;
    public GameObject CreditPanel;
    public Button button;
    public Button button2;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button1)) // Assurez-vous que "BButton" correspond au bouton B de votre manette
        {
            BackToTitle();
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_confirm");
        }
    }

    public void ChangesScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ChangesIndex(int roomNumber)
    {
        GameManager.instance.rooms = roomNumber;
    }

    public void SelectLevel()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_confirm");
        TitlePanel.SetActive(false);
        SelectLevelPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        button.Select();
    }

    public void Credit()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_confirm");
        TitlePanel.SetActive(false);
        CreditPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        button.Select();
    }

    public void BackToTitle()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_confirm");
        SelectLevelPanel.SetActive(false);
        CreditPanel.SetActive(false);
        TitlePanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null); // Désélectionner le bouton
        button2.Select();
    }

    public void Quit()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_confirm");
        Application.Quit();
    }
}
