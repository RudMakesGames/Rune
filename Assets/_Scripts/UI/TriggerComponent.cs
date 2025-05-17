using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerComponent : MonoBehaviour
{
    [SerializeField]
    private GameObject ObjectToSetActive;

    private void Awake()
    {
        gameObject.GetComponent<Collider2D>().isTrigger = true;
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            ObjectToSetActive.SetActive(true);
            EventManager.Instance.SetEvent();
        }
    }
}
