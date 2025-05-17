using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulProj : MonoBehaviour
{

    [SerializeField]
    private float BulletLifetime = 1.0f;
    [SerializeField]
    private float BulletSpeed = 1.0f;
    private float Timer;
    public float Distance = 3f;
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
