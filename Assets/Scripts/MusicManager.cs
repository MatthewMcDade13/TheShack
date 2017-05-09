using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    static GameObject startRoomMusic;
    static GameObject bloodHallwayMusic;
    static GameObject interoRoomMusic;
    static GameObject outdoorMusic;

    private void Awake()
    {
        startRoomMusic = GameObject.Find("StartRoomMusic");
        bloodHallwayMusic = GameObject.Find("BloodHallwayMusic");
        interoRoomMusic = GameObject.Find("InteroRoomMusic");
        outdoorMusic = GameObject.Find("OutdoorMusic");
        //startRoomMusic.SetActive(false);
        //bloodHallwayMusic.SetActive(false);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "StartRoom")
        {
            startRoomMusic.SetActive(true);

            bloodHallwayMusic.SetActive(false);
            interoRoomMusic.SetActive(false);
            outdoorMusic.SetActive(false);         
        }
        else if (scene.name == "BloodHallway")
        {
            bloodHallwayMusic.SetActive(true);

            startRoomMusic.SetActive(false);
            interoRoomMusic.SetActive(false);
            outdoorMusic.SetActive(false);
        }
        else if (scene.name == "InterrogationRoom" || scene.name == "Captured")
        {
            interoRoomMusic.SetActive(true);

            bloodHallwayMusic.SetActive(false);
            startRoomMusic.SetActive(false);
            outdoorMusic.SetActive(false);
        }
        else if (scene.name == "Outdoor")
        {

            outdoorMusic.SetActive(true);

            bloodHallwayMusic.SetActive(false);
            startRoomMusic.SetActive(false);
            interoRoomMusic.SetActive(false);
        }
    }
}
