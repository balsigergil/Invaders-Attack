using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manager the game logic, levels and score
/// </summary>
public class GameManager : MonoBehaviour
{

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
    private Text healthText;

    [SerializeField]
    private Button retryBtn;

    [SerializeField]
    private Button mainMenuBtn;

    // Initialization
    void Start()
    {
        scoreText.text = score.ToString();
        finalText.text = "";
        retryBtn.gameObject.SetActive(false);
        mainMenuBtn.gameObject.SetActive(false);
        levelgen = new LevelGeneration(spawnLevel, enemies);
        NewLevel(level);
        levelText.text = "Niveau: " + level;
    }

    // Update is called once per frame
    private void Update()
    {
        // Handle the pause and resume feature
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Time.timeScale = 1;                         // Restart the time
                mainMenuBtn.gameObject.SetActive(false);    // Hide the main menu button
                isPaused = false;
            }
            else
            {
                Time.timeScale = 0;                         // Stop the time
                mainMenuBtn.gameObject.SetActive(true);     // Show the main menu button
                isPaused = true;
            }
        }

        // Marks the level as "done" when there isn't any enemy left in the scene
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            isLevelDone = true;
        }

        if (isLevelDone)
        {
            if (!isCooldownStarted)
            {
                level++;
                levelText.text = "Niveau: " + level;
            }

            NewLevel(level);
        }
    }

    /// <summary>
    /// Handles the creation of a new level
    /// </summary>
    /// <param name="level"></param>
    private void NewLevel(int level)
    {
        // Start the cooldown between levels if it isn't already started
        if (!isCooldownStarted)
        {
            timeStamp = Time.time + cooldown;
            isCooldownStarted = true;
        }

        // Instantiate the level when the cooldown is done
        if (timeStamp < Time.time)
        {
            levelgen.Generate(level);
            isLevelDone = false;
            isCooldownStarted = false;
        }
    }

    /// <summary>
    /// Update the UI to winning states
    /// </summary>
    public void Win()
    {
        retryBtn.gameObject.SetActive(true);
        mainMenuBtn.gameObject.SetActive(true);
        SetFinalText("Gagné !");
    }

    /// <summary>
    /// Update the UI to loosing states
    /// </summary>
    public void Loose()
    {
        retryBtn.gameObject.SetActive(true);
        mainMenuBtn.gameObject.SetActive(true);
        SetFinalText("Perdu !");
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


    /************************************************************************/
    /* Getters and setters                                                  */
    /************************************************************************/

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

    public bool GetIsPaused()
    {
        return isPaused;
    }

}
