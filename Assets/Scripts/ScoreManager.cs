using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    private int score = 0;

    [SerializeField]
    private Text scoreLabel;

    private static ScoreManager sm = null;

    public static ScoreManager GetInstante()
    {
        if (sm == null)
            sm = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        return sm;
    }

    public void IncrementScore(int amount)
    {
        score += amount;
        UpdateGUI();
    }

    public void UpdateGUI()
    {
        scoreLabel.text = score.ToString();
    }
}
