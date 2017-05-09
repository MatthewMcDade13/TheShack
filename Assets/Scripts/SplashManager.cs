using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{
    GameObject player;

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player = GameObject.Find("Player");
        player.SetActive(false);
    }

    public void LoadCapturedScene()
    {
        player.SetActive(true);
        SceneManager.LoadScene("Captured");
    }
}
