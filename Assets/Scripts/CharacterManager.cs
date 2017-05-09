using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager playerInstance = null; //Ensures only one instance of this object exists through scenes

    #region Global Variables
    ChoiceManager choice;
    EnemyManager enemy;

    RaycastHit[] hits;
    RaycastHit hitInfo;

    Transform cam;

    Animator Weaponanim;

    Text actionUIText;
    Text healthUI;
    Text eventUIText;

    GameObject menuScript;

    MeshRenderer weapon;

    AudioSource flashLightClip;

    Light flashLight;

    bool lookingAtObject;
    bool hasWeapon = false;
    bool hasFlashLight = false;
    bool toggleFlashLight = true;
    bool hallUIactive = true;
    bool tortureUIactive = true;
    bool startHealthBottleTimer = false;
    bool wasCaptured = false;
    public static bool foughtBoss;
    public static bool foughtHallwayMonster;

    public float health = 100.0f;
    public float damage = 25f;
    public float attackSpeed = 0.12f;
    float timer = 0.0f;
    float uiTimer = 0.0f;
    float bottleTimer = 0.0f;

    #endregion

    /// <summary>
    /// Public property for managing Player Health
    /// </summary>
    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            healthUI.text = "Health: " + ((int)health).ToString(); // Update player health everytime it is changed.

            if (health <= 0) // If after updating health, player health is at or below 0, take player to Game Over screen
            {
                SceneManager.LoadScene("GameOver");
            }
        }
    }

    private void Awake()
    {
        //Check if an instance of this object as already been created, if so destroy the newly created object
        if (playerInstance != null && playerInstance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            playerInstance = this;
        }

        DontDestroyOnLoad(gameObject);

        //Load Event Delegate to find an enemy everytime the scene is loaded and assign it to the enemy variable.
        SceneManager.sceneLoaded += LookForEnemy;
    }

    // Use this for initialization
    void Start()
    {
        Time.timeScale = 1;
        foughtBoss = false;
        foughtHallwayMonster = false;

        //Initialize variable for MenuManager Object
        menuScript = GameObject.Find("MenuManager");
        //Check if object is not null, if so, destroy it
        if (menuScript != null)
        {
            Destroy(menuScript);
        }

        #region Object Assignments
        choice = gameObject.GetComponent<ChoiceManager>();
        cam = Camera.main.transform;

        flashLight = GameObject.Find("Flashlight").GetComponent<Light>();
        flashLightClip = GameObject.Find("Flashlight").GetComponent<AudioSource>();

        //Variables handling weapon object attached to player
        weapon = GameObject.Find("PlayerWeapon").GetComponent<MeshRenderer>();
        Weaponanim = GameObject.Find("PlayerWeapon").GetComponent<Animator>();

        //Variables dealing with UI text
        actionUIText = GameObject.Find("ActionUIText").GetComponent<Text>();
        healthUI = GameObject.Find("HealthUIText").GetComponent<Text>();
        eventUIText = GameObject.Find("EventUIText").GetComponent<Text>();

        //"Default" behaviors to start the game with
        WriteActionUIText("");       //Clear UI text that tells us about interactable objects
        WriteEventUIText("");        //Clear UI text that tells us about things happening in game
        weapon.enabled = false;      //Start off with no weapon. So hide the mesh renderer!
        flashLight.enabled = false;  //Start off with no flashlight, so disable the object!
        healthUI.text = "Health: " + ((int)health).ToString(); //Displays health on UI
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        //Finds our target and assigns it to hitInfo
        hitInfo = FindTarget();

        #region Logic for writing text to UI and interacting with objects in world
        //Checks to see if our FindTarget Method returned anything we care about.
        if (hitInfo.transform != null)
        {

            //If FindTarget returned an object we care about, 
            //we check to see if we are close enough to the target to interact with it
            if (hitInfo.distance <= 2)
            {
                lookingAtObject = true;
            }
            else
            {
                lookingAtObject = false;
            }

            //Checks what we are looking at for specific tags,
            //If what we are looking at has the appropriate tag, 
            //we allow actions to happen when the E key is pressed on it
            if (lookingAtObject && hitInfo.collider.tag == "door") //Interact with door in first room
            {
                WriteActionUIText("Open Door");

                if (Input.GetKeyDown(KeyCode.E))
                {
                    SceneManager.LoadScene("BloodHallway");
                }
            }
            else if (lookingAtObject && hitInfo.collider.tag == "bed") //Interact with bed
            {
                WriteActionUIText("Sleep");

                if (Input.GetKeyDown(KeyCode.E))
                {
                    choice.UseBed();
                }
            }
            else if (lookingAtObject && hitInfo.collider.tag == "doorback") //Interact with the door back to the start room
            {
                WriteActionUIText("Open Door");

                if (Input.GetKeyDown(KeyCode.E))
                {
                    //SceneManager.LoadScene("Captured");
                    WriteActionUIText("");
                    WriteEventUIText("");
                    SceneManager.LoadScene("GoBackSplashScreen");
                    wasCaptured = true;
                }
            }
            else if (lookingAtObject && hitInfo.collider.tag == "flashlight") //Interact with the flashlight
            {
                WriteActionUIText("Pick Up");

                if (Input.GetKeyDown(KeyCode.E))
                {
                    hasFlashLight = true;
                    flashLight.enabled = true;
                    Destroy(GameObject.Find("FlashLight"));
                }
            }
            else if (lookingAtObject && hitInfo.collider.tag == "axe") //Interact with the Axe
            {
                WriteActionUIText("Pick Up");

                if (Input.GetKeyDown(KeyCode.E))
                {
                    hasWeapon = true;
                    weapon.enabled = true;
                    Destroy(GameObject.Find("Axe"));
                }
            }
            else if (lookingAtObject && hitInfo.collider.tag == "hallexit") //Interact with the door in hallway that leads to Interrogation Room
            {
                WriteActionUIText("Open Door");

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (foughtHallwayMonster == false)
                    {
                        WriteEventUIText("Locked");
                    }
                    else
                    {
                        SceneManager.LoadScene("InterrogationRoom");
                    }
                }

            }
            else if (lookingAtObject && hitInfo.collider.tag == "enterInteroDoor") //Interact with the door you entered the Interrogation Room with
            {
                WriteActionUIText("Open Door");

                if (Input.GetKeyDown(KeyCode.E))
                {
                    WriteEventUIText("Jammed");
                }
            }
            else if (lookingAtObject && hitInfo.collider.tag == "healthBottle") //Interact with the health bottle.
            {
                WriteActionUIText("Drink");

                if (Input.GetKeyDown(KeyCode.E))
                {
                    choice.UseHealthBottle();
                    startHealthBottleTimer = true;
                    //WriteEventUIText("You gain 25HP");
                }
            }
            else if (lookingAtObject && hitInfo.collider.tag == "endGameDoor") //Interact with the final door that initiates the end of the game scene / final scene
            {
                WriteActionUIText("Open Door");

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (foughtBoss == false)
                    {
                        WriteEventUIText("Locked");
                    }
                    else
                    {
                        SceneManager.LoadScene("Outdoor");
                    }
                }
            }

        }       
        else
        {
            #region Logic for cleaning up UI. (Also includes displaying health text UI for 3 seconds before wiping)

            //Clears the UI screen of text if we are not looking at any interactable objects
            //or if player does not need to be notified about an event in game.
            WriteActionUIText("");

            //Check if health bottle has been drank.
            //If so, run a timer so we can display the effects of the bottle for 4 seconds
            //Otherwise, just wipe the screen of Text UI as usual
            if (startHealthBottleTimer)
            {
                bottleTimer += Time.deltaTime;

                if (bottleTimer <= 3)
                {
                    WriteEventUIText("Restored 25HP");
                }
                else
                {
                    startHealthBottleTimer = false;
                }
            }
            else
            {
                WriteEventUIText("");
            }
            #endregion
        }



        #endregion

        #region Logic for Attacking
        //Timer for attack cooldown
        timer += Time.deltaTime;


        if (Input.GetMouseButtonDown(0))
        {
            //Checks if cooldown has "finsihed" and allows player to attack
            if (timer >= attackSpeed)
            {
                Weaponanim.SetTrigger("Attack");

                if (hitInfo.transform != null)
                {
                    if (lookingAtObject && hitInfo.collider.tag == "enemy" && hasWeapon)//Only runs attack method if we are looking at an enemy object
                    {
                        Attack(enemy);
                    }
                }
                timer = 0.0f;
            }
        }
        #endregion

        //Checks if player has gotten flashlight yet and if so, allows player to toggle
        //flashlight on and off with the F key.
        if (hasFlashLight && Input.GetKeyDown(KeyCode.F))
        {
            toggleFlashLight = !toggleFlashLight;

            flashLight.enabled = toggleFlashLight;
            flashLightClip.Play();
        }

        #region Logic for Telling player when they can proceed to next room

        if (foughtHallwayMonster && hallUIactive && wasCaptured == false)
        {
            uiTimer += Time.deltaTime;

            if (uiTimer <= 4)
            {
                WriteEventUIText("You hear a door unlock.");
            }
            else
            {
                WriteEventUIText("");
                hallUIactive = false;
                uiTimer = 0.0f;
            }
        }

        if (foughtBoss && tortureUIactive)
        {
            uiTimer += Time.deltaTime;

            if (uiTimer <= 4)
            {
                WriteEventUIText("You hear a door unlock.");
            }
            else
            {
                WriteEventUIText("");
                tortureUIactive = false;
                uiTimer = 0.0f;
            }
        }

        #endregion
    }

    /// <summary>
    /// Only finds target objects we care about and returns what we are looking at
    /// </summary>
    /// <returns>Currently Targeted object</returns>
    RaycastHit FindTarget()
    {
        RaycastHit target = new RaycastHit();

        hits = Physics.RaycastAll(cam.transform.position, cam.transform.forward, 500f);

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.name == "Player")
            {
                continue;
            }

            if (hit.transform.tag == "bed")
            {
                target = hit;
            }
            else if (hit.transform.tag == "door")
            {
                target = hit;
            }
            else if (hit.transform.tag == "enemy")
            {
                target = hit;
            }
            else if (hit.transform.tag == "doorback")
            {
                target = hit;
            }
            else if (hit.transform.tag == "flashlight")
            {
                target = hit;
            }
            else if (hit.transform.tag == "axe")
            {
                target = hit;
            }
            else if (hit.transform.tag == "hallexit")
            {
                target = hit;
            }
            else if (hit.transform.tag == "enterInteroDoor")
            {
                target = hit;
            }
            else if (hit.transform.tag == "healthBottle")
            {
                target = hit;
            }
            else if (hit.transform.tag == "endGameDoor")
            {
                target = hit;
            }
        }

        return target;


    }

    private void WriteActionUIText(string text)
    {
        actionUIText.text = text;
    }

    private void WriteEventUIText(string text)
    {
        eventUIText.text = text;
    }

    public void Attack(EnemyManager enemy)
    {
        Weaponanim.SetTrigger("Attack");
        enemy.Health -= damage;
    }

    /// <summary>
    /// To be assigned to the Scenemanager.sceneLoaded event.
    /// This method finds specific enemies based on scene loaded and assigns them to the enemy variable.
    /// </summary>
    /// <param name="scene"> not used </param>
    /// <param name="mode"> not used </param>
    private void LookForEnemy(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("StartRoom"))
        {
            return;
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BloodHallway"))
        {
            enemy = GameObject.Find("Nightmare").GetComponent<EnemyManager>();
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("InterrogationRoom"))
        {
            enemy = GameObject.Find("TheButcher").GetComponent<EnemyManager>();
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Captured"))
        {
            enemy = GameObject.Find("TheButcher").GetComponent<EnemyManager>();
        }

    }

}
