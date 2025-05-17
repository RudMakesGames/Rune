using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementalPowerBar : MonoBehaviour
{
    [SerializeField]
    Slider Fire, Water, Wind, Soul;

    [SerializeField]
    float FireMaxHealth,WaterMaxHealth,WindMaxHealth,SoulMaxHealth;

    private float CurrFireHealth, CurrWaterHealth, CurrIceHealth, CurrSoulHealth;
    private float ReplenishAmt = 5;
    private float Timer = 0;
    void Start()
    {
        SetupSliders();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSliders();
        ReplenishEnergy();
    }

    void ReplenishEnergy()
    {
        if(Fire != null && Water != null && Wind != null && Soul != null )
        {
            Timer += Time.deltaTime;
            if (Timer >= 5)
            {
                Timer = 0;
            }
            else
            {
                if (CurrFireHealth < 100)
                {
                    CurrFireHealth += ReplenishAmt * Time.deltaTime;
                    if (CurrFireHealth > 100) CurrFireHealth = 100; // Clamp to max
                }

                if (CurrWaterHealth < 100)
                {
                    CurrWaterHealth += ReplenishAmt * Time.deltaTime;
                    if (CurrWaterHealth > 100) CurrWaterHealth = 100; // Clamp to max
                }

                if (CurrIceHealth < 100)
                {
                    CurrIceHealth += ReplenishAmt * Time.deltaTime;
                    if (CurrIceHealth > 100) CurrIceHealth = 100; // Clamp to max
                }

                if (CurrSoulHealth < 100)
                {
                    CurrSoulHealth += (20 / 5) * Time.deltaTime;
                    if (CurrSoulHealth > 100) CurrSoulHealth = 100; // Clamp to max
                }
            }
        }
        
    }

    public void TakeFireEnergy(float Energy)
    {
       
        CurrFireHealth -= Energy;
    }
    public void TakeWaterEnergy(float Energy)
    {
        CurrWaterHealth -= Energy;
    }
    public void TakeIceEnergy(float Energy)
    {
        CurrIceHealth -= Energy;
    }
    public void TakeSoulEnergy(float Energy)
    {
        CurrSoulHealth -= Energy;
    }

    private void UpdateSliders()
    {
        if (Fire != null && Water != null && Wind != null && Soul != null)
        {
            Fire.value = CurrFireHealth;
            Water.value = CurrWaterHealth;
            Wind.value = CurrIceHealth;
            Soul.value = CurrSoulHealth;
        }

    }

    private void SetupSliders()
    {
        CurrFireHealth = FireMaxHealth;
        CurrWaterHealth = WaterMaxHealth;
        CurrIceHealth =  WindMaxHealth;
        CurrSoulHealth = SoulMaxHealth;
    }
}
