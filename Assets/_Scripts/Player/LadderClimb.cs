using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderClimb : MonoBehaviour
{
    public GameObject ladder;
    public List<Transform> waypoints = new List<Transform>();
    bool isClimbing;
    public bool canClimbLadder;

    [SerializeField] float climbSpeed, snapDistance;

    Rigidbody2D rb;
    private void Start()
    {
        canClimbLadder = false;
        isClimbing = false;

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && canClimbLadder && !isClimbing)
            StartCoroutine(climbLadder());
    }
    /*private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.tag=="ladder")
        {
            ladder = collision.gameObject;

            GetWaypoints(ladder);
            Debug.Log("ladder found, waypoint updated");

        }
    }*/

    public void GetWaypoints()
    {
        waypoints.Clear();
        foreach (Transform t in ladder.transform)
        {
            waypoints.Add(t);
        }

        //to sort them cuz player upar niche dono se use krskta hai ladder
        waypoints.Sort((a, b) => Vector3.Distance(a.position, transform.position).CompareTo(Vector3.Distance(b.position, transform.position)));
    }

    IEnumerator climbLadder()
    {
        rb.gravityScale = 0;
        isClimbing = true;

        foreach (Transform waypoint in waypoints)
        {
            Debug.Log("moving to"+ waypoint.name);
            while (Vector3.Distance(transform.position, waypoint.position) > snapDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, waypoint.position, climbSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = waypoint.position;
            Debug.Log("reached"+ waypoint.name);
        }

        rb.gravityScale = 1;
        isClimbing = false;
    }

} 
