using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NotifyNearbyEnemies : MonoBehaviour
{
    [SerializeField]
    private float detectionRadius = 5f; 
    private List<PlayerDetection> nearbyEnemies = new List<PlayerDetection>();

    void Update()
    {
        // Clear the list and find nearby enemies
        nearbyEnemies.Clear();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy") && collider != this.GetComponent<Collider2D>())
            {
                nearbyEnemies.Add(collider.GetComponent<PlayerDetection>());
            }
        }
    }

    public void NotifyEnemies()
    {
        foreach (var enemy in nearbyEnemies)
        {
            if (enemy != null && !enemy.IsNotifying) 
            {
                enemy.ChangeState(); 
            }
        }
    }
}
