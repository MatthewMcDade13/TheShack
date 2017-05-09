using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public static MenuManager instance = null;
    public static MenuManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);

    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadHelpMenu()
    {
        SceneManager.LoadScene("HelpMenu");
    }

    public void LoadAboutMenu()
    {
        SceneManager.LoadScene("AboutMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("StartRoom");
    }

    public void RetryGame()
    {
        SceneManager.LoadScene("StartRoom");
    }

    public void LoadGoBackScene()
    {
        SceneManager.LoadScene("GoBackSplashScreen");
    }

    public void GoToGameOverScreen()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GameOver");
    }
}
