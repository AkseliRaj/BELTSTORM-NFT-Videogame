using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Thrust Effect")]
    public GameObject engineThrustPrefab;   // Prefab with 4-frame thrust animation
    public Transform enginePoint;           // Empty child Transform at ship's engine location
    public ParticleSystem engineParticles;  // Particle system for engine dust

    [Header("Movement Settings")]
    public float thrustForce = 300f;    // Force applied per second
    public float rotationSpeed = 200f;
    public float maxSpeed = 10f;

    [Header("Dodge Settings")]
    public float dodgeForce = 20f;
    public float dodgeDuration = 0.3f;
    public float dodgeCooldown = 1f;

    [Header("Audio Settings")]
    public AudioClip boostSound;
    public Vector2 pitchRange = new Vector2(0.95f, 1.05f);

    private Rigidbody2D rb;
    private PlayerHealth playerHealth;
    private Vector2 screenBounds;
    private bool isDodging = false;
    private bool canDodge = true;
    private float thrustInput = 0f;

    private GameObject engineThrustInstance;

    void Start()
    {
        // Cache components
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealth>();
        if (rb == null) Debug.LogError("Rigidbody2D missing!");
        if (playerHealth == null) Debug.LogError("PlayerHealth missing!");

        // Instantiate the thrust animation prefab under the engine point
        if (engineThrustPrefab != null && enginePoint != null)
        {
            engineThrustInstance = Instantiate(
                engineThrustPrefab,
                enginePoint.position,
                enginePoint.rotation,
                enginePoint
            );
            engineThrustInstance.SetActive(false);
        }

        // Calculate screen bounds based on camera
        Camera cam = Camera.main;
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 topRight   = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));
        screenBounds = new Vector2(topRight.x, topRight.y);
    }

    void Update()
    {
        // Read thrust input
        thrustInput = (!isDodging && Input.GetKey(KeyCode.W)) ? 1f : 0f;

        // Toggle thrust animation prefab
        if (engineThrustInstance != null)
        {
            bool shouldBeActive = thrustInput > 0f;
            if (engineThrustInstance.activeSelf != shouldBeActive)
                engineThrustInstance.SetActive(shouldBeActive);
        }

        // Handle engine particle effects (dust)
        if (engineParticles != null)
        {
            if (thrustInput > 0f && !engineParticles.isPlaying)
                engineParticles.Play();
            else if (thrustInput == 0f && engineParticles.isPlaying)
                engineParticles.Stop();
        }

        // Handle rotation
        if (!isDodging)
        {
            float rot = Input.GetAxisRaw("Horizontal");
            transform.Rotate(0, 0, -rot * rotationSpeed * Time.deltaTime);
        }

        // Handle dodge input
        if (Input.GetMouseButtonDown(1) && canDodge)
            StartCoroutine(PerformDodge());
    }

    void FixedUpdate()
    {
        if (!isDodging && thrustInput > 0f)
        {
            // Apply thrust force
            rb.AddForce(transform.up * thrustForce * thrustInput * Time.fixedDeltaTime,
                        ForceMode2D.Force);
        }

        // Clamp maximum speed
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);

        // Keep the ship on-screen
        ClampPositionToScreen();
    }

    void PlayBoostSound()
    {
        if (boostSound != null && SFXManager.Instance != null)
        {
            float pitch = Random.Range(pitchRange.x, pitchRange.y);
            SFXManager.Instance.PlaySoundWithPitch(boostSound, transform.position, pitch);
        }
    }

    void ClampPositionToScreen()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -screenBounds.x, screenBounds.x);
        pos.y = Mathf.Clamp(pos.y, -screenBounds.y, screenBounds.y);
        transform.position = pos;
    }

    IEnumerator PerformDodge()
    {
        isDodging = true;
        canDodge = false;

        // Make player temporarily invincible
        if (playerHealth != null)
            playerHealth.SetInvincible(true);

        // Clear velocity and add dodge impulse
        rb.velocity = Vector2.zero;
        rb.AddForce(transform.up * dodgeForce, ForceMode2D.Impulse);
        PlayBoostSound();

        // Wait for dodge duration
        yield return new WaitForSeconds(dodgeDuration);

        isDodging = false;
        if (playerHealth != null)
            playerHealth.SetInvincible(false);

        // Wait for cooldown
        yield return new WaitForSeconds(dodgeCooldown);
        canDodge = true;
    }
}
