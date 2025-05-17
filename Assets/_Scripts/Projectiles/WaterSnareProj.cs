using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSnareProj : MonoBehaviour
{
    public float Damage;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            //add stun vfx
            PlayerDetection playerDetection = collision.gameObject.GetComponent<PlayerDetection>();
            WindEnemy Imy = collision.gameObject.GetComponent<WindEnemy>();
            WaterEnemy Wmy = collision.gameObject.GetComponent<WaterEnemy>();
            FireEnemy Fmy = collision.gameObject.GetComponent<FireEnemy>();
            if (Imy != null)
            {
                Imy.TakeStealthDamage(0);
                playerDetection.enabled = false;
            }
            if (Wmy != null)
            {
                Wmy.TakeStealthDamage(0);
                playerDetection.enabled = false;
            }
            if (Fmy != null)
            {
                Fmy.TakeStealthDamage(Damage);
                playerDetection.enabled = false;

            }
            Destroy(gameObject);
        }
    }
}
