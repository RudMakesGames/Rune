using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D trigger;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            OutOfBounds.Instance.respawnPoint = transform;
            trigger.enabled = false;
        }
    }
}
