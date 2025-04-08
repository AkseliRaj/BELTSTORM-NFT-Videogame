using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;

    // Event to notify when health changes
    public event Action<int, int> OnHealthChanged;

    private Timer gameTimer; // Reference to the Timer script

    void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        // Find the Timer component in the scene
        gameTimer = FindObjectOfType<Timer>();

        // Optional: Add a check in case the Timer component is not found
        if (gameTimer == null)
        {
            Debug.LogError("Timer component not found in the scene!");
        }
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

        // Stop the timer if the Timer component was found
        if (gameTimer != null)
        {
            gameTimer.StopTimer();
        }

        Destroy(gameObject);
    }
}