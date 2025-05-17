using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public Transform PlayerPos;
    public Transform FiringPoint;
    [SerializeField] float AttackRange = 3;
    public LayerMask PlayerLayer;
    [SerializeField]
    float Speed;
    [SerializeField]
    private float Distance;
    private bool CanAttack;
    private Animator anim;
    private float PunchTimer;
    [SerializeField]
    private float PunchDelay;
    [SerializeField]
    private float DamageToPlayer = 100;
    [SerializeField]
    private BossHealth bossHealth;
    private bool IsInSecondPhase;

    private int consecutiveAttackCount = 0;
    private const int maxConsecutiveAttacks = 2;

    public enum AttackType
    {
        None,
        Attack,
        Clap
    }
    private AttackType lastAttack = AttackType.None;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        PunchTimer += Time.deltaTime;
       if(!CanAttack && Vector2.Distance(transform.position, PlayerPos.position) > Distance)
        {
            transform.position = Vector2.MoveTowards(transform.position, PlayerPos.position, Speed * Time.deltaTime);
        }
       else
        {
            CheckForAttack();
        }
       
       if (Vector2.Distance(transform.position, PlayerPos.position) < Distance)
        {
            CanAttack = true;
            anim.SetBool("IsRunning",false);
        }
       else
        {
            CanAttack=false;
            anim.SetBool("IsRunning", true);
        }
       SecondPhaseCheck();
        
    }

    private void SecondPhaseCheck()
    {
        if(bossHealth.CurrentHealth <= (bossHealth.MaxHealth * 1/2))
        {
            IsInSecondPhase = true;
        }
    }

    private void CheckForAttack()
    {
        if (CanAttack)
        {
            if (PunchTimer >= PunchDelay)
            {
                if (IsInSecondPhase)
                {
                    // Randomly choose between Clap (1) and Attack (0)
                    int randomAttack = Random.Range(0, 2);
                    AttackType selectedAttack;

                    // Determine the selected attack
                    if (lastAttack == AttackType.Attack && consecutiveAttackCount >= maxConsecutiveAttacks)
                    {
                        selectedAttack = AttackType.Clap; // Force switch if max is reached
                        consecutiveAttackCount = 0; // Reset for a new type
                    }
                    else if (lastAttack == AttackType.Clap && consecutiveAttackCount >= maxConsecutiveAttacks)
                    {
                        selectedAttack = AttackType.Attack; // Force switch if max is reached
                        consecutiveAttackCount = 0; // Reset for a new type
                    }
                    else
                    {
                        selectedAttack = (randomAttack == 0) ? AttackType.Attack : AttackType.Clap;
                    }

                    // Execute the selected attack
                    if (selectedAttack == AttackType.Attack)
                    {
                        anim.SetTrigger("Attack");
                        Collider2D[] HitPlayer = Physics2D.OverlapCircleAll(FiringPoint.position, AttackRange, PlayerLayer);
                        foreach (Collider2D player in HitPlayer)
                        {
                            if (PlayerController.isHittable)
                            {
                                player.GetComponent<PlayerHealth>().TakeDamage(DamageToPlayer);
                            }
                        }

                        // Update counters
                        if (lastAttack == AttackType.Attack)
                        {
                            consecutiveAttackCount++;
                        }
                        else
                        {
                            consecutiveAttackCount = 1; // Reset for new attack type
                        }
                    }
                    else if (selectedAttack == AttackType.Clap)
                    {
                        anim.SetTrigger("Clap");
                        Collider2D[] Hit2DPlayer = Physics2D.OverlapCircleAll(FiringPoint.position, AttackRange, PlayerLayer);
                        foreach (Collider2D player in Hit2DPlayer)
                        {
                            if (PlayerController.isHittable)
                            {
                                player.GetComponent<PlayerHealth>().TakeDamage(DamageToPlayer * 2.25f);
                                player.GetComponent<PlayerHealth>().KnockbackForce = 5;
                            }
                        }

                        // Update counters
                        if (lastAttack == AttackType.Clap)
                        {
                            consecutiveAttackCount++;
                        }
                        else
                        {
                            consecutiveAttackCount = 1; // Reset for new attack type
                        }
                    }

                    // Update the last attack
                    lastAttack = selectedAttack;
                    PunchTimer = 0;
                }
                else
                {
                    anim.SetTrigger("Attack");
                    Collider2D[] HitPlayer = Physics2D.OverlapCircleAll(FiringPoint.position, AttackRange, PlayerLayer);
                    foreach (Collider2D player in HitPlayer)
                    {
                        if (PlayerController.isHittable)
                        {
                            player.GetComponent<PlayerHealth>().TakeDamage(DamageToPlayer);
                        }
                    }

                    // Update for normal phase
                    lastAttack = AttackType.Attack;
                    consecutiveAttackCount = 1; // Start count
                    PunchTimer = 0;
                }
            }
        }
    

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(FiringPoint.position,AttackRange);
    }


}
