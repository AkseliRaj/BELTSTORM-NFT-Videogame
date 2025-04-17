using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 1;        // Set to 3 on your normal prefab
    private int currentHealth;

    [Header("Coin Drop (goldens only)")]
    public bool isGolden = false;    // Tick this on your golden prefab
    public GameObject coinPrefab;    

    [Header("Destroy Off‐Screen")]
    public float screenBoundsBuffer = 1f;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (IsOffScreen())
            Destroy(gameObject);
    }

    private bool IsOffScreen()
    {
        Vector3 vp = Camera.main.WorldToViewportPoint(transform.position);
        return vp.x < -screenBoundsBuffer || vp.x > 1 + screenBoundsBuffer
            || vp.y < -screenBoundsBuffer || vp.y > 1 + screenBoundsBuffer;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser"))
        {
            Destroy(collision.gameObject);
            TakeDamage(1);
        }
        else if (collision.CompareTag("Player"))
        {
            var playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null && !playerHealth.IsInvincible())
            {
                playerHealth.TakeDamage(1);
                var shake = Camera.main.GetComponent<Shake>();
                if (shake != null) shake.start = true;
                Destroy(gameObject);
            }
        }
    }

    private void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            if (isGolden && coinPrefab != null)
                Instantiate(coinPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
