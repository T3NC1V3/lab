using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public int CurrentHealth
    { 
        get 
        { 
            return currentHealth;
        }
      //  set
      //  {
      //      currentHealth = value;
      //  }
    
    }

    public int damage;
    public int armor;

    public event System.Action<int, int> OnHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
        StartCoroutine(HealthIncrease());
        // StopCoroutine(HealthIncrease());
    }

    public virtual void TakeDamage(int damage)
    {
        damage -= armor;

        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentHealth -= damage;

        if (OnHealthChanged != null)
        {
            OnHealthChanged(maxHealth, currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void RestoreHealth(int restore)
    {
        // currentHealth += restore;
        // Anti overheal
        currentHealth = Mathf.Clamp(currentHealth + restore, 0, int.MaxValue);

        if (OnHealthChanged != null)
        {
            OnHealthChanged(maxHealth, currentHealth);
        }
    }

    IEnumerator HealthIncrease()
    {
        Debug.Log("Start Coroutine");

        for(int x = 1; x <=maxHealth; x++) 
        {
            currentHealth = x;
            if (OnHealthChanged != null)
            {
                OnHealthChanged(maxHealth, currentHealth);
            }

            yield return new WaitForSeconds(0.01f);
        }

        
    }

    public virtual void Die()
    {
        Debug.Log(transform.name + " died!");
    }
}