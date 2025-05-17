using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyDistraction : MonoBehaviour
{
    public float SoundCircleRadius = 0.5f;
    public int DestroyTime = 3;
    float Timer = 0f;
    bool hasStarted = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStarted)
        {
            Timer += Time.deltaTime;
            if (Timer >= DestroyTime)
            {
                Debug.Log("Destroyed");
               Destroy(gameObject);
                
            }
        }
            
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            DrawCircle();
            hasStarted = true;
            Debug.Log("Started!");      
        }
    }
    public void DrawCircle()
    {
        Collider2D[] Hit = Physics2D.OverlapCircleAll(transform.position, SoundCircleRadius);
        foreach (Collider2D enemy in Hit)
        {
            if (enemy.GetComponent<PlayerDetection>() != null)
            {
                PlayerDetection playerDetection = enemy.GetComponent<PlayerDetection>();
                if ( !playerDetection.isChasing && !playerDetection.isInvestigating)
                {
                    if(playerDetection.isFacingLeft == false)
                    {
                        Vector2 distractionPoint = transform.position;
                        StartCoroutine(playerDetection.Investigate(distractionPoint));
                        playerDetection.FlipLeft();
                    }
                    else
                    {

                        Vector2 distractionPoint = transform.position;
                        StartCoroutine(playerDetection.Investigate(distractionPoint));
                    }
                }
                

            }
        }
    }
    private void OnDestroy()
    {
        Collider2D[] Hit = Physics2D.OverlapCircleAll(transform.position, SoundCircleRadius);
        foreach (Collider2D enemy in Hit)
        {
            if (enemy.GetComponent<PlayerDetection>() != null)
            {
                PlayerDetection playerDetection = enemy.GetComponent<PlayerDetection>();
                playerDetection.ReturnState();
                playerDetection.isInvestigating = false;


            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, SoundCircleRadius);
    }
}
