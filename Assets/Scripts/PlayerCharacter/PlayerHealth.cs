using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;

    // Event to notify when health changes
    public event Action<int, int> OnHealthChanged;

    void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        Debug.Log("Player hit! Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player has died.");
        // Ensure UI is updated before destroying the player
        OnHealthChanged?.Invoke(0, maxHealth);
        Destroy(gameObject);
    }
}
