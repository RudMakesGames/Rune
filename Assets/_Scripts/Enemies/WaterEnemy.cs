using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class WaterEnemy : MonoBehaviour,IDamageable
{
    [SerializeField]
    private float MaxHealth = 100f;
    private float CurrentHealth;
    [SerializeField]
    private Slider HealthSlider;
    public TextMeshProUGUI DamageNumber;
    public float fadeDuration = 1f;
    private DamageFlash damageFlash;
    public Color32 startColor = new Color32(255, 255, 255, 255);
    [SerializeField]
    private PlayerDetection playerDetection;
    public bool HasbeenHit = false;
    private Rigidbody2D rb;
    [SerializeField]
    public float KnockbackForce;
    public void HandleDeath()
    {
        StartCoroutine(EnemyDeath());
    }

    public void TakeDamage(float damage)
    {
        HasbeenHit = true;
        DamageNumber.color = startColor;
        CurrentHealth -= damage;
        DamageNumber.text = damage.ToString();
        StartCoroutine(FadeOut());
        damageFlash.CallDamageFlash();
        if (CurrentHealth <=0)
        {
            HandleDeath();
        }
        if (playerDetection != null)
        {
            playerDetection.ChangeState();
            
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        CurrentHealth = MaxHealth;
        damageFlash = GetComponent<DamageFlash>();
        
    }

    // Update is called once per frame
    void Update()
    {
        HealthSlider.value = CurrentHealth;
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
    IEnumerator EnemyDeath()
    {
        //ADD Effects
        yield return new WaitForSeconds(0.6f);
        CurrencyManager.Instance.GainCurrency(1);
        Destroy(gameObject);
    }

    public void TakeStealthDamage(float damage)
    {
        HasbeenHit = true;
            DamageNumber.color = startColor;
            CurrentHealth -= damage;
            DamageNumber.text = damage.ToString();
            StartCoroutine(FadeOut());
            damageFlash.CallDamageFlash();
            Debug.Log($"Current Health: {CurrentHealth}");
            if (CurrentHealth <= 0f)
            {
                HandleDeath();
            }
        
    }

    public void AddKnockBack()
    {
        rb.AddForce(Vector2.up * KnockbackForce, ForceMode2D.Impulse);
        rb.AddForce(Vector2.left * KnockbackForce, ForceMode2D.Impulse);
    }
}
