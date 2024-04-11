using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerController : MonoBehaviour
{
    public KeyCode playerKeyboard = KeyCode.Space;
    public KeyCode playerMouse = KeyCode.Mouse0;
    public KeyCode playerShootKeyboard = KeyCode.RightShift;
    public KeyCode playerShootMouse = KeyCode.Mouse1;

    // speed variables
    public float speed;
    public float speedIncrease = 0.5f;

    private bool isPositive = true;

    // Bullet variables
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform bulletSpawn;
    public float fireRate = 2.0f;
    private float nextFireRate = 0.0f;

    private GameOver gameOver;

    private ScoreManager scoreManager;
    private int currentScore;
    private int highScore;
    private int lastCheckedScore = -1;

    // Opacity changers
    private NegaticveBarrierOpacity negative;
    private PositiveBarrierOpacity positive;

    // Start is called before the first frame update
    void Start()
    {
        if (negative == null)
        {
            negative = FindObjectOfType<NegaticveBarrierOpacity>();
        }
        if (positive == null)
        {
            positive = FindObjectOfType<PositiveBarrierOpacity>();
        }
        if (scoreManager == null)
        {
            scoreManager = FindObjectOfType<ScoreManager>();
        }
        if (gameOver == null)
        {
            gameOver = FindObjectOfType<GameOver>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        updateMovement();

        CheckAndUpdateScore();
        if (gameOver == null)
        {
            gameOver = FindObjectOfType<GameOver>();
        }

        if (Input.GetKeyDown(playerKeyboard) || Input.GetKeyDown(playerMouse))
        {
            MovementPolarity();
        }
        if (Input.GetKeyDown(playerShootKeyboard) || Input.GetKeyDown(playerShootMouse))
        {
            ShootProjectile();
        }
    }

    public void MovementPolarity()
    {
        isPositive = !isPositive;
    }

    void updateMovement()
    {
        if (PauseMenu.isPaused == false)
        {
            Vector3 direction = transform.position;
            if (isPositive)
            {
                direction.y += speed * Time.deltaTime;
            }
            if (!isPositive)
            {
                direction.y -= speed * Time.deltaTime;
            }
            transform.position = direction;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyShip") || collision.gameObject.CompareTag("Obstacle")
            || collision.gameObject.CompareTag("Barrier"))
        {
            FindAnyObjectByType<AudioManager>().Play("PlayerDeath");

            Destroy(this.gameObject);

            if (gameOver != null)
            {
                gameOver.SetGameOver();
                Debug.Log("GG's");
            }
            else
            {
                Debug.LogError("GameOver script not found!");
            }
        }
    }

    public void ShootProjectile()
    {
        if (Time.time >= nextFireRate) {
            Instantiate(projectile, bulletSpawn.position, Quaternion.identity);
            FindAnyObjectByType<AudioManager>().Play("PlayerShoot");
            //Debug.Log("Pew");
            nextFireRate = Time.time + 1f / fireRate;
        }
    }

    void CheckAndUpdateScore()
    {
        if (scoreManager != null)
        {
            currentScore = scoreManager.GetScore();

            highScore = PlayerPrefs.GetInt("HighScore", 0);

            // Check if score has changed and if it's a multiple of 2
            if (currentScore != lastCheckedScore && currentScore % 10 == 0)
            {
                UpdateSpeed(speedIncrease);
                lastCheckedScore = currentScore; // Update last checked score
            }

            if (currentScore > highScore)
            {
                highScore = currentScore;
                PlayerPrefs.SetInt("HighScore", highScore);
                PlayerPrefs.Save();
                scoreManager.highScoreText.text = "New High score: " + highScore;
            }
            else if (currentScore < highScore)
            {
                scoreManager.highScoreText.text = "High score: " + highScore 
                    + "\n\n" + "Score: " + currentScore;
            }
            else if (highScore == 0)
            {
                scoreManager.highScoreText.text = "Score: " + currentScore;
            }
        }
    }

    public int getHighScore()
    {
        return PlayerPrefs.GetInt("HighScore", 0);
    }

    void UpdateSpeed(float speedIncrement)
    {
        // Update the speed of the game object here
        speed += speedIncrement;
        Debug.Log("Speed is " + speed);
    }
}
