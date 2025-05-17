using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthManager : MonoBehaviour
{
    public static StealthManager instance;
    public bool IsPlayerSpotted;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
