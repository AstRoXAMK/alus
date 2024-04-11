using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{
    // Score text variables
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI highScoreText;
    public delegate void ScoreUpdated(int newScore);
    public event ScoreUpdated OnScoreUpdated;
    private static int score;
    public static ScoreManager Instance { get; private set; }

    // For enemy speed increse
    public float speedIncrement = 0.3f;

    void Awake()
    {
        Debug.Log("ScoreManager Awake called");
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("ScoreManager instance created");
        }
        else
        {
            Debug.Log("ScoreManager instance already exists");
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        UpdateScoreText();
    }

    public int GetScore()
    {
        return score;
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        SetScoreText();

        if (score % 10 == 0)
        {
            EnemyScript.UpdateGlobalSpeedModifier(speedIncrement); // Increment global speed modifier
        }

        if (OnScoreUpdated != null)
        {
            OnScoreUpdated(score);
        }
        Debug.Log("Score updated to: " + score);
    }

    private string UpdateScoreText()
    {
        if (ScoreText == null)
        {
            Debug.LogError("ScoreText is null");
        }
        return ScoreText.text =  "Score: " + score;
    }

    public void SetScoreText()
    {
        ScoreText.text = UpdateScoreText();
    }
}
