using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

   
    public TextMeshProUGUI CurrencyText;

    
    public int CurrencyCount;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        UpdateCurrencyText();
    }
    public void  GainCurrency(int  currency)
    {
        CurrencyCount += currency;
        UpdateCurrencyText();
    }
    public void SpendCurrency(int currency)
    {
        if (CurrencyCount >= currency) // Check if there's enough currency
        {
            CurrencyCount -= currency;
            UpdateCurrencyText(); // Update the UI
        }
        else
        {
            Debug.LogWarning("Not enough currency!");
        }
    }
    // Update is called once per frame
    public void UpdateCurrencyText()
    {
       CurrencyText.text = CurrencyCount.ToString();
    }
}
