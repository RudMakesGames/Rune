using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ladderDetect : MonoBehaviour
{
    LadderClimb ladderClimbScript;

    private void Start()
    {
        ladderClimbScript = GameObject.Find("Player").GetComponent<LadderClimb>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
       // Debug.Log("detected smth");
        if (collision.gameObject.tag == "Player")
        {
            //ladderClimbScript.ladder = this.gameObject;
            //ladderClimbScript.GetWaypoints();
            ladderClimbScript.canClimbLadder = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("detected smth");
        if (collision.gameObject.tag == "Player")
        {
            ladderClimbScript.ladder = this.gameObject;
            ladderClimbScript.GetWaypoints();
            //ladderClimbScript.canClimbLadder = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
            ladderClimbScript.canClimbLadder = false;
    }
}
