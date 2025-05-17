using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBodyLifeTime : MonoBehaviour
{
    [SerializeField]
    float LifeTime;
    private float Timer;
    private void Start()
    {
        LifeTime = Random.Range(2, 4);
    }

    void Update()
    {
        Timer += Time.deltaTime;
        if(Timer > LifeTime)
        {
            Destroy(gameObject);
        }
    }
}
