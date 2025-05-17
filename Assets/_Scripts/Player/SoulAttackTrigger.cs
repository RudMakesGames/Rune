using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulAttackTrigger : MonoBehaviour
{
    private int EnemyIndex;
    public bool IsInRangeToPosses;
    public CinemachineVirtualCamera VirtualCamera;
    [SerializeField]
    private GameObject EnemyPawn1;
    [SerializeField]
    private GameObject EnemyPawn2;
    [SerializeField]
    private GameObject EnemyPawn3;

    [SerializeField]
    private GameObject Player;
    public PlayerController characterController;
    private bool hasPossesedEnemy;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            IsInRangeToPosses = true;
            
            if(hasPossesedEnemy)
            {
                EnemyIndex = collision.gameObject.GetComponent<EnemyIndex>().EnemyIndexNo;
                Destroy(collision.gameObject);
            }
        }
        if (collision.gameObject.CompareTag("StrongEnemy") && PossesStrongerFoes.canPossesStrongerFoes)
        {
            IsInRangeToPosses = true;
            
            if (hasPossesedEnemy)
            {
                EnemyIndex = collision.gameObject.GetComponent<EnemyIndex>().EnemyIndexNo;
                Destroy(collision.gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            IsInRangeToPosses = false;
            if (hasPossesedEnemy)
            {
                Destroy(collision.gameObject);
            }
        }
        if (collision.gameObject.CompareTag("StrongEnemy") && PossesStrongerFoes.canPossesStrongerFoes)
        {
            IsInRangeToPosses = false;
            if (hasPossesedEnemy)
            {
                Destroy(collision.gameObject);
            }
        }

    }
    public void Posses()
    {
        if (EnemyIndex == 1)
        {
            StartCoroutine(InstantiateEnemyPawn1());
        }
        if (EnemyIndex == 2)
        {
            StartCoroutine(InstantiateEnemyPawn2());
        }
        if(EnemyIndex == 3)
        {
            StartCoroutine(InstantiateEnemyPawn3());
        }
        
    }
    IEnumerator InstantiateEnemyPawn1()
    {
        //add effects
        hasPossesedEnemy = true;
        yield return new WaitForSeconds(1.5f);
        Player.SetActive(false);
        GameObject SpawnedEnemy = Instantiate(EnemyPawn1,transform.position,Quaternion.identity);
        VirtualCamera.Follow = SpawnedEnemy.transform;

    }
    IEnumerator InstantiateEnemyPawn2()
    {
        //add effects
        hasPossesedEnemy = true;
        yield return new WaitForSeconds(1.5f);
        Player.SetActive(false);
        GameObject SpawnedEnemy = Instantiate(EnemyPawn2, transform.position, Quaternion.identity);
        VirtualCamera.Follow = SpawnedEnemy.transform;

    }
    IEnumerator InstantiateEnemyPawn3()
    {
        //add effects
        hasPossesedEnemy = true;
        yield return new WaitForSeconds(1.5f);
        Player.SetActive(false);
        GameObject SpawnedEnemy = Instantiate(EnemyPawn3, transform.position, Quaternion.identity);
        VirtualCamera.Follow = SpawnedEnemy.transform;

    }
}
