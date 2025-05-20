#if UNITY_EDITOR
using System.Collections;
using UnityEditor;
#endif
using UnityEngine;

public class CurveFollower : MonoBehaviour
{
    public Transform[] curvePoints; // Assigned in Inspector as children
    public float moveDuration = 1.0f;
    private bool isRunning = false;

    public void StartCurveFollow(GameObject player)
    {
        if (!isRunning && player != null)
            StartCoroutine(FollowCurve(player.transform));
    }

    private IEnumerator FollowCurve(Transform player)
    {
        isRunning = true;

        for (int i = 0; i < curvePoints.Length - 1; i++)
        {
            Vector3 start = curvePoints[i].position;
            Vector3 end = curvePoints[i + 1].position;
            float t = 0f;

            while (t < 1f)
            {
                player.position = Vector3.Lerp(start, end, t);
                t += Time.deltaTime / (moveDuration / (curvePoints.Length - 1));
                yield return null;
            }

            player.position = end;
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.CastKill();
        }

        isRunning = false;

        // Play stab animation on player
        Animator playerAnim = player.GetComponent<Animator>();
        if (playerAnim != null)
        {
            playerAnim.SetTrigger("Stab");
        }
        else
        {
            Debug.Log("No animator found");
        }
    }

    // Scene view curve visualization
    private void OnDrawGizmos()
    {
        if (curvePoints == null || curvePoints.Length < 2)
            return;

        Gizmos.color = Color.red;

        for (int i = 0; i < curvePoints.Length - 1; i++)
        {
            if (curvePoints[i] != null && curvePoints[i + 1] != null)
            {
                Gizmos.DrawLine(curvePoints[i].position, curvePoints[i + 1].position);
                Gizmos.DrawSphere(curvePoints[i].position, 0.05f);
            }
        }

        Gizmos.DrawSphere(curvePoints[curvePoints.Length - 1].position, 0.05f);
    }
}