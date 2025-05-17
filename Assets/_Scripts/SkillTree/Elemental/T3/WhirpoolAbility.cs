using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WhirpoolAbility : MonoBehaviour
{
    [SerializeField]
    private GameObject WindProj;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private ElementalPowerBar powerBar;
    [SerializeField]
    private Slider AirSlider;
    void Start()
    {

    }
    public void FireWhirpool(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
           if(playerController.IsInWindState)
            {
                if (AirSlider.value >= 40)
                {
                    SpawnWhirpool();
                    powerBar.TakeIceEnergy(40);
                }
            }
          
        }
    }
    private void SpawnWhirpool()
    {
       GameObject Whirpools = Instantiate(WindProj, playerController.FiringPoint.position, Quaternion.identity);
        if (!playerController.isFacingRight)
        {
            Whirpools.GetComponent<WindProj>().Flip();
        }
    }
    void Update()
    {

    }

}
