using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoiceManager : MonoBehaviour
{
    CharacterManager player;

    GameObject useBedPanel;
    GameObject healthBottle;
    GameObject goBackUI;

    private void Awake()
    {
        SceneManager.sceneLoaded += FindObjects;

        useBedPanel = GameObject.Find("UseBedPanel");

        if (useBedPanel != null)
        {
            useBedPanel.SetActive(false);
        }


    }

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<CharacterManager>();
    }

    public void UseBed()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameObject.GetComponent<FirstPersonController>().enabled = false;
        useBedPanel.SetActive(true);
    }

    //public void GoBack()
    //{
    //    goBackUI.SetActive(true);
    //    Time.timeScale = 0;
    //    Cursor.lockState = CursorLockMode.None;
    //    Cursor.visible = true;
    //    gameObject.GetComponent<FirstPersonController>().enabled = false;

    //}

        /// <summary>
        /// Called when scene is loaded.
        /// Finds objects in scene that we can use.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
    private void FindObjects(Scene scene, LoadSceneMode mode)
    {
        //TODO: Find out why UI goes glitchy after first death, but bottle object isnt effected

        healthBottle = GameObject.Find("HealthBottle");

        //goBackUI = GameObject.Find("GoBackUI");

        //if (goBackUI != null)
        //{
        //    goBackUI.SetActive(false);
        //}

    }

    public void UseHealthBottle()
    {
        player.Health += 25f;

        Destroy(healthBottle);
    }
}
