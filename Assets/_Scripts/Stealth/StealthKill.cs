using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class StealthKill : MonoBehaviour
{
    private bool CanKill;
    private bool InRangeOfKill;
    public Animator anim;
    public AudioClip KillSfx;

    public void KillEnemy(InputAction.CallbackContext context)
    {
        if (EventManager.Instance.isEventActive) return;
        if (context.performed && InRangeOfKill)
        {
            // Only allow killing if the enemy has not seen the player
            if (!StealthManager.instance.IsPlayerSpotted)
            {
                anim.SetTrigger("Stab");
                CanKill = true;
                Debug.Log("performed Stealth Kill");
            }
            else
            {
                CanKill = false;
                Debug.Log("Cannot perform Stealth Kill - enemy is aware");
            }
        }
        else if (context.performed && !InRangeOfKill)
        {
            CanKill = false;
            Debug.Log("Cannot perform Stealth Kill - out of range");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            PlayerDetection playerDetection = collision.gameObject.GetComponent<PlayerDetection>();
            ShowSkullOnKillableEnemy showSkullOnKillableEnemy = collision.gameObject.GetComponent<ShowSkullOnKillableEnemy>();
            if (playerDetection != null)
            {
                if (!StealthManager.instance.IsPlayerSpotted && !playerDetection.HasSeenPlayerOnce)
                {
                    showSkullOnKillableEnemy.ActivateIcon();
                    InRangeOfKill = true;
                    Debug.Log("Can Kill the Enemy");
                }
                else
                {
                    showSkullOnKillableEnemy.DeactivateIcon();
                    InRangeOfKill = false;
                }
            }

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            
            ShowSkullOnKillableEnemy showSkullOnKillableEnemy = collision.gameObject.GetComponent<ShowSkullOnKillableEnemy>();
            if(CanKill && StealthManager.instance.IsPlayerSpotted)
            {
                showSkullOnKillableEnemy.DeactivateIcon();
            }
            if (CanKill && !StealthManager.instance.IsPlayerSpotted) // Ensure enemy is not aware
            {
                FireEnemy fireEnemy = collision.gameObject.GetComponent<FireEnemy>();
                WaterEnemy waterEnemy = collision.gameObject.GetComponent<WaterEnemy>();
                WindEnemy iceEnemy = collision.gameObject.GetComponent<WindEnemy>();

                if (fireEnemy != null)
                {
                    AudioManager.instance.PlaySoundFXClip(KillSfx, transform, 0.4f, Random.Range(0.9f,1));
                    fireEnemy.HandleDeath();
                    ThrowableKnifeManager.Instance.AddOnePoint();
                    CanKill = false;
                }

                if (waterEnemy != null)
                {
                    AudioManager.instance.PlaySoundFXClip(KillSfx, transform, 0.4f, Random.Range(0.9f, 1));
                    waterEnemy.HandleDeath();
                    ThrowableKnifeManager.Instance.AddOnePoint();
                    CanKill = false;
                }

                if (iceEnemy != null)
                {
                    AudioManager.instance.PlaySoundFXClip(KillSfx, transform, 0.4f, Random.Range(0.9f, 1));
                    iceEnemy.HandleDeath();
                    ThrowableKnifeManager.Instance.AddOnePoint();
                    CanKill = false;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            PlayerDetection playerDetection = collision.gameObject.GetComponent<PlayerDetection>();
            ShowSkullOnKillableEnemy showSkullOnKillableEnemy = collision.gameObject.GetComponent<ShowSkullOnKillableEnemy>();
            if(playerDetection != null)
            {
                if (!StealthManager.instance.IsPlayerSpotted && !playerDetection.HasSeenPlayerOnce)
                {
                    showSkullOnKillableEnemy.DeactivateIcon();
                    InRangeOfKill = false;
                    
                }
            }
            else
            {
                showSkullOnKillableEnemy.DeactivateIcon();
                InRangeOfKill = false;
            }
            
        }
    }
    
}
