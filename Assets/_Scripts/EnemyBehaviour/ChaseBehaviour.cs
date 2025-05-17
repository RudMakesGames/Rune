using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChaseBehaviour : StateMachineBehaviour
{
    private Transform PlayerPos;
    public float MoveSpeed = 3f;
    public float JumpSpeed = 4f;
    private Rigidbody2D rb;
    private bool isFacingRight = false; // Start facing left
    public float Ythreshold = 0.2f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
        PlayerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Move towards the player's position
        animator.transform.position = Vector2.MoveTowards(animator.transform.position, PlayerPos.position, MoveSpeed * Time.deltaTime);
        if (PlayerPos.position.y > animator.transform.position.y + Ythreshold)
        {
            Jump();
        }
        // Check player movement to determine direction to face
        
    }
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, JumpSpeed * Time.deltaTime);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }





    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
