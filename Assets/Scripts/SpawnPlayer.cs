using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class SpawnPlayer : MonoBehaviour
{
    //Camera dialogueCam;

    GameObject player;
    GameObject spawnLoc;

    private void Awake()
    {
        player = GameObject.Find("Player");
        spawnLoc = GameObject.Find("Location");       
    }

    // Use this for initialization
    void Start()
    {
        player.transform.position = spawnLoc.transform.position; //When scene is loaded. Puts player to the location of this object

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Captured"))
        {
            //dialogueCam = GameObject.Find("DialogueCamera").GetComponent<Camera>();
            player.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
