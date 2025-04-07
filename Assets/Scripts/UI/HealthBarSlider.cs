using UnityEngine;
using UnityEngine.UI;

public class HealthBarSlider : MonoBehaviour
{
    public Slider healthSlider;           // Reference to the Slider component
    public PlayerHealth playerHealth;     // Reference to the PlayerHealth script

    void Start()
    {
        // Set up slider boundaries based on player health
        healthSlider.minValue = 0;
        healthSlider.maxValue = playerHealth.maxHealth;
        healthSlider.value = playerHealth.currentHealth;

        // Subscribe to the health changed event
        playerHealth.OnHealthChanged += UpdateHealthBar;
    }

    void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        healthSlider.value = currentHealth;
    }

    void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthBar;
        }
    }
}
