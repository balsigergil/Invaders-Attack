using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private Text finalText;

    [SerializeField]
    private Button retryBtn;

    [SerializeField]
    private Button mainMenuBtn;

    private int level = 1;
    private int score = 0;

    [SerializeField]
    private GameObject spawnLevel;
    [SerializeField]
    private GameObject lvl1;

    private float cooldown = 2.0f;
    private float timeStamp = 0;
    private bool isLevelDone = false;
    private bool isCooldownStarted = false;

    void Start () {
        scoreText.text = score.ToString();
        finalText.text = "";
        retryBtn.gameObject.SetActive(false);
        mainMenuBtn.gameObject.SetActive(false);
        NewLevel(level);
    }

    private void Update()
    {

        int i = 0;
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            i++;
        }

        if (i == 0)
        {
            isLevelDone = true;
        }

        if(isLevelDone && !isCooldownStarted)
            level++;

        if (isLevelDone)
        {
            NewLevel(level);
            Debug.Log("Level:" + level);
        }
    }

    private void NewLevel(int level)
    {
        // Start the cooldown between levels if it isn't already started
        if (!isCooldownStarted)
        {
            timeStamp = Time.time + cooldown;
            isCooldownStarted = true;
        }

        // Instantiate the level when the cooldown is done
        if(timeStamp < Time.time)
        {
            Debug.Log("Cooldown: " + timeStamp);
            Debug.Log("Current time: " + Time.time);
            switch (level)
            {
                case 1:
                    Instantiate(lvl1, spawnLevel.transform);
                    isLevelDone = false;
                    isCooldownStarted = false;
                    break;
                case 2:
                    Instantiate(lvl1, spawnLevel.transform);
                    isLevelDone = false;
                    isCooldownStarted = false;
                    break;
            }
        }
    }

    public void SetScoreText(string text)
    {
        scoreText.text = text;
    }

    public void SetFinalText(string text)
    {
        finalText.text = text;
    }

    public void Loose()
    {
        retryBtn.gameObject.SetActive(true);
        mainMenuBtn.gameObject.SetActive(true);
        SetFinalText("Perdu !");
    }

    public void Win()
    {
        retryBtn.gameObject.SetActive(true);
        mainMenuBtn.gameObject.SetActive(true);
        SetFinalText("Gagné !");
    }

    public void IncrementScore(int loopPoints)
    {
        score += loopPoints;
        SetScoreText(score.ToString());
    }

    public void Retry()
    {
        SceneManager.LoadScene("Game");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    

}
