using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueBoxManager : MonoBehaviour
{
    AudioSource laughClip;

    GameObject player;
    GameObject realEnemy;
    GameObject enemyCopy;

    GameObject introPanel;
    GameObject insultPanel;
    GameObject answerPanel;
    GameObject pleadPanel;
    GameObject cryPanel;
    GameObject deadPanel;
    GameObject fightPanel;
    

    int diceResult;

    private void Start()
    {
        //Pause game for dialogue scene
        Time.timeScale = 0;

        //Variable assignments
        laughClip = gameObject.GetComponent<AudioSource>();

        //Player and Enemy Objects
        player = GameObject.Find("Player");
        realEnemy = GameObject.Find("TheButcher");
        enemyCopy = GameObject.Find("TheButcherCopy");

        //UI Panels
        introPanel = GameObject.Find("IntroPanel");
        insultPanel = GameObject.Find("InsultPanel");
        answerPanel = GameObject.Find("AnswerPanel");
        pleadPanel = GameObject.Find("PleadPanel");
        cryPanel = GameObject.Find("CryPanel");
        deadPanel = GameObject.Find("DeadPanel");
        fightPanel = GameObject.Find("FightPanel");

        //Set up for start of dialogue event
        realEnemy.SetActive(false);
        insultPanel.SetActive(false);
        answerPanel.SetActive(false);
        pleadPanel.SetActive(false);
        cryPanel.SetActive(false);
        deadPanel.SetActive(false);
        fightPanel.SetActive(false);
    }

    public void RollDice()
    {
        diceResult = Random.Range(1, 101);

        if (diceResult >= 75)
        {
            ShowFightPanel();
        }
        else
        {
            ShowDeadPanel();
        }
    }

    public void ShowAnswerPanel()
    {
        introPanel.SetActive(false);
        answerPanel.SetActive(true);
    }

    public void ShowPleadPanel()
    {
        introPanel.SetActive(false);
        pleadPanel.SetActive(true);
    }

    public void ShowCryPanel()
    {
        introPanel.SetActive(false);
        cryPanel.SetActive(true);
    }

    public void ShowInsultPanel()
    {
        laughClip.Play();
        introPanel.SetActive(false);
        insultPanel.SetActive(true);
    }

    public void ShowFightPanel()
    {
        insultPanel.SetActive(false);
        fightPanel.SetActive(true);
    }

    public void ShowDeadPanel()
    {
        //Set all other panels to inactive to avoid conflicts
        introPanel.SetActive(false);
        realEnemy.SetActive(false);
        insultPanel.SetActive(false);
        answerPanel.SetActive(false);
        pleadPanel.SetActive(false);
        cryPanel.SetActive(false);
        deadPanel.SetActive(false);

        deadPanel.SetActive(true);
    }

    public void StartFight()
    {
        player.SetActive(true);
        enemyCopy.SetActive(false);
        realEnemy.SetActive(true);
        Time.timeScale = 1;

        Destroy(GameObject.Find("Camera"));
        Destroy(gameObject);
    }

    public void LoadGameOverScreen()
    {
        player.SetActive(true);
        SceneManager.LoadScene("GameOver");
    }


}
