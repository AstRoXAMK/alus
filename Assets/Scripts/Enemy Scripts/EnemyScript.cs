using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    // Movement variables
    public static float speedModifier = 0f;
    public float movementSpeed;
    private float x_Min = -15.0f;

    // Score to manage speed ups
    private ScoreManager scoreManager;

    public float limitLow = 4.3f;
    public float limitHigh = 4.8f;

    void Start()
    {
        movementSpeed = Random.Range(limitLow, limitHigh) + speedModifier;
        if (scoreManager == null)
        {
            scoreManager = FindObjectOfType<ScoreManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * Time.deltaTime * movementSpeed);

        if (transform.position.x <= x_Min)
        {
            Destroy(gameObject);
        }
    }

    public static void UpdateGlobalSpeedModifier(float increment)
    {
        speedModifier += increment;
        Debug.Log("Speed increased by: " + speedModifier);
    }
}