using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float MaxHealth = 100f;
    private float CurrentHealth;
    [SerializeField]
    private Slider HealthSlider;
    private DamageFlash damageFlash;
    private Rigidbody2D rb;
    [SerializeField]
    public float KnockbackForce = 0.5f;
    [SerializeField]
    private float DamageAmount = 100;
    public bool HasBeenHit = false;
    [SerializeField]
    private Animator SceneFade;
    public void HandleDeath()
    {
        StartCoroutine(PlayerDeath());
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        ApplyKnockback();
        damageFlash.CallDamageFlash();
        if (CurrentHealth <= 0)
        {
            HandleDeath();
        }
        HasBeenHit = true;
    }
    public void ApplyKnockback()
    {
        rb.AddForce(Vector2.up * KnockbackForce, ForceMode2D.Impulse);
        rb.AddForce(Vector2.left * KnockbackForce, ForceMode2D.Impulse);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        CurrentHealth = MaxHealth;
        damageFlash = GetComponent<DamageFlash>();
    }
    IEnumerator PlayerDeath()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<Collider2D>().enabled = false;
        Debug.Log("U died!");
        SceneFade.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneFade.SetTrigger("Start");
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<Collider2D>().enabled = true;
        gameObject.transform.position = OutOfBounds.Instance.respawnPoint.transform.position;
        CurrentHealth = MaxHealth;
    }
    // Update is called once per frame
    void Update()
    {
        if (HealthSlider != null)
        {
            HealthSlider.value = CurrentHealth;
        }
    }
    public void SetBackToHitable()
    {
        HasBeenHit = false;
    }

    public void TakeStealthDamage(float damage)
    {
        throw new System.NotImplementedException();
    }

    public void AddKnockBack()
    {

    }
}

