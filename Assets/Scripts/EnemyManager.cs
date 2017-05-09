using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof (AudioSource))]
public class EnemyManager : MonoBehaviour
{
    //public int maxDist ;
    public int minDist = 1;
    public int attackRange = 2;
    public float health = 100.0f;
    public float damage = 25.0f;
    public float attackSpeed = 1.5f;
    float time;

    GameObject player;
    CharacterManager playerHealth;
    CharacterController cc;
    Animator anim;

    public AudioClip[] attackSounds;
    AudioSource soundPlayer;


    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;

            if (health <= 0)
            {
                Destroy(this.gameObject);
                Destroy(this.gameObject);
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        time = 0.0f;
        soundPlayer = gameObject.GetComponent<AudioSource>();
        player = GameObject.Find("Player");
        playerHealth = player.GetComponent<CharacterManager>();
        cc = gameObject.GetComponent<CharacterController>();
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Vector3.Distance(gameObject.transform.position, player.transform.position) >= minDist)
        {
            transform.LookAt(new Vector3(player.transform.position.x, 2, player.transform.position.z));
            cc.SimpleMove(transform.forward * 4);
            anim.SetBool("Running", true);           
        }

        if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= attackRange)
        {
            time += Time.deltaTime;
            if (time >= attackSpeed)
            {
                anim.SetTrigger("Attack");
                Attack(playerHealth);
                time = 0.0f;
                PlayAttackSound();
            }
            
        }
    }

    public void Attack(CharacterManager player)
    {
        player.Health -= damage;
    }

    void PlayAttackSound()
    {
        int num = Random.Range(0, attackSounds.Length);
        soundPlayer.clip = attackSounds[num];
        soundPlayer.PlayOneShot(soundPlayer.clip);
    }

    private void OnDestroy()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BloodHallway"))
        {
            CharacterManager.foughtHallwayMonster = true;
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("InterrogationRoom"))
        {
            CharacterManager.foughtBoss = true;
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Captured"))
        {
            CharacterManager.foughtBoss = true;
        }
    }
}
