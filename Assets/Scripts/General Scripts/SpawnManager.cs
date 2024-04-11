using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Enemy Game objects
    public GameObject Enemy;
    public GameObject Obstacle;

    // Spawn positions for enemies and obstacles
    private Vector2 spawnPos_1 = new Vector2(15, 3);
    private Vector2 spawnPos_2 = new Vector2(15, 1);
    private Vector2 spawnPos_3 = new Vector2(15, -1);
    private Vector2 spawnPos_4 = new Vector2(15, -3);

    // Time to spawn enemies
    public float startDelay = 2f;
    public float spawnInterval = 2f;

    // Array for enemies
    private int[] lastTwoEnemySpawns = new int[] { -1, -1 };

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnLineOfEnemies", startDelay, spawnInterval);
    }

    void SpawnLineOfEnemies()
    {
        Vector2[] spawnPositions = new Vector2[] { spawnPos_1, spawnPos_2, spawnPos_3, spawnPos_4 };
        int enemySpawnIndex;

        // Loop to ensure we don't spawn in the same position 3 times in a row
        do
        {
            enemySpawnIndex = Random.Range(0, spawnPositions.Length);
        }
        while (enemySpawnIndex == lastTwoEnemySpawns[0] && enemySpawnIndex == lastTwoEnemySpawns[1]);

        // Update the last two spawn indices
        lastTwoEnemySpawns[0] = lastTwoEnemySpawns[1];
        lastTwoEnemySpawns[1] = enemySpawnIndex;

        for (int i = 0; i < spawnPositions.Length; i++)
        {
            if (i == enemySpawnIndex)
            {
                Instantiate(Enemy, spawnPositions[i], Enemy.transform.rotation);
            }
            else
            {
                Instantiate(Obstacle, spawnPositions[i], Enemy.transform.rotation);
            }
        }
    }
}