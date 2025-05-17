using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrapProj : MonoBehaviour
{
    public float Damage;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
           WindEnemy Imy =  collision.gameObject.GetComponent<WindEnemy>();
            WaterEnemy Wmy = collision.gameObject.GetComponent<WaterEnemy>();
            FireEnemy Fmy = collision.gameObject.GetComponent<FireEnemy>();
            if(Imy != null)
            {
                Imy.TakeDamage(Damage);
            }
            if(Wmy != null)
            {
                Wmy.TakeDamage(0);
            }
            if(Fmy != null)
            {
                Fmy.TakeDamage(0);
            }
            Destroy(gameObject);
        }
        
    }
}
