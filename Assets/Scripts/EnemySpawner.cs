using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    AudioSource creatureScream;

    GameObject bloodHallwayEnemy;
    GameObject butcherBoss;

    float audioTimer;
    float spawnTimer;
    bool enemySpawned = false;

    // Use this for initialization
    void Start()
    {
        audioTimer = 0.0f;
        spawnTimer = 0.0f;

        creatureScream = gameObject.GetComponent<AudioSource>();

        bloodHallwayEnemy = GameObject.Find("Nightmare");
        butcherBoss = GameObject.Find("TheButcher");
        
        if (bloodHallwayEnemy != null)
        {
            bloodHallwayEnemy.SetActive(false);
        }
        if (butcherBoss != null)
        {
            butcherBoss.SetActive(false);
        }

        
    }

    //This update method is only used for The Butcher Enemy.
    private void Update()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("InterrogationRoom"))
        {
            //Checks if enemy is spawned yet, and if not, runs timer to count up to 30 secs
            //Once timer is at 25 secs, The Butcher Enemy Spawns.
            if (enemySpawned == false)
            {
                spawnTimer += Time.deltaTime;

                if (spawnTimer >= 25)
                {
                    enemySpawned = true;
                    butcherBoss.SetActive(true);
                }
            }
        }
       
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && enemySpawned == false)
        {
            bloodHallwayEnemy.SetActive(true);
            creatureScream.Play();

            //sets enemy spawn to true so we can start the timer for the object to destroy itself.
            enemySpawned = true;
        }

        //Waits to destroy the object so that the audio source component attached to it has enough time to play.
        if (enemySpawned)
        {
            audioTimer += Time.deltaTime;

            if (audioTimer >= 4)
            {
                Destroy(gameObject);
            }
        }
    }
}
