using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerDetection : MonoBehaviour
{
    [Header("References for Boxcast and CircleCast")]
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Transform castPoint;
    [SerializeField]
    private Transform GroundCheck;
    [SerializeField]
    private float detectionRadius = 1.5f; // Radius for the circle cast
    [SerializeField]
    private Vector2 groundBoxSize = new Vector2(1f, 0.25f); // Size for the ground box cast
    [SerializeField]
    private LayerMask PlayerLayer;
    [SerializeField]

    [Header("Enemy AI ref")]
    private LayerMask GroundLayer;
    public float PlayerDetectionTimer = 0;
    public bool HasSeenPlayerOnce;
    public bool isInvestigating;
    public Transform OriginalPos;
    public GameObject ExclaimationMark;
    private NotifyNearbyEnemies notifyNearbyEnemies;
    public bool isFacingLeft = true;
    private ShowSkullOnKillableEnemy showSkullOnKillableEnemy;
    private float Timer;
    public float Duration = 2f;
    private bool isInIdleState = true;
    private bool isReturning = false;
    public bool IsNotifying { get; private set; }
    public bool IsApatrollingEnemy = false;
    private EnemyAttack enemyAttack;

    [Header("Only for moving patrolling Enemies")]
    [SerializeField]
    private Transform patrolpoint1, patrolpoint2;
    private Transform currentPatrolPoint;
    [SerializeField]
    private float PatrolSpeed;
    [SerializeField]
    private float DistanceThreshold = 0.1f;

    [Header("Attack Trigger ref")]
    public GameObject AttackTrigger;
    private Coroutine attackCoroutine; // To hold the reference to the attack coroutine

    [Header("Climbing abiltity")]
    [SerializeField]
    private float ClimbingSpeed;
    private Rigidbody2D rb;
    private GameObject Player;
    public bool flip;
    [SerializeField]
    private float KnockBackForce = 0.5f;
    private Transform PlayerPos;

    [Header("Chase state ref")]
    public float ChaseSpeed = 1.5f;
    public float JumpSpeed = 4f;
    public float Ythreshold = 0.2f;
    public bool isChasing = false;

    [Header("Switchable Patrolling")]
    public bool isPatrollingSwitchable;
    private bool canSwitchToStatic;

    public float MovementVar = 1;

    [Header("Sounds")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] patrolSounds;
    [SerializeField] private AudioClip[] InvestigationSounds;
    [SerializeField] private AudioClip[] ChasingSounds;
    [SerializeField] private AudioClip[] ReturnSounds;
    [Header("Sound Cooldown")]
    [SerializeField] private float chasingSoundCooldown = 2f; // Adjust as needed
    private float chasingSoundTimer = 0f;


    void Start()
    {
        PlayerPos = GameObject.FindGameObjectWithTag("Player").transform;
        Player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        enemyAttack = GetComponent<EnemyAttack>();
        showSkullOnKillableEnemy = gameObject.GetComponent<ShowSkullOnKillableEnemy>();
        notifyNearbyEnemies = gameObject.GetComponent<NotifyNearbyEnemies>();
        if (IsApatrollingEnemy)
        {
            currentPatrolPoint = patrolpoint1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Physics2D.IgnoreLayerCollision(3, 3, true);
        Physics2D.IgnoreLayerCollision(6, 9, true);
        if (isInvestigating)
            return;
        IdleBehaviour();
        CastCircle(); // CircleCast for player detection
        
        if (HasSeenPlayerOnce)
        {
            Vector3 scale = transform.localScale;
            if (Player.transform.position.x > transform.position.x)
            {
                scale.x = Mathf.Abs(scale.x) * -1 * (flip ? -1 : 1);
            }
            else
            {
                scale.x = Mathf.Abs(scale.x) * (flip ? -1 : 1);
            }
            transform.localScale = scale;
        }
        Chase();
    }
    #region StateChanges and Abilities

    public void PlayRandomSound(AudioClip[] clips)
    {
        if(clips.Length>0 && audioSource!= null)
        {
            int randIndex = Random.Range(0, clips.Length);
            audioSource.clip = clips[randIndex];
            audioSource.Play();
        }
    }
    private void CastCircle()
    {
        Vector2 origin = castPoint.position;
        RaycastHit2D hit = Physics2D.CircleCast(origin, detectionRadius, Vector2.zero, 0f, PlayerLayer);
        RaycastHit2D groundHit = Physics2D.BoxCast(GroundCheck.position, groundBoxSize, 0f, Vector2.down, 0f, GroundLayer);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            ChangeState();
        }

        if (HasSeenPlayerOnce)
        {
            PlayerDetectionTimer += Time.deltaTime;
            if (PlayerDetectionTimer >= 4)
            {
                ReturnState();
            }
        }

        if (groundHit.collider != null && groundHit.collider.CompareTag("Ground"))
        {
            ApplyForce();
        }
    }

    public void ApplyForce()
    {
        rb.AddForce(Vector2.up * KnockBackForce, ForceMode2D.Impulse);
    }

    private void StaticEnemyPatrol()
    {
        if (isInIdleState)
        {
            Timer += Time.deltaTime;
            if (Timer >= Duration)
            {
                PlayRandomSound(patrolSounds);
                FlipLeft();
                Timer = 0;
            }
        }
    }

    private void MovingEnemyPatrol()
    {
        if (isInIdleState)
        {
            transform.position = Vector2.MoveTowards(transform.position, currentPatrolPoint.position, MovementVar *PatrolSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, currentPatrolPoint.position) < DistanceThreshold)
            {
                PlayRandomSound(patrolSounds);  
                currentPatrolPoint = (currentPatrolPoint == patrolpoint1) ? patrolpoint2 : patrolpoint1;
                FlipLeft();
            }
        }
    }

    private void IdleBehaviour()
    {
      if (!IsApatrollingEnemy)
      {
                StaticEnemyPatrol();
      }
      else
      {
            anim.SetBool("IsChasing",true);
            if(canSwitchToStatic)
            {
                StaticEnemyPatrol();
            }
            else
            {
                MovingEnemyPatrol();
            }
               
      }  
    }
    public IEnumerator Investigate(Vector2 distractionPoint)
    {
        PlayRandomSound(InvestigationSounds);
        isInvestigating = true;
        float InvestigationTimer = 3f;
        while (InvestigationTimer>0)
        {
            anim.SetBool("IsChasing", false);
            transform.position = Vector2.MoveTowards(transform.position,distractionPoint,ChaseSpeed * Time.deltaTime);
            Debug.Log("Investigation Started.");
            InvestigationTimer -= Time.deltaTime;
                yield return null;
            

        }
        
        ReturnState();
        isInvestigating = false;

    }
    private void EnableAttack()
    {
        AttackTrigger.SetActive(true);
        anim.SetTrigger("Attack");
    }

    private void DisableAttack()
    {
        AttackTrigger.SetActive(false);
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            EnableAttack();
            yield return new WaitForSeconds(2); // Attack every 2 seconds
            DisableAttack();
            if (PlayerDetectionTimer >= 4)
            {
                ReturnState();
            }
        }
    }
    public void ChangeState()
    {
        if(isPatrollingSwitchable)
        {
            if(isInvestigating)
            {
                StopCoroutine(Investigate(Vector2.zero));
                isInvestigating=false;
                Debug.Log("Investigation Stopped.");
            }
            if (IsNotifying) return;
            canSwitchToStatic = true;
            anim.SetBool("IsChasing", true);
            isChasing = true;
            IsNotifying = true;
            StealthManager.instance.IsPlayerSpotted = true;
            isInIdleState = false;
            ExclaimationMark.SetActive(true);
            HasSeenPlayerOnce = true;
            Debug.Log("Following Player");
            if (notifyNearbyEnemies != null)
            {
                notifyNearbyEnemies.NotifyEnemies();
            }
            if (showSkullOnKillableEnemy != null)
            {
                showSkullOnKillableEnemy.DeactivateIcon();
            }
            if (attackCoroutine == null) // Start the attack coroutine if it's not already running
            {
                attackCoroutine = StartCoroutine(Attack());
            }
            StartCoroutine(ResetNotifyingState());
        }
        else
        {
            if (IsNotifying) return;
            isChasing = true;
            anim.SetBool("IsChasing", true);
            IsNotifying = true;
            StealthManager.instance.IsPlayerSpotted = true;
            isInIdleState = false;
            ExclaimationMark.SetActive(true);
            HasSeenPlayerOnce = true;
            Debug.Log("Following Player");
            if (notifyNearbyEnemies != null)
            {
                notifyNearbyEnemies.NotifyEnemies();
            }
            if (showSkullOnKillableEnemy != null)
            {
                showSkullOnKillableEnemy.DeactivateIcon();
            }
            if (attackCoroutine == null) 
            {
                attackCoroutine = StartCoroutine(Attack());
            }
            StartCoroutine(ResetNotifyingState());
        }     
    }

    private IEnumerator ResetNotifyingState()
    {
        yield return new WaitForSeconds(0.5f);
        IsNotifying = false;
    }

    public void ReturnState()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
        PlayRandomSound(ReturnSounds);
        StartCoroutine(Return());
        anim.SetBool("IsChasing", false);
        ExclaimationMark.SetActive(false);
        PlayerDetectionTimer = 0f;
        isChasing = false;
        HasSeenPlayerOnce = false;
        StealthManager.instance.IsPlayerSpotted = false;
        isFacingLeft = false;
        if (IsApatrollingEnemy)
        {
            SwitchPoints();
        }
        Debug.Log("Stopped Following Player");
    }
    public void SwitchPoints()
    {
        Vector3 tempPosition = patrolpoint1.position;

        // Swap positions
        patrolpoint1.position = patrolpoint2.position;
        patrolpoint2.position = tempPosition;
    }
    private IEnumerator Return()
    {
            if (!IsApatrollingEnemy)
            {
                while (Vector2.Distance(transform.position, OriginalPos.position) > 0.5f)
                {
                    DisableAttack();
                    isReturning = true;
                Vector2 direction = OriginalPos.position - transform.position;

                // Flip the enemy to face the direction of movement
                if (direction.x > 0 && isFacingLeft)
                {
                    FlipLeft(); // Face right
                }
                else if (direction.x < 0 && !isFacingLeft)
                {
                    FlipLeft(); // Face left
                }

                // Move towards the original position
                transform.position = Vector2.MoveTowards(transform.position, OriginalPos.position, 5f * Time.deltaTime);

                yield return null;
            }

                // Once the original position is reached, transition to idle state
                isReturning = false;
                isInIdleState = true; // Ensure we set idle state to true
                StaticEnemyPatrol(); // Optionally call IdleBehaviour here if needed
            }
            else
            {
                   
                  MovingEnemyPatrol();
                 isReturning = false;
                 isInIdleState = true;
            }
    }
    private void Chase()
    {
        if (isChasing)
        {
            // Play the chasing sound only if the cooldown timer has elapsed
            if (chasingSoundTimer <= 0f)
            {
                PlayRandomSound(ChasingSounds);
                chasingSoundTimer = chasingSoundCooldown; // Reset the timer
            }

            // Countdown the timer
            chasingSoundTimer -= Time.deltaTime;

            // Move towards the player
            transform.position = Vector2.MoveTowards(transform.position, PlayerPos.position, ChaseSpeed * Time.deltaTime);

            // Perform jump if the player is above the Y-threshold
            if (PlayerPos.position.y > transform.position.y + Ythreshold)
            {
                Jump();
            }
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, JumpSpeed * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        // Draw the Player Detection Circle
        Gizmos.color = Color.red; // Color for the player detection circle
        Gizmos.DrawWireSphere(castPoint.position, detectionRadius);

        // Draw the Ground Check Box
        Gizmos.color = Color.green; // Color for the ground check box
        Gizmos.DrawWireCube(GroundCheck.position, groundBoxSize);
    }

    public void FlipLeft()
    {
        isFacingLeft = !isFacingLeft;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
    #endregion

    #region  Collisions

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Vector2 contactPoint = collision.transform.position;

            if (contactPoint.x < transform.position.x && !isFacingLeft)
            {
                FlipLeft();
            }
            else if (contactPoint.x > transform.position.x && isFacingLeft)
            {
                FlipLeft();
            }
        }
    }
    #endregion
}