using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlameBurstAbility : MonoBehaviour
{
    [SerializeField]
    private GameObject FlameProj;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private ElementalPowerBar powerBar;
    [SerializeField]
    private UnityEngine.UI.Slider FireSlider;
    void Start()
    {

    }
    public void FireFlameBursts(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(playerController.IsInFireState)
            {
                if (FireSlider.value >= 40)
                {
                    SpawnFlame();
                    powerBar.TakeFireEnergy(40);
                }
            }
            
        }
    }
    private void SpawnFlame()
    {
      GameObject FlameBursts = Instantiate(FlameProj, playerController.FiringPoint.position, Quaternion.identity);
      if(!playerController.isFacingRight)
        {
            FlameBursts.GetComponent<FireProj>().Flip();
        }
    }
    void Update()
    {

    }
}
