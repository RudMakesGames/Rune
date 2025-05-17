using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IDamageable 
{
  public void TakeDamage(float damage);
    public void TakeStealthDamage(float damage);
    public void AddKnockBack();
    public void HandleDeath();
}
