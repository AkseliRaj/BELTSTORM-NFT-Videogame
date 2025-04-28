using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float thrustForce = 300f;    // Force applied per second
    public float rotationSpeed = 200f;
    public float maxSpeed = 10f;
    public ParticleSystem engineParticles;

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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealth>();

        if (rb == null) Debug.LogError("Rigidbody2D missing!");
        if (playerHealth == null) Debug.LogError("PlayerHealth missing!");

        // Ensure you set Rigidbody2D.Linear Drag (e.g., 1f) in the Inspector for friction

        Camera cam = Camera.main;
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));
        screenBounds = new Vector2(topRight.x, topRight.y);
    }

    void Update()
    {
        // Read thrust input in Update for responsiveness
        thrustInput = (!isDodging && Input.GetKey(KeyCode.W)) ? 1f : 0f;

        // Handle engine particle effects
        if (engineParticles != null)
        {
            if (thrustInput > 0f && !engineParticles.isPlaying)
                engineParticles.Play();
            else if (thrustInput == 0f && engineParticles.isPlaying)
                engineParticles.Stop();
        }

        // Rotation
        if (!isDodging)
        {
            float rot = Input.GetAxisRaw("Horizontal");
            transform.Rotate(0, 0, -rot * rotationSpeed * Time.deltaTime);
        }

        // Dodge
        if (Input.GetMouseButtonDown(1) && canDodge)
            StartCoroutine(PerformDodge());
    }

    void FixedUpdate()
    {
        if (!isDodging)
        {
            // Apply framerate-independent thrust
            rb.AddForce(transform.up * thrustForce * thrustInput * Time.fixedDeltaTime,
                        ForceMode2D.Force);

            // Clamp speed
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }

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

        if (playerHealth != null)
            playerHealth.SetInvincible(true);

        Vector2 dodgeDir = transform.up;

        rb.velocity = Vector2.zero;
        rb.AddForce(dodgeDir * dodgeForce, ForceMode2D.Impulse);
        PlayBoostSound();

        yield return new WaitForSeconds(dodgeDuration);

        isDodging = false;
        if (playerHealth != null)
            playerHealth.SetInvincible(false);

        yield return new WaitForSeconds(dodgeCooldown);
        canDodge = true;
    }
}
