using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private int level = 1;
    private int score = 0;

    private float cooldown = 2.0f;
    private float timeStamp = 0;
    private bool isLevelDone = false;
    private bool isCooldownStarted = false;

    private bool isPaused = false;

    private LevelGeneration levelgen = null;

    [Header("Levels")]
    [SerializeField]
    private GameObject spawnLevel;

    [SerializeField]
    private List<Enemy> enemies;
        
    [SerializeField]
    private bool godMode = false;


    [Header("UI")]
    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private Text finalText;

    [SerializeField]
    private Text levelText;

    [SerializeField]
    private Button retryBtn;

    [SerializeField]
    private Button mainMenuBtn;

    [SerializeField]
    private Text healthText;

    void Start () {
        scoreText.text = score.ToString();
        finalText.text = "";
        retryBtn.gameObject.SetActive(false);
        mainMenuBtn.gameObject.SetActive(false);
        levelgen = new LevelGeneration(spawnLevel, enemies);
        NewLevel(level);
        levelText.text = "Niveau: " + level;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Time.timeScale = 1;
                mainMenuBtn.gameObject.SetActive(false);
                isPaused = false;
            }
            else
            {
                Time.timeScale = 0;
                mainMenuBtn.gameObject.SetActive(true);
                isPaused = true;
            }
        }

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
        {
            level++;
            levelText.text = "Niveau: " + level;
        }

        if (isLevelDone)
        {
            NewLevel(level);
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
            levelgen.Generate(level);
            isLevelDone = false;
            isCooldownStarted = false;
        }
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

    public bool GodMode()
    {
        return godMode;
    }

    public void SetScoreText(string text)
    {
        scoreText.text = text;
    }

    public void SetFinalText(string text)
    {
        finalText.text = text;
    }

    public void SetHealthText(string text)
    {
        healthText.text = text;
    }

}
