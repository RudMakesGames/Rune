using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderClimb : MonoBehaviour
{
    public GameObject ladder;
    public List<Transform> waypoints = new List<Transform>();

    public bool canClimbLadder;
    private void Start()
    {
        canClimbLadder = false;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && canClimbLadder)
            climbLadder();
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

    void climbLadder()
    {
        /*for (int i = 0; i < waypoints.Count; i++)
        {

        }*/

        //canClimbLadder = false ;
    }
}
