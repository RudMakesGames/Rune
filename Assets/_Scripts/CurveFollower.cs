#if UNITY_EDITOR
using System.Collections;
using UnityEditor;
#endif
using UnityEngine;

public class CurveFollower : MonoBehaviour
{
    public float moveDuration = 0.5f; // Time to reach enemy

    private bool isRunning = false;

    public void StartLerpToTarget(GameObject player, Transform enemyTarget)
    {
        if (!isRunning && player != null && enemyTarget != null)
        {
            StartCoroutine(LerpToTarget(player.transform, enemyTarget.position));
        }
    }

    private IEnumerator LerpToTarget(Transform player, Vector3 targetPos)
    {
        isRunning = true;

        Vector3 startPos = player.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            player.position = Vector3.Lerp(startPos, targetPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.position = targetPos;

        // Optional: trigger a stab or assassination method
        var controller = player.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.CastAirAssasination();
        }

        Animator anim = player.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("Stab");
        }

        isRunning = false;
    }
}