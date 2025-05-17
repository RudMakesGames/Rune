using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine;

public class KnifeThrow : MonoBehaviour
{
    public GameObject KnifePrefab;
    public Transform FiringPoint;
    private PlayerController playerController;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    
    void Update()
    {
        
    }
    public void ThrowHomingKnife(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(StealthManager.instance.IsPlayerSpotted)
            {
                if (ThrowableKnifeManager.Instance.canThrowKnife)
                {
                    ThrowableKnifeManager.Instance.RefreshAbility();
                    StartCoroutine(SlowDownTime());
                    GameObject ThrowableKnife = Instantiate(KnifePrefab, FiringPoint.position, Quaternion.identity);
                    if (!playerController.isFacingRight)
                    {
                        ThrowableKnife.GetComponent<Knife>().Flip();
                    }
                }
            }
 
        }
    }
    IEnumerator SlowDownTime()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(2);
        Time.timeScale = 1f;
    }
}
