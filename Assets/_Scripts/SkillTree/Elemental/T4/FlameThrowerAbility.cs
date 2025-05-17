using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlameThrowerAbility : MonoBehaviour
{
    [SerializeField]
    private GameObject Flamethrowers;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private ElementalPowerBar powerBar;
    [SerializeField]
    private UnityEngine.UI.Slider FireSlider;
    private float FlamethrowerUptime = 0.75f;
    private float energyPerSecond;
    private float FlameThrowerTimer;
    bool FlamethrowerEnabled = false;
    void Start()
    {
        energyPerSecond = 60f / FlamethrowerUptime;
    }
    public void FireFlameBursts(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(playerController.IsInFireState)
            {
                if (FireSlider.value >= 60)
                {
                    FlamethrowerEnabled = true;
                    FlameThrowerTimer = FlamethrowerUptime;
                }
            }
            
        }
    }
    private void SpawnFlamethrower()
    {
        GameObject FlameBursts = Instantiate(Flamethrowers, playerController.FiringPoint.position, Quaternion.identity);
        if (!playerController.isFacingRight)
        {
            FlameBursts.GetComponent<FireProj>().Flip();
        }
    }
    void Update()
    {
        if (FlamethrowerEnabled)
        {
            FlameThrowerTimer -= Time.deltaTime;
            float EnergyToDeduct = energyPerSecond * Time.deltaTime;
            powerBar.TakeFireEnergy(EnergyToDeduct);

            SpawnFlamethrower();

            if(FlameThrowerTimer<= 0 || FireSlider.value <= 0)
            {
                FlamethrowerEnabled=false;
            }
        }
    }
}
