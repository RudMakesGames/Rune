using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyComponentCheck : MonoBehaviour
{
    public GameObject Enemy;

    private void Update()
    {
        if (Enemy == null)
        {
            Destroy(gameObject);
        }
    }
}
