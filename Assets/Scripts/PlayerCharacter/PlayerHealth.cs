using UnityEngine;
using System;
public class PlayerHealth : MonoBehaviour

{
    public int maxHealth = 5;
    public int currentHealth;
    public event Action<int, int> OnHealthChanged;

    private Timer gameTimer;
    private bool isInvincible = false;

    [Header("Audio")]
    public AudioClip damageSound;
    public AudioSource audioSource;
    public Vector2 pitchRange = new Vector2(0.95f, 1.05f);


    void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        gameTimer = FindObjectOfType<Timer>();
        if (gameTimer == null) Debug.LogError("Timer not found!");
    }

    public void TakeDamage(int damageAmount)
    {
        if (isInvincible) return;

        currentHealth -= damageAmount;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        Debug.Log("Player hit! Health: " + currentHealth);

        // Play damage sound
        if (damageSound != null && audioSource != null)
        {
            audioSource.pitch = UnityEngine.Random.Range(pitchRange.x, pitchRange.y);
            audioSource.PlayOneShot(damageSound);
        }

        if (currentHealth <= 0) Die();
    }


    public void SetInvincible(bool value)
    {
        isInvincible = value;
        Debug.Log("Invincibility: " + value);
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }

    void Die()
    {
        Debug.Log("Player has died.");
        OnHealthChanged?.Invoke(0, maxHealth);
        if (gameTimer != null) gameTimer.StopTimer();
        Destroy(gameObject);
    }
}
