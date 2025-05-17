using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BlinkAbility : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private ElementalPowerBar powerBar;
    [SerializeField] private Slider AirSlider;
    [SerializeField] private Rigidbody2D rb;

    [Header("Blink Settings")]
    [SerializeField] private float blinkDistance = 2f; 
    [SerializeField] private float blinkCost = 50f;
    [SerializeField] private AudioClip BlinkSfx;
    public void PerformBlink(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (playerController.IsInWindState && AirSlider.value >= blinkCost)
            {
              
                powerBar.TakeIceEnergy(blinkCost);
                Blink();
                AudioManager.instance.PlaySoundFXClip(BlinkSfx,transform,1, UnityEngine.Random.Range(0.9f, 1));
            }
        }
    }
    
    private void Blink()
    {
        
        Vector3 blinkDirection = playerController.isFacingRight ? Vector3.right : Vector3.left;
        Vector3 targetPosition = transform.position + blinkDirection * blinkDistance;

        
        transform.position = targetPosition;

        
        Debug.Log("Blink performed to position: " + targetPosition);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the blink range in the Scene view
        Gizmos.color = Color.cyan;
        Vector3 direction = playerController.isFacingRight ? Vector3.right : Vector3.left;
        Gizmos.DrawLine(transform.position, transform.position + direction * blinkDistance);
    }


}
