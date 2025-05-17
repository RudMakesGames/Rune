using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField]
    private float bulletLifetime = 1.0f;
    [SerializeField]
    private float bulletSpeed = 1.0f;
    private float timer;
    public float distance = 3f;
    private float movementDir = 1;

    private GameObject targetEnemy; // Target enemy GameObject
    private Vector3 lastKnownEnemyPosition;
    
    private void Start()
    {
        // Find the nearest enemy at the start
        FindNearestEnemy();
    }

    private void Update()
    {
        // Update the timer and destroy bullet if it exceeds lifetime
        timer += Time.deltaTime;
        if (timer > bulletLifetime)
        {
            StartCoroutine(DestroyBullet());
        }

        // Find and track the nearest enemy each frame
        FindNearestEnemy();
        HomingMovement();
    }

    // Method to flip the projectile's direction if needed
    public void Flip()
    {
        movementDir *= -1;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    // Handle collision with different types of enemies
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            WaterEnemy Wenemy = collision.GetComponent<WaterEnemy>();
            FireEnemy Fenemy = collision.GetComponent<FireEnemy>();
            WindEnemy Ienemy = collision.GetComponent<WindEnemy>();

            if (Wenemy != null)
            {
                Wenemy.HandleDeath();
                ThrowableKnifeManager.Instance.EnemyKilled = true;
            }
            if (Fenemy != null)
            {
                Fenemy.HandleDeath();
                ThrowableKnifeManager.Instance.EnemyKilled = true;
            }
            if (Ienemy != null)
            {
                Ienemy.HandleDeath();
               ThrowableKnifeManager.Instance.EnemyKilled = true;
            }

            StartCoroutine(DestroyBullet());
        }
    }

    // Homing movement towards the nearest enemy
    void HomingMovement()
    {
        if (targetEnemy != null)
        {
            // Calculate direction towards the target
            Vector3 direction = (targetEnemy.transform.position - transform.position).normalized;

            // Move towards the target
            transform.Translate(direction * bulletSpeed * Time.deltaTime, Space.World);

            // Store the last known position of the target
            lastKnownEnemyPosition = targetEnemy.transform.position;
        }
        else if (lastKnownEnemyPosition != Vector3.zero)
        {
            // Move towards the last known position if the enemy is null
            Vector3 direction = (lastKnownEnemyPosition - transform.position).normalized;
            transform.Translate(direction * bulletSpeed * Time.deltaTime, Space.World);
        }
    }

    // Find the nearest enemy
    void FindNearestEnemy()
    {
        float minDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < minDistance)
            {
                minDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        targetEnemy = nearestEnemy;
    }

    // Destroy the bullet after a delay
    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
