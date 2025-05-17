using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public AirStealthKill airStealthKill;
    public KnifeThrow knifeThrow;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ActivateAirAssasination()
    {
        if (CurrencyManager.Instance.CurrencyCount >=4)
        {
            airStealthKill.AirStealthKillUnlocked = true;
        }
        else
        {
            Debug.Log("Not enough to but air assas");
        }

    }
    public void ActivateThrowableKnife()
    {
        if (CurrencyManager.Instance.CurrencyCount >= 6)
        {
            knifeThrow.enabled = true;
        }
        else
        {
            Debug.Log("nOT Enough to buy throwable");
        }
        
    }
}
