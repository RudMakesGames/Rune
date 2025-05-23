using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Movement & Abilities
    [Header("GameState")]
    [SerializeField] private bool isBossFight;
    public static bool isHittable = true;
    private Rigidbody2D rb;
    private float horizontal;
    private float vertical;
    [Header("PlayerController")]
    [SerializeField] private float JumpSpeed;
    [SerializeField] private float RunningSpeed;
    [SerializeField] private float ClimbingSpeed;
    [SerializeField] private float CrouchSpeed;

    [Header("Abilities")]
    [SerializeField] private GameObject FireIcon, WaterIcon, WindIcon, SoulIcon;
    public bool IsInFireState, IsInWaterState, IsInWindState;
    [SerializeField] private Transform GroundCheck;
    [SerializeField] public Transform FiringPoint;
    public LayerMask groundLayer;
    public LayerMask HitableLayer;

    [SerializeField]
    private RuntimeAnimatorController Normalcontroller;
    [SerializeField]
    private RuntimeAnimatorController Crouchcontroller;
    

    private bool isCrouching;
    public bool isFacingRight = true;
    private Element CurrentElement = Element.None;
    #endregion
    public Transform PlayerPos;
    [SerializeField]
    private Animator Anim;
    #region References & BossFight
    [Header("References")]
    [SerializeField]
    private CapsuleCollider2D capsuleColl2D;
    [SerializeField]
    private BoxCollider2D boxCollider2D;
    [SerializeField]
    private GameObject FireProjectile, WaterProjectile, IceProjectile, SoulProjectile;
    [SerializeField]
    public float ShootingDelay = 0.5F;
    public float ShootingDelayTimer;
    public bool IsLadder;
    public bool CanClimb;
    [Header("Slider References")]
    [SerializeField]
    private GameObject FireSlider, WaterSlider, WindSlider, SoulSlider;

    private ElementalPowerBar elementalPowerBar;


    [Header("BossFight")]
    [SerializeField] private float PunchDelay = 1f;
    private float PunchDelayTimer;
    [SerializeField] private float AttackRange = 0.3f;
    [SerializeField] private LayerMask BossLayer;

    [Header("Dodge Roll")]
    [SerializeField] private float dodgeSpeed = 15f;
    [SerializeField] private float dodgeDuration = 0.5f;
    [SerializeField] private float dodgeCooldown = 1f;
    [SerializeField] private float DamageAmountToBoss = 100;
    private bool isDodging = false;
    private float dodgeCooldownTimer = 0f;

    [SerializeField]
    private SoulAttackTrigger trigger;
    #endregion

    [SerializeField]
    float SoundCircleRadius;
    [SerializeField]
    GameObject Collider;
    bool hasSneakActivated = false;
    bool hasJumped = false;
    private int jumpCount = 0;
    public int maxJumpCount = 2;
    private DoubleJump doubleJump;
    private bool IsClimbingMoving;



    [Header("Stealth Kill")]
    [SerializeField]
    private Transform AirAssasinationTransform;
    [SerializeField]
    float StealthKillRange = 5f;
    bool isKilling = false;
    [SerializeField]
    float CircleRadius = 1f;
    [SerializeField] private Vector2 capsuleSize = new Vector2(1f, 1.5f); // Width and Height of the capsule
    [SerializeField] private Vector2 capsuleOffset = new Vector2(0f, -0.5f);
    bool IsInRangeToKill;
    Transform EnemyPos;

    [SerializeField]
    AudioClip KillSfx;

    [SerializeField]
    float AerialStealthKillRange = 2f;
    private GameObject _currentEnemyTarget;

    bool IsAllowedToAirAssasinate = false;


    public GameObject SkillTree;
    private bool isSkillTreeOpen;
    public AudioSource Walksfx;
    [SerializeField] private float footstepInterval = 0.5f; // Set this to the desired interval between steps
    private float footstepTimer = 0f;
    [SerializeField] private AudioSource landingSfx;
    private bool wasGrounded = false;
    [SerializeField] private float minPitch = 0.9f; 
    [SerializeField] private float maxPitch = 1.1f;

    [Header("Distraction Prefab")]
    public GameObject DistractionPrefab;
    public float DistractionCooldown = 5f;
    private float DistractionTimer = 0f;
    public float ThrowForce = 500f;
    Vector3 ThrownDirection = Vector3.right * 0.5f;

    private ShowSkullOnKillableEnemy _previousSkullTarget;


    [Header("Camera Effects")]
    [SerializeField]
    private CinemachineVirtualCamera CinemachineVirtualCamera;
    [SerializeField]
    private float OriginalCamSize = 4.3f, CloseUpCamSize = 1.95f , Duration = 0.5f;
    [SerializeField]
    private GameObject CinematicBars;
    [SerializeField]
    RedFilterEffect redFilterEffect;
    public enum Element
    {
        None,
        Fire,
        Water,
        Wind,
        Soul
    }
    private void Awake()
    {
        boxCollider2D.enabled = false;
        capsuleColl2D.enabled = true;
    }
    void Start()
    {
        doubleJump = gameObject.GetComponent<DoubleJump>();
        Anim.runtimeAnimatorController = Normalcontroller;
        rb = GetComponent<Rigidbody2D>();
        elementalPowerBar = GetComponent<ElementalPowerBar>();
    }
    private void FixedUpdate()
    {
        if (EventManager.Instance.isEventActive)
        {
            rb.velocity = Vector2.zero; // Stops any residual velocity
            return;
        }
        if (CanClimb)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * ClimbingSpeed);
        }
        else
        {
            rb.gravityScale = 1f;
        }
        
    }
    void Update()
    {
       
        CheckForAirKill();

        Vector2 direction = new Vector2(horizontal, vertical);
        Anim.SetFloat("VerticalAxis",Mathf.Abs(direction.y));
        if (EventManager.Instance.isEventActive)
        {
            horizontal = 0f;
            vertical = 0f;
            rb.velocity = Vector2.zero;

            Anim.SetBool("IsRunning", false);
            Anim.SetBool("IsFalling", false);
            Anim.ResetTrigger("Jump");
            Anim.ResetTrigger("Attack");
            return;
        }
        // Tutorial 
        if (hasSneakActivated)
        {
            Destroy(Collider);
        }
        //Climbing
        if (Mathf.Abs(vertical) > 0f && IsLadder)
        {
            CanClimb = true;

        }
        else
        {
            CanClimb = false;

        }
        //Climb End
       
        
        
        
        CheckForStealthKill();
        if (Mathf.Abs(horizontal) > 0f && isGrounded())
        {
            Anim.SetBool("IsRunning", true);
            if (!isCrouching)
            {
                footstepTimer += Time.deltaTime;

                if (footstepTimer >= footstepInterval)
                {
                    PlayFootstepSound();
                    footstepTimer = 0f;
                }
                DrawSoundCircle();
            }

        }
        else
        {
            Anim.SetBool("IsRunning", false);
            footstepTimer = 0f;
        }

        if (isBossFight) return;
        //Jump Checks
        if (isGrounded())
        {
            Anim.SetBool("IsFalling", false);
            IsAllowedToAirAssasinate = false;
        }
        else
        {
            Anim.SetBool("IsFalling", true);
            IsAllowedToAirAssasinate = true;
        }
        //Dodge Timer
        if (dodgeCooldownTimer > 0)
        {
            dodgeCooldownTimer -= Time.deltaTime;
        }
        if (isBossFight)
        {
            PunchDelayTimer += Time.deltaTime;
        }
        ShootingDelayTimer -= Time.deltaTime;

        //Crouch Movement Changes
        if (isCrouching)
        {
            rb.velocity = new Vector2(horizontal * CrouchSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(horizontal * RunningSpeed, rb.velocity.y);
        }

        //Distraction Object
        if(DistractionTimer > 0)
        {
            DistractionTimer -= Time.deltaTime;
        }
        FlipChecks();
        wasGrounded = isGrounded();


    }
    private IEnumerator LerpOrthoLensSize(float TargetSize, float time)
    {
        float startSize = CinemachineVirtualCamera.m_Lens.OrthographicSize;
        float elapsed = 0f;

        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / time;
            CinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startSize, TargetSize, t);
            yield return null;
        }
        CinemachineVirtualCamera.m_Lens.OrthographicSize = TargetSize;
    }
    private void CloseUpCamera()
    {
        StartCoroutine(LerpOrthoLensSize(CloseUpCamSize,Duration));
    }
    private void ResetCamera()
    {
        StartCoroutine(LerpOrthoLensSize(OriginalCamSize, Duration));
    }
    private IEnumerator StealthKillCinematicEffect()
    {
        CloseUpCamera();
        redFilterEffect?.EnableRedFilter();
        CinematicBars?.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        ResetCamera();
        redFilterEffect?.DisableRedFilter();
        CinematicBars?.SetActive(false);
    }
    #region StealthKill
    private void CheckForStealthKill()
    {
        if(isCrouching)
        {
            if(!StealthManager.instance.IsPlayerSpotted)
            {
                RaycastHit2D hit = Physics2D.CircleCast(FiringPoint.position, CircleRadius, Vector2.right, StealthKillRange, HitableLayer);

                if (hit.collider != null && hit.collider.TryGetComponent<IDamageable>(out var healthComponent))
                {
                    if (hit.collider.TryGetComponent<ShowSkullOnKillableEnemy>(out var skull))
                    {
                        // If we're looking at a new target, deactivate the old icon
                        if (_previousSkullTarget != null && _previousSkullTarget != skull)
                        {
                            _previousSkullTarget.DeactivateIcon();
                        }

                        skull.ActivateIcon();
                        _previousSkullTarget = skull;
                        IsInRangeToKill = true;
                        EnemyPos = skull.StealthKillPosition;
                    }
                }
                else
                {
                    
                    if (_previousSkullTarget != null)
                    {
                        _previousSkullTarget.DeactivateIcon();
                        _previousSkullTarget = null;
                    }
                    IsInRangeToKill = false;
                    EnemyPos = null;
                }
            }
           
        }
       
    }
    private void CheckForAirKill()
    {
        Vector2 origin = (Vector2)AirAssasinationTransform.position + capsuleOffset;
        if (IsAllowedToAirAssasinate)
        {
            if (!StealthManager.instance.IsPlayerSpotted)

               
            {
                RaycastHit2D hit2d = Physics2D.CapsuleCast(origin, capsuleSize,CapsuleDirection2D.Vertical,0f,Vector2.down, AerialStealthKillRange, HitableLayer);
                
                if (hit2d.collider != null && hit2d.collider.TryGetComponent<IDamageable>(out var healthComponent))
                {
                    
                    if (hit2d.collider.TryGetComponent<ShowSkullOnKillableEnemy>(out var skull))
                    {
                      
                        // If we're looking at a new target, deactivate the old icon
                        if (_previousSkullTarget != null && _previousSkullTarget != skull)
                        {
                            _previousSkullTarget.DeactivateIcon();
                        }

                        skull.ActivateIcon();
                        _previousSkullTarget = skull;
                        IsInRangeToKill = true;
                        EnemyPos = skull.AirAssasinationTransform;


                        _currentEnemyTarget = hit2d.collider.gameObject;

                    }
                }
                else
                {

                    if (_previousSkullTarget != null)
                    {
                        _previousSkullTarget.DeactivateIcon();
                        _previousSkullTarget = null;
                    }

                    IsInRangeToKill = false;
                    EnemyPos = null;
                    _currentEnemyTarget = null;
                }
            }
        }
    }
    public void ConfirmAirAssasination(InputAction.CallbackContext context)
    {
        
        
            if(IsAllowedToAirAssasinate)
            {
                if (IsInRangeToKill && context.performed)
                {
                    if (!isKilling && _currentEnemyTarget != null)
                    {
                    StartCoroutine(StealthKillCinematicEffect());
                        Debug.Log("air assasination performed");
                        isKilling = true;
                        _currentEnemyTarget.GetComponent<CurveFollower>()?.StartLerpToTarget(gameObject, EnemyPos);

                        StartCoroutine(PeformKill());

                    }

                }
            }
           
        
    }
    public void ConfirmStealthKill(InputAction.CallbackContext context)
    {
        if(isCrouching)
        {
            if (IsInRangeToKill && context.performed)
            {
                if(!isKilling)
                {
                    isKilling = true;
                    StartCoroutine(StealthKillCinematicEffect());
                    transform.position = EnemyPos.position;
                    Anim.SetTrigger("Stab");
                    StartCoroutine(PeformKill());

                }
               
            }
        }
       
    }
    private IEnumerator PeformKill()
    {
        yield return new WaitForSeconds(2);
        isKilling = false;  
    }
    public void CastKill()
    {
        
        
            //used as an animation event
            RaycastHit2D hit = Physics2D.CircleCast(FiringPoint.position, CircleRadius, Vector2.right, StealthKillRange, HitableLayer);

            if (hit.collider != null && hit.collider.TryGetComponent<IDamageable>(out var healthComponent))
            {
                hit.collider.TryGetComponent<WindEnemy>(out var windenemy);
                hit.collider.TryGetComponent<FireEnemy>(out var fireEnemy);
                hit.collider.TryGetComponent<WaterEnemy>(out var waterenemy);
                
                waterenemy?.HandleDeath();
                windenemy?.HandleDeath();
                fireEnemy?.HandleDeath();


            }
        
     
    }
    public void CastAirAssasination()
    {


        //used as an animation event
        RaycastHit2D hit = Physics2D.CircleCast(FiringPoint.position, CircleRadius, Vector2.right, StealthKillRange, HitableLayer);

        if (hit.collider != null && hit.collider.TryGetComponent<IDamageable>(out var healthComponent))
        {
            hit.collider.TryGetComponent<WindEnemy>(out var windenemy);
            hit.collider.TryGetComponent<FireEnemy>(out var fireEnemy);
            hit.collider.TryGetComponent<WaterEnemy>(out var waterenemy);

            waterenemy?.HandleDeath();
            windenemy?.HandleDeath();
            fireEnemy?.HandleDeath();


        }


    }
    public void PlayStabSoundEffect()
    {
        AudioManager.instance.PlaySoundFXClip(KillSfx, transform, 0.4f, Random.Range(0.9f, 1));
    }
    #endregion
    public void PlayFootstepSound()
    {
        Walksfx.pitch = Random.Range(minPitch, maxPitch);
        Walksfx.Play();
    }
    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(GroundCheck.position, 0.2f, groundLayer);

    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
    public void PlayLandSoundSfx()
    {
        landingSfx.Play();
    }
    #region Moving
    public void MoveHorizontally(InputAction.CallbackContext context)
    {
        if (EventManager.Instance.isEventActive) return;
        if(!CanClimb)
        {
            horizontal = context.ReadValue<Vector2>().x;
        }
       

    }
    public void MoveVertically(InputAction.CallbackContext context)
    {
        if (EventManager.Instance.isEventActive) return;
        vertical = context.ReadValue<Vector2>().y;
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (EventManager.Instance.isEventActive) return;
        if (context.performed)
        {
            if (doubleJump.isDoubleJumpAllowed)
            {
                if (isGrounded() || jumpCount < maxJumpCount) // Allow jump if grounded or has a jump available
                {
                    rb.velocity = new Vector2(rb.velocity.x, JumpSpeed);
                    Anim.SetTrigger("Jump");

                    // Increment jump count if not grounded
                    if (!isGrounded())
                    {
                        jumpCount++;
                    }
                    else
                    {
                        jumpCount = 1; // Reset to 1 if the player is grounded
                    }

                    hasJumped = true;
                }
            }
            else
            {
                if (isGrounded())
                {
                    rb.velocity = new Vector2(rb.velocity.x, JumpSpeed);
                    Anim.SetTrigger("Jump");
                }
            }

        }
    }


    #endregion
    #region Abilities
    public void OpenSkillTree(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!isSkillTreeOpen)
            {
                SkillTree.SetActive(true);
                EventManager.Instance.isEventActive = true;
                isSkillTreeOpen = true;
            }
            else
            {
                SkillTree.SetActive(false);
                EventManager.Instance.isEventActive = false;
                isSkillTreeOpen = false;
            }


        }
    }
    private void ResetAllIcons()
    {
        IsInFireState = false;
        IsInWaterState = false;
        IsInWindState = false;
        FireIcon?.SetActive(false);
        FireSlider?.SetActive(false);
        WaterIcon?.SetActive(false);
        WaterSlider?.SetActive(false);
        WindIcon?.SetActive(false);
        WindSlider?.SetActive(false);
        SoulIcon?.SetActive(false);
        SoulSlider?.SetActive(false);

    }
    public void Sneak(InputAction.CallbackContext context)
    {
        if (EventManager.Instance.isEventActive) return;
        if (context.performed)
        {
            Anim.runtimeAnimatorController = Crouchcontroller;
            isCrouching = true;
            boxCollider2D.enabled = true;
            capsuleColl2D.enabled = false;
            hasSneakActivated = true;
            Debug.Log("In Stealth");
        }
        else if (context.canceled)
        {
            Anim.runtimeAnimatorController = Normalcontroller;
            isCrouching = false;
            boxCollider2D.enabled = false;
            capsuleColl2D.enabled = true;
            Debug.Log("Out of Stealth");
        }
    }
    public void ThrowDistraction(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            GameObject DistactionObject = Instantiate(DistractionPrefab, FiringPoint.position, Quaternion.identity);
            Rigidbody2D ObjectRb = DistactionObject.GetComponent<Rigidbody2D>();
            Vector3 throwDirection = FiringPoint.forward + Vector3.up * 0.5f + ThrownDirection;
            ObjectRb.AddForce(throwDirection.normalized * ThrowForce);
            DistractionTimer = DistractionCooldown;

        }
    }
    public void Punch(InputAction.CallbackContext context)
    {
        if (EventManager.Instance.isEventActive) return;
        if (context.performed)
        {
            if (PunchDelayTimer >= PunchDelay)
            {
                Anim.SetTrigger("Attack");
                Collider2D[] HitEnemy = Physics2D.OverlapCircleAll(FiringPoint.position, AttackRange, BossLayer);
                foreach (Collider2D enemy in HitEnemy)
                {
                    enemy.GetComponent<BossHealth>().TakeDamage(DamageAmountToBoss);
                }

                PunchDelayTimer = 0;
            }

        }
    }
    public void Dodge(InputAction.CallbackContext context)
    {
        if (EventManager.Instance.isEventActive) return;
        if (context.performed && !isDodging && dodgeCooldownTimer <= 0f)
        {
            StartCoroutine(DodgeRoll());
        }
    }
    private void DrawSoundCircle()
    {
        if (EventManager.Instance.isEventActive) return;
        Collider2D[] Hit = Physics2D.OverlapCircleAll(transform.position, SoundCircleRadius);
        foreach (Collider2D enemy in Hit)
        {
            if (enemy.GetComponent<PlayerDetection>() != null)
            {
                PlayerDetection playerDetection = enemy.GetComponent<PlayerDetection>();
                if (playerDetection.isFacingLeft == false)
                {
                    playerDetection.ChangeState();
                    playerDetection.FlipLeft();
                }
                else
                {
                    playerDetection.ChangeState();

                }

            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 origin = (Vector2)AirAssasinationTransform.position + capsuleOffset;
        Gizmos.DrawWireSphere(FiringPoint.position, AttackRange);
        Gizmos.DrawWireCube(origin + Vector2.down * (AerialStealthKillRange / 2f), new Vector3(capsuleSize.x, AerialStealthKillRange, 0.1f));
        Gizmos.DrawWireSphere(FiringPoint.position, CircleRadius);
        Gizmos.DrawWireSphere(transform.position, SoundCircleRadius);
    }
    public void FireElement(InputAction.CallbackContext context)
    {
        if (EventManager.Instance.isEventActive) return;
        if (isBossFight) return;
        if (context.performed)
        {
            ResetAllIcons();
            IsInFireState = true;
            CurrentElement = Element.Fire;
            
            FireIcon?.SetActive(true);
            FireSlider?.SetActive(true);
            Debug.Log("SWITCHED TO FIRE");
        }
    }
    public void WaterElement(InputAction.CallbackContext context)
    {
        if (EventManager.Instance.isEventActive) return;
        if (isBossFight) return;
        if (context.performed)
        {
            ResetAllIcons();
            IsInWaterState = true;
            CurrentElement = Element.Water;
            
            WaterIcon?.SetActive(true);
            WaterSlider?.SetActive(true);
            Debug.Log("SWITCHED TO WATER");
        }
    }
    public void WindElement(InputAction.CallbackContext context)
    {
        if (EventManager.Instance.isEventActive) return;
        if (isBossFight) return;
        if (context.performed)
        {
            ResetAllIcons();
            IsInWindState = true;
            CurrentElement = Element.Wind;
           
            WindIcon?.SetActive(true);
            WindSlider?.SetActive(true);
            Debug.Log("SWITCHED TO ICE");
        }
    }
    public void SoulElement(InputAction.CallbackContext context)
    {
        if (EventManager.Instance.isEventActive) return;
        if (isBossFight) return;
        if (context.performed)
        {
            CurrentElement = Element.Soul;
            ResetAllIcons();
            SoulIcon?.SetActive(true);
            SoulSlider?.SetActive(true);
            Debug.Log("SWITCHED TO Soul");
        }
    }
    public void Fire(InputAction.CallbackContext context)
    {
        if (EventManager.Instance.isEventActive) return;
        if (isBossFight) return;
        Slider fireSlider = FireSlider.GetComponent<Slider>();
        Slider waterSlider = WaterSlider.GetComponent<Slider>();
        Slider iceSlider = WindSlider.GetComponent<Slider>();
        Slider soulSlider = SoulSlider.GetComponent<Slider>();
        if (context.performed)
        {
            if (ShootingDelayTimer <= 0)
            {
                if (CurrentElement == Element.Fire)
                {
                    if (fireSlider.value > 0)
                    {
                        GameObject Fire = Instantiate(FireProjectile, FiringPoint.position, Quaternion.identity);
                        if (!isFacingRight)
                        {
                            Fire.GetComponent<FireProj>().Flip();
                        }
                        elementalPowerBar.TakeFireEnergy(10);
                        ShootingDelayTimer = ShootingDelay;
                    }

                }
                if (CurrentElement == Element.Water)
                {
                    if (waterSlider.value > 0)
                    {
                        GameObject Water = Instantiate(WaterProjectile, FiringPoint.position, Quaternion.identity);
                        if (!isFacingRight)
                        {
                            Water.GetComponent<WaterProj>().Flip();
                        }
                        elementalPowerBar.TakeWaterEnergy(10);
                        ShootingDelayTimer = ShootingDelay;
                    }

                }
                if (CurrentElement == Element.Wind)
                {
                    if (iceSlider.value != 0)
                    {
                        GameObject Ice = Instantiate(IceProjectile, FiringPoint.position, Quaternion.identity);
                        if (!isFacingRight)
                        {
                            Ice.GetComponent<WindProj>().Flip();
                        }
                        elementalPowerBar.TakeIceEnergy(10);
                        ShootingDelayTimer = ShootingDelay;
                    }

                }
                if (CurrentElement == Element.Soul)
                {
                    if (soulSlider.value != 0)
                    {
                        if (trigger.IsInRangeToPosses)
                        {
                            elementalPowerBar.TakeSoulEnergy(100);
                            GameObject Soul = Instantiate(SoulProjectile, FiringPoint.position, Quaternion.identity);
                            {
                                if (!isFacingRight)
                                {
                                    Soul.GetComponent<SoulProj>().Flip();
                                }

                            }

                            trigger.Posses();
                            DisableElementalBars();
                            Debug.Log("Possesed Enemy!");
                        }
                        else
                        {
                            Debug.Log("No Enemy Found in Range to Posses.");
                        }
                    }
                }
                Debug.Log($"{CurrentElement}Projectile Fired ! ");
            }

        }

    }
  
    private void DisableElementalBars()
    {
        FireSlider.SetActive(false);
        WaterSlider.SetActive(false);
        WindSlider.SetActive(false);
        SoulSlider.SetActive(false);
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
        if (isFacingRight)
        {
            ThrownDirection = Vector3.right * 0.5f;
        }
        else
        {
            ThrownDirection = Vector3.left * 0.5f;
        }
    }
    private IEnumerator DodgeRoll()
    {
        Debug.Log("Player is invulnerable");
        isDodging = true;
        dodgeCooldownTimer = dodgeCooldown;
        isHittable = false;
        // Disable controls or animations if needed
        Anim.SetTrigger("Dodge");

        // Determine dodge direction
        Vector2 dodgeDirection = new Vector2(horizontal, 0).normalized;
        if (dodgeDirection == Vector2.zero) dodgeDirection = transform.right;

        rb.velocity = dodgeDirection * dodgeSpeed;

        // Wait for the dodge duration
        yield return new WaitForSeconds(dodgeDuration);
        isHittable = true;
        Debug.Log("Player is vulnerable");
        // Reset velocity after dodge
        rb.velocity = new Vector2(0, rb.velocity.y); // Keep the vertical velocity
        isDodging = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isBossFight) return;
        if (collision.gameObject.CompareTag("Ladder"))
        {
            IsLadder = true;
           
            Anim.SetBool("IsClimbing", true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isBossFight) return;
        if (collision.gameObject.CompareTag("Ladder"))
        {
            IsLadder = false;
           
            Anim.SetBool("IsClimbing", false);
            IsClimbingMoving = false;

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBossFight) return;
        if (collision.gameObject.CompareTag("Top"))
        {
           
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
            hasJumped = false;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isBossFight) return;
        if (collision.gameObject.CompareTag("Top"))
        {
           
        }
    }
    #endregion

}
