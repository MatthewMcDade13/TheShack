using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class CharacterMenuManager : MonoBehaviour
{
    GameObject player;
    GameObject popupPanel;
    GameObject pauseMenu;
    GameObject optionsMenu;
    bool menuToggle = false;


    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
        popupPanel = GameObject.Find("Popup");
        pauseMenu = GameObject.Find("PauseMenu");
        optionsMenu = GameObject.Find("OptionsPanel");
        popupPanel.SetActive(false);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuToggle = !menuToggle;
            pauseMenu.SetActive(menuToggle);
            

            if (menuToggle)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;
                player.GetComponent<FirstPersonController>().enabled = false;
                
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1;
                player.GetComponent<FirstPersonController>().enabled = true;

            }
        }
    }

    public void ResumeGame()
    {
        menuToggle = !menuToggle;
        pauseMenu.SetActive(menuToggle);
        Time.timeScale = 1;
        player.GetComponent<FirstPersonController>().enabled = true;
        Cursor.visible = false;

    }

    public void OpenOptionsMenu()
    {
        optionsMenu.SetActive(true);
    }

    public void HideOptionsMenu()
    {
        optionsMenu.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowPopupWindow()
    {
        popupPanel.SetActive(true);
    }

    public void HidePopupWindow()
    {
        popupPanel.SetActive(false);
    }
}
