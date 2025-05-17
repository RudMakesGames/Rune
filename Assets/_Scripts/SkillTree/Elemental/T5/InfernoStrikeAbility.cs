using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InfernoStrikeAbility : MonoBehaviour
{
    
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private ElementalPowerBar powerBar;
    [SerializeField]
    private Slider FireSlider;
    public float AttackRange;
    public float DamageAmount;
    void Start()
    {

    }
    public void ThrowEmberTrap(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(playerController.IsInFireState)
            {
                if (FireSlider.value >= 100)
                {
                    DrawInfernoRadius();
                    powerBar.TakeFireEnergy(100);
                }
            }
           
        }
    }
    private void DrawInfernoRadius()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, AttackRange);
        foreach(Collider2D enemy in hit)
        {
            FireEnemy Fmy = enemy.GetComponent<FireEnemy>();
            WaterEnemy Wmy = enemy.GetComponent<WaterEnemy>();
            WindEnemy Imy = enemy.GetComponent<WindEnemy>();

            if(Imy != null)
            {
                Imy.TakeDamage(DamageAmount);
            }
        }
    }
    void Update()
    {

    }
}
