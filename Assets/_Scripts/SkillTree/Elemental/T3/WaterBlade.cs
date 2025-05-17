using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WaterBladeAbility : MonoBehaviour
{
    [SerializeField]
    private GameObject WaterProj;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private ElementalPowerBar powerBar;
    [SerializeField]
    private Slider WaterSlider;
    void Start()
    {

    }
    public void ThrowWaterBlade(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(playerController.IsInWaterState)
            {
                if (WaterSlider.value >= 40)
                {
                    SpawnBlade();
                    powerBar.TakeWaterEnergy(40);
                }
            }
         
        }
    }
    private void SpawnBlade()
    {
       GameObject WaterBlades = Instantiate(WaterProj, playerController.FiringPoint.position, Quaternion.identity);
        if (!playerController.isFacingRight)
        {
            WaterBlades.GetComponent<WaterBladeproj>().Flip();
        }
    }
    void Update()
    {

    }
}
