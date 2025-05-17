using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutOfBounds : MonoBehaviour
{
    [SerializeField]
    private string CurrentScene;
    [SerializeField]
    private Animator SceneFade;
    public Transform respawnPoint;
    public static OutOfBounds Instance;
    public GameObject Player;
    private void Awake()
    {
        Instance = this;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            
            StartCoroutine(ReloadScene());
           
        }
    }

    IEnumerator ReloadScene()
    {
        Player.GetComponent<SpriteRenderer>().enabled = false;
        SceneFade.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneFade.SetTrigger("Start");
        Player.transform.position = respawnPoint.position;
        StealthManager.instance.IsPlayerSpotted = false;
        Player.GetComponent<SpriteRenderer>().enabled = true;
    }

    
}
