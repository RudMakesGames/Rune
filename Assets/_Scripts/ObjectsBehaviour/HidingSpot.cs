using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    [SerializeField]
    private LayerMask PlayerLayer, HideLayer;
    [SerializeField]
    private float HidingThreshold = 1f;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Change the layer of the player object
            collision.gameObject.layer = LayerMask.NameToLayer("Hidden"); // Use the correct layer name
            Debug.Log("In Hiding Spot");
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            PlayerDetection playerDetection = collision.gameObject.GetComponent<PlayerDetection>();
            if (playerDetection != null && playerDetection.PlayerDetectionTimer > HidingThreshold)
            {
                playerDetection.ReturnState();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Change the layer of the player object back to the original layer
            collision.gameObject.layer = LayerMask.NameToLayer("Player"); // Use the correct layer name
            Debug.Log("Out of Hiding Spot");
        }
    }
}
