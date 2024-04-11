using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 5f;
    public float missedShot = 3.5f;
    

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyBulletAfterTime", missedShot);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector3 movementDirection = transform.position;
        movementDirection.x += speed * Time.deltaTime;
        transform.position = movementDirection;
    }

    void DestroyBulletAfterTime()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("EnemyShip"))
        {
            Destroy(this.gameObject);
            Destroy(collider.gameObject);
            FindAnyObjectByType<AudioManager>().Play("EnemyDeath");
            Debug.Log("kill");
            ScoreManager.Instance.IncreaseScore(1);
        }
        if (collider.gameObject.CompareTag("Obstacle"))
        {
            Destroy(this.gameObject);
            Debug.Log("Obstacle hit");
        }
    }
}
