using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FireTrapAbility : MonoBehaviour
{
    [SerializeField]
    private GameObject Trap;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private ElementalPowerBar powerBar;
    [SerializeField]
    private Slider FireSlider;
    void Start()
    {
        
    }
    public void ThrowFireTrap(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(playerController.IsInFireState == true)
            {
                if (FireSlider.value >= 20)
                {
                    SpawnTrap();
                    powerBar.TakeFireEnergy(20);
                }
            }
           
        }
    }
    private void SpawnTrap()
    {
       GameObject FireTraps = Instantiate(Trap, playerController.FiringPoint.position, Quaternion.identity);
        if (!playerController.isFacingRight)
        {
           
        }
    }
    void Update()
    {
        
    }
}
