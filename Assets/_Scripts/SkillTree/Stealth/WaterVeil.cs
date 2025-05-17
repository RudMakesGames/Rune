using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WaterVeil : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private ElementalPowerBar powerBar;
    [SerializeField]
    private Slider WaterSlider;

    public float Duration;
    void Start()
    {

    }
    public void UseWaterVeil(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (WaterSlider.value >= 40)
            {
                SpawnWaterVeil();
                powerBar.TakeWaterEnergy(40);
            }
        }
    }
    private void SpawnWaterVeil()
    {
        StartCoroutine(WaterVeilAbility());
    }
    IEnumerator WaterVeilAbility()
    {
        //add some sprite
        gameObject.layer = LayerMask.NameToLayer("Hidden");
        yield return new WaitForSeconds(Duration);
        gameObject.layer = LayerMask.NameToLayer("Player");
        //remove the sprite
    }
    void Update()
    {

    }
}
