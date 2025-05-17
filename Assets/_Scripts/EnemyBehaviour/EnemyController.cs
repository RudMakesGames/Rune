using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyController : MonoBehaviour
{
    #region Movement References
    private Rigidbody2D rb;
    private float horizontal;
    private float vertical;

    [SerializeField] private float JumpSpeed;
    [SerializeField] private float RunningSpeed;
    [SerializeField] private float ClimbingSpeed;
    [SerializeField] private float WalkSpeed; // This can be removed if not used.

    private bool isFacingRight = true;
    private bool CanClimb;
    [SerializeField] private Transform GroundCheck;
    public LayerMask groundLayer;

    [SerializeField] private Animator Anim;
    #endregion

    #region Moving
    private void Start()
    {
       rb = GetComponent<Rigidbody2D>();
    }
    public void MoveHorizontally(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }

    public void MoveVertically(InputAction.CallbackContext context)
    {
        if (CanClimb)
        {
            vertical = context.ReadValue<Vector2>().y;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpSpeed);
            Anim.SetTrigger("Jump");
        }
    }

    private void FlipChecks()
    {
        if (!isFacingRight && horizontal > 0f)
        {
            Flip();
        }
        else if (isFacingRight && horizontal < 0f)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
    #endregion

    #region Attacking References
    [SerializeField] private Transform FiringPoint;
    [SerializeField] private float PunchDelay = 1f;
    private float PunchDelayTimer;
    [SerializeField] private float AttackRange = 0.3f;
    public LayerMask EnemyLayer;
    private float DamageAmountToEnemies = 100;
    #endregion

    #region Attacking
    public void Punch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (PunchDelayTimer >= PunchDelay)
            {
                Anim.SetTrigger("Attack");
                Collider2D[] HitEnemy = Physics2D.OverlapCircleAll(FiringPoint.position, AttackRange, EnemyLayer);
                foreach (Collider2D enemy in HitEnemy)
                {
                    enemy.GetComponent<BossHealth>().TakeDamage(DamageAmountToEnemies);
                }

                PunchDelayTimer = 0;
            }
        }
    }
    #endregion

    void Update()
    {
        // Movement control
        if (CanClimb)
        {
            rb.velocity = new Vector2(rb.velocity.x, vertical * ClimbingSpeed);
            rb.gravityScale = 0f;
        }
        else
        {
            rb.gravityScale = 1f;
            rb.velocity = new Vector2(horizontal * RunningSpeed, rb.velocity.y);
        }

        FlipChecks();
    }
    private void FixedUpdate()
    {
        if (Mathf.Abs(horizontal) > 0f)
        {
            Anim.SetBool("IsRunning", true);
        }
        else
        {
            Anim.SetBool("IsRunning", false);
        }
        if (isGrounded())
        {
            Anim.SetBool("IsFalling", false);
        }
        else
        {
            Anim.SetBool("IsFalling", true);
        }
    }
    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(GroundCheck.position, 0.2f, groundLayer);
    }
}
