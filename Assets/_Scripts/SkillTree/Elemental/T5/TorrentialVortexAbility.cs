using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TorrentialVortexAbility : MonoBehaviour
{
    [SerializeField]
    private GameObject Vortex;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private ElementalPowerBar powerBar;
    [SerializeField]
    private Slider WaterSlider;
    void Start()
    {

    }
    public void FireVortexes(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(playerController.IsInWaterState)
            {
                if (WaterSlider.value >= 100)
                {
                    SpawnVortex();
                    powerBar.TakeWaterEnergy(100);
                }
            }
            
        }
    }
    private void SpawnVortex()
    {
        GameObject Vortexes = Instantiate(Vortex, playerController.FiringPoint.position, Quaternion.identity);
        if (!playerController.isFacingRight)
        {
            Vortexes.GetComponent<WindProj>().Flip();
        }
    }
    void Update()
    {

    }
}
