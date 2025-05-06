using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 1;
    private int currentHealth;

    [Header("Coin Drop (goldens only)")]
    public bool isGolden = false;
    public GameObject coinPrefab;

    [Header("Destroy Off‐Screen")]
    public float screenBoundsBuffer = 1f;

    [Header("Audio")]
    public AudioClip[] destructionSounds;
    public AudioClip hitSound;
    public float volume = 1f;
    public Vector2 destructionPitchRange = new Vector2(0.95f, 1.05f);
    public Vector2 hitPitchRange = new Vector2(0.95f, 1.05f);

    [Header("Smoke Effect")]
    public GameObject smokePrefab; // Shown on asteroid destruction

    [Header("Hit Effect")]
    public GameObject laserImpactPrefab; // Shown on asteroid hit (not destroyed)

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
            Vector3 impactPoint = collision.transform.position; // Use laser position as impact point
            SpawnLaserImpact(impactPoint); // Spawn the laser hit effect at impact point

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

                SpawnSmoke(); // Optional: show smoke when colliding with player
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

            PlayRandomDestructionSound();
            SpawnSmoke(); // Smoke when asteroid is destroyed
            Destroy(gameObject);
        }
        else
        {
            if (!isGolden && hitSound != null)
                PlayHitSound();
        }
    }

    private void PlayRandomDestructionSound()
    {
        if (SFXManager.Instance == null || destructionSounds == null || destructionSounds.Length == 0)
            return;

        AudioClip clip = destructionSounds[Random.Range(0, destructionSounds.Length)];
        float pitch = Random.Range(destructionPitchRange.x, destructionPitchRange.y);
        SFXManager.Instance.PlaySoundWithPitch(clip, transform.position, pitch);
    }

    private void PlayHitSound()
    {
        if (SFXManager.Instance == null || hitSound == null) return;

        float pitch = Random.Range(hitPitchRange.x, hitPitchRange.y);
        SFXManager.Instance.PlaySoundWithPitch(hitSound, transform.position, pitch);
    }

    private void SpawnSmoke()
    {
        if (smokePrefab != null)
        {
            Instantiate(smokePrefab, transform.position, Quaternion.identity);
        }
    }

    private void SpawnLaserImpact(Vector3 position)
    {
        if (laserImpactPrefab != null)
        {
            // Slight z-offset to render in front of asteroid (adjust as needed)
            Vector3 spawnPos = new Vector3(position.x, position.y, transform.position.z - 0.1f);
            GameObject impact = Instantiate(laserImpactPrefab, spawnPos, Quaternion.identity);

            // Optional: attach to asteroid to follow its movement (disable if you want effect to stay at hit point)
            impact.transform.SetParent(transform);
        }
    }
}
