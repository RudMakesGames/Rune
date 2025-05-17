using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TidalWaveAbility : MonoBehaviour
{
    [SerializeField]
    private GameObject WaterWave;
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
                if (WaterSlider.value >= 60)
                {
                    SpawnBlade();
                    powerBar.TakeWaterEnergy(60);
                }
            }
           
        }
    }
    private void SpawnBlade()
    {
        GameObject TidalWave = Instantiate(WaterWave, playerController.FiringPoint.position, Quaternion.identity);
        if (!playerController.isFacingRight)
        {
            TidalWave.GetComponent<WaterProj>().Flip();
        }
    }
    void Update()
    {

    }
}
