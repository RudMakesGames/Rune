using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WaterSnareAbility : MonoBehaviour
{
    [SerializeField]
    private GameObject WaterTrap;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private ElementalPowerBar powerBar;
    [SerializeField]
    private Slider WaterSlider;
    void Start()
    {

    }
    public void ThrowWaterSnare(InputAction.CallbackContext context)
    {
        if (context.performed)
        {if(playerController.IsInWaterState)
            {
                if (WaterSlider.value >= 20)
                {
                    SpawnTrap();
                    powerBar.TakeWaterEnergy(20);
                }
            }
           
        }
    }
    private void SpawnTrap()
    {
       GameObject Snare = Instantiate(WaterTrap, playerController.FiringPoint.position, Quaternion.identity);
        if (!playerController.isFacingRight)
        {

        }
    }
    void Update()
    {

    }
}
