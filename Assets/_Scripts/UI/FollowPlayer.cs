using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;      // Assign the player's transform here.
    public Vector3 offset = new Vector3(0, 1, 0); // Offset for positioning the canvas above the player.

    private void Update()
    {
        // Follow the player with offset
        transform.position = player.position + offset;

        // Keep the canvas facing forward to avoid flipping with the player
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
