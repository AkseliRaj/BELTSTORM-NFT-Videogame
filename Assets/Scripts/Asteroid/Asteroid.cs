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
    public AudioClip[] destructionSounds;    // 2 random sounds
    public AudioClip hitSound;               // plays on laser hit (non-golden)
    public float volume = 1f;
    public Vector2 destructionPitchRange = new Vector2(0.95f, 1.05f);
    public Vector2 hitPitchRange = new Vector2(0.95f, 1.05f);

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
            Destroy(gameObject); // No sound here!
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

            PlayRandomSound(destructionSounds, destructionPitchRange);
            Destroy(gameObject);
        }
        else if (!isGolden && hitSound != null)
        {
            PlaySingleSound(hitSound, hitPitchRange);
        }
    }

    private void PlayRandomSound(AudioClip[] clips, Vector2 pitchRange)
    {
        if (clips == null || clips.Length == 0) return;

        AudioClip clip = clips[Random.Range(0, clips.Length)];
        GameObject tempGO = new GameObject("AsteroidSound");
        tempGO.transform.position = transform.position;

        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = clip;
        aSource.volume = volume;
        aSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
        aSource.Play();

        Destroy(tempGO, clip.length / aSource.pitch);
    }

    private void PlaySingleSound(AudioClip clip, Vector2 pitchRange)
    {
        GameObject tempGO = new GameObject("AsteroidHitSound");
        tempGO.transform.position = transform.position;

        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = clip;
        aSource.volume = volume;
        aSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
        aSource.Play();

        Destroy(tempGO, clip.length / aSource.pitch);
    }
}
