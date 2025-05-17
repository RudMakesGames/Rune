using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EmberTrap : MonoBehaviour
{
    [SerializeField]
    private GameObject EmberTraps;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private ElementalPowerBar powerBar;
    [SerializeField]
    private Slider FireSlider;
    void Start()
    {

    }
    public void ThrowEmberTrap(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (FireSlider.value >= 100)
            {
                SpawnTrap();
                powerBar.TakeFireEnergy(100);
            }
        }
    }
    private void SpawnTrap()
    {
        GameObject EmberFireTraps = Instantiate(EmberTraps, playerController.FiringPoint.position, Quaternion.identity);
        if (!playerController.isFacingRight)
        {

        }
    }
    void Update()
    {

    }
}
