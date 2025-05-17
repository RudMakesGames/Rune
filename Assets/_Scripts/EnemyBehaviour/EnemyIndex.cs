using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIndex : MonoBehaviour
{
    public int EnemyIndexNo;
    public static EnemyIndex Instance;

    private void Awake()
    {
        if(Instance == null)
        Instance = this;
    }
}
