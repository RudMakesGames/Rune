using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour,IDamageable
{
    [SerializeField] public float MaxHealth = 100f;
    [SerializeField,HideInInspector]
    public float CurrentHealth;
    [SerializeField] private Slider HealthSlider;
    public TextMeshProUGUI DamageNumber;
    public float fadeDuration = 1f;
    private DamageFlash damageFlash;
    public Color32 startColor = new Color32(255, 255, 255, 255);
    public bool HasBeenHit = false;
    private Rigidbody2D rb;
    [SerializeField] private float KnockbackForce = 0.5f;
    private Animator anim;


    [SerializeField] private float dodgeSpeed = 15f;
    [SerializeField] private float dodgeDuration = 0.5f;
    [SerializeField] private float dodgeCooldown = 1f;
    private bool isDodging = false;
    private float dodgeCooldownTimer = 0f;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        CurrentHealth = MaxHealth;
        damageFlash = GetComponent<DamageFlash>();
        StartCoroutine(DodgeRollRoutine());
    }

    void Update()
    {
        HealthSlider.value = CurrentHealth;

        if (dodgeCooldownTimer > 0)
        {
            dodgeCooldownTimer -= Time.deltaTime;
        }
    }

    private IEnumerator DodgeRollRoutine()
    {
        while (true) // Keep looping to check for dodging
        {
            if (!isDodging && dodgeCooldownTimer <= 0f)
            {
                if(CurrentHealth <= MaxHealth/2)
                {
                    float randomDelay = Random.Range(1f, 2.5f);
                    yield return new WaitForSeconds(randomDelay);
                    StartCoroutine(DodgeRoll());
                }
                else
                {
                    float randomDelay = Random.Range(2f, 4f);
                    yield return new WaitForSeconds(randomDelay);
                    StartCoroutine(DodgeRoll());
                }
                
            }
            yield return null; // Wait for the next frame
        }
    }

    private IEnumerator DodgeRoll()
    {
        Debug.Log("Boss is dodging!");
        isDodging = true;
        dodgeCooldownTimer = dodgeCooldown;

        // Determine dodge direction (randomly to the left or right)
        Vector2 dodgeDirection = Random.value > 0.5f ? Vector2.right : Vector2.left;

        rb.velocity = dodgeDirection * dodgeSpeed;

        anim.SetTrigger("Dodge"); // Trigger dodge animation

        // Wait for the dodge duration
        yield return new WaitForSeconds(dodgeDuration);

        // Reset the velocity
        rb.velocity = new Vector2(0, rb.velocity.y);
        isDodging = false;
    }

    public void HandleDeath()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        HasBeenHit = true;
        anim.SetTrigger("Hurt");
        DamageNumber.color = startColor;
        CurrentHealth -= damage;
        DamageNumber.text = damage.ToString();
        AddKnockBack();
        StartCoroutine(FadeOut());
        damageFlash.CallDamageFlash();

        if (CurrentHealth <= 0)
        {
            HandleDeath();
        }
    }

    private IEnumerator FadeOut()
    {
        float startAlpha = 1f;
        float endAlpha = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            Color color = DamageNumber.color;
            color.a = alpha;
            DamageNumber.color = color;
            yield return null;
        }

        // Ensure the final alpha is set to 0
        Color finalColor = DamageNumber.color;
        finalColor.a = endAlpha;
        DamageNumber.color = finalColor;
    }

    public void TakeStealthDamage(float damage)
    {
       
    }

    public void AddKnockBack()
    {
        rb.AddForce(Vector2.right * KnockbackForce, ForceMode2D.Impulse);
    }
}
