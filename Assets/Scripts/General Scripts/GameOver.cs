using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject ScoreScreen;
    public static bool isGameOver = false;

    public void SetGameOver()
    {
        isGameOver = true;
        gameOverScreen.SetActive(true);
        ScoreScreen.SetActive(false);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        isGameOver = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gameObject.SetActive(false);
        EnemyScript.speedModifier = 0f;
    }

    public void BackToMenu()
    {
        isGameOver = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
        EnemyScript.speedModifier = 0f;
    }
}
