using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBladeproj : MonoBehaviour
{
    [SerializeField]
    private float BulletLifetime = 1.0f;
    [SerializeField]
    private float BulletSpeed = 1.0f;
    private float Timer;
    public float Distance = 3f;
    public float DAMAGE = 50;

    private List<FireEnemy> fireEnemies = new List<FireEnemy>();
    private List<WaterEnemy> waterEnemies = new List<WaterEnemy>();
    private List<WindEnemy> iceEnemies = new List<WindEnemy>();
    private float MovementDir = 1;
    private void Update()
    {

        BulletMovement();
        Timer += Time.deltaTime;
        if (Timer > BulletLifetime)
        {
            StartCoroutine(DestroyBullet());
        }
    }
    public void Flip()
    {
        MovementDir *= -1;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {

            WaterEnemy Wenemy = collision.GetComponent<WaterEnemy>();
            FireEnemy Fenemy = collision.GetComponent<FireEnemy>();
            WindEnemy Ienemy = collision.GetComponent<WindEnemy>();

            if (Ienemy != null && Ienemy.HasbeenHit == false)
            {
                Ienemy.TakeStealthDamage(0);
                iceEnemies.Add(Ienemy);
            }
            if (Fenemy != null && Fenemy.HasbeenHit == false)
            {
                Fenemy.TakeStealthDamage(DAMAGE);
                fireEnemies.Add(Fenemy);
            }
            if (Wenemy != null && Wenemy.HasbeenHit == false)
            {
                Wenemy.TakeStealthDamage(0);
                waterEnemies.Add(Wenemy);
            }
        }
        ReturnAttack();
        DestroyBullet();
    }
    private void ReturnAttack()
    {
        foreach (var enemy in fireEnemies)
        {
            enemy.HasbeenHit = false;
        }
        foreach (var enemy in iceEnemies)
        { enemy.HasbeenHit = false; }
        foreach (var enemy in waterEnemies)
        {
            enemy.HasbeenHit = false;
        }

    }
    void BulletMovement()
    {
        transform.Translate(Vector3.right * BulletSpeed * MovementDir * Time.deltaTime);
    }
    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
