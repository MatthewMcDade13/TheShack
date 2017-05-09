using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    GameObject character;

    //Used for credits
    GameObject outro;
    GameObject credits;


    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    // Use this for initialization
    void Start()
    {
        outro = GameObject.Find("OutroPanel");
        credits = GameObject.Find("CreditsPanel");

        if (credits != null)
        {
            credits.SetActive(false);
        }


        character = GameObject.Find("Player");

        if (character != null)
        {
            Destroy(character);
        }
    }

    public void PlayCredits()
    {
        outro.SetActive(false);
        credits.SetActive(true);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


}
