using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmberTrapProj : MonoBehaviour
{
    public float Damage;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            WindEnemy Imy = collision.gameObject.GetComponent<WindEnemy>();
            WaterEnemy Wmy = collision.gameObject.GetComponent<WaterEnemy>();
            FireEnemy Fmy = collision.gameObject.GetComponent<FireEnemy>();
            if (Imy != null)
            {
                Imy.TakeStealthDamage(Damage);
            }
            if (Wmy != null)
            {
                Wmy.TakeStealthDamage(0);
            }
            if (Fmy != null)
            {
                Fmy.TakeStealthDamage(0);
            }
            Destroy(gameObject);
        }

    }
}
