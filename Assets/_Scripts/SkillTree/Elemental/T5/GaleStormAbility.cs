using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GaleStormAbility : MonoBehaviour
{
    [SerializeField]
    private GameObject Storm;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private ElementalPowerBar powerBar;
    [SerializeField]
    private Slider AirSlider;
    void Start()
    {

    }
    public void FireGaleStorm(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(playerController.IsInWindState)
            {
                if (AirSlider.value >= 100)
                {
                    SpawnStorm();
                    powerBar.TakeIceEnergy(100);
                }
            }
           
        }
    }
    private void SpawnStorm()
    {
        GameObject Whirpools = Instantiate(Storm, playerController.FiringPoint.position, Quaternion.identity);
        if (!playerController.isFacingRight)
        {

        }
    }
    void Update()
    {

    }
}
