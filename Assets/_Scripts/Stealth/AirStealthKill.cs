using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


    public class AirStealthKill : MonoBehaviour
{
    private bool CanKill;
    private bool InRangeOfKill;
    public string AnimationTransitionName;
    public Animator anim;
    public CapsuleCollider2D PlayerCapsule;
    public BoxCollider2D PlayerBox;
    public Transform EnemyPos;
    public PlayerController playerController;
    private bool isInAirKill;
    public bool AirStealthKillUnlocked = false;

    public void KillEnemy(InputAction.CallbackContext context)
    {
        if(AirStealthKillUnlocked)
        {
            if (context.performed && InRangeOfKill)
            {
                CanKill = true;
                Debug.Log("can perform Stealth kILL");
            }
            else
            {
                CanKill = false;
                Debug.Log("cannot perform Stealth kILL");
            }
        }
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            
            
                if (!isInAirKill)
                {
                    PlayerDetection playerDetection = collision.gameObject.GetComponent<PlayerDetection>();
                    ShowSkullOnKillableEnemy showSkullOnKillableEnemy = collision.gameObject.GetComponent<ShowSkullOnKillableEnemy>();
                    if (StealthManager.instance.IsPlayerSpotted == false)
                    {
                        if (playerDetection.HasSeenPlayerOnce == false)
                        {
                            showSkullOnKillableEnemy.ActivateIcon();
                            InRangeOfKill = true;
                            Debug.Log("Can Kill the Enemy");
                        }
                    }
                }
            
            
            
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            
            
                EnemyPos = collision.transform;
                if (CanKill)
                {
                    if (StealthManager.instance.IsPlayerSpotted == false)
                    {
                        PlayerDetection playerDetection = collision.gameObject.GetComponent<PlayerDetection>();
                        FireEnemy fireEnemy = collision.gameObject.GetComponent<FireEnemy>();
                        WaterEnemy waterEnemy = collision.gameObject.GetComponent<WaterEnemy>();
                        WindEnemy iceEnemy = collision.gameObject.GetComponent<WindEnemy>();
                        isInAirKill = true;

                        if (fireEnemy != null)
                        {
                            anim.SetTrigger(AnimationTransitionName);
                            playerDetection.MovementVar = 0;
                            playerDetection.enabled = false;
                            StartCoroutine(DisableCollider());
                            playerController.MoveTowardsEnemy();
                            ThrowableKnifeManager.Instance.AddOnePoint();
                            playerDetection.enabled = false;
                            if (Vector2.Distance(playerController.PlayerPos.position, EnemyPos.position) < 2f)
                            {
                                anim.SetTrigger("AirSlash");
                                fireEnemy.HandleDeath();
                                CanKill = false;
                                isInAirKill = false;
                            }

                        }

                        if (waterEnemy != null)
                        {
                            anim.SetTrigger(AnimationTransitionName);
                            playerDetection.MovementVar = 0;
                            playerDetection.enabled = false;
                            StartCoroutine(DisableCollider());
                            playerController.MoveTowardsEnemy();

                            ThrowableKnifeManager.Instance.AddOnePoint();
                            playerDetection.enabled = false;
                            if (Vector2.Distance(playerController.PlayerPos.position, EnemyPos.position) < 2f)
                            {
                                anim.SetTrigger("AirSlash");
                                waterEnemy.HandleDeath();
                                CanKill = false;
                                isInAirKill = false;
                            }




                        }

                        if (iceEnemy != null)
                        {
                            anim.SetTrigger(AnimationTransitionName);
                            playerDetection.MovementVar = 0;
                            playerDetection.enabled = false;
                            StartCoroutine(DisableCollider());
                            playerController.MoveTowardsEnemy();
                            ThrowableKnifeManager.Instance.AddOnePoint();
                            playerDetection.enabled = false;
                            if (Vector2.Distance(playerController.PlayerPos.position, EnemyPos.position) < 2f)
                            {
                                anim.SetTrigger("AirSlash");
                                iceEnemy.HandleDeath();
                                CanKill = false;
                                isInAirKill = false;
                            }

                        }
                    }

                }
            }
           

        
        

    }
    IEnumerator DisableCollider()
    {
       
        
            PlayerCapsule.isTrigger = true;
            PlayerBox.isTrigger = true;
            yield return new WaitForSeconds(0.1f);
            PlayerCapsule.isTrigger = false;
            PlayerBox.isTrigger = false;
        
       


    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            
            
                if (!isInAirKill)
                {
                    PlayerDetection playerDetection = collision.gameObject.GetComponent<PlayerDetection>();
                    ShowSkullOnKillableEnemy showSkullOnKillableEnemy = collision.gameObject.GetComponent<ShowSkullOnKillableEnemy>();
                    if (StealthManager.instance.IsPlayerSpotted == false)
                    {
                        if (playerDetection.HasSeenPlayerOnce == false)
                        {
                            showSkullOnKillableEnemy.DeactivateIcon();
                            InRangeOfKill = false;

                        }
                    }
                }
            
            
           
        }
    }
}
    
