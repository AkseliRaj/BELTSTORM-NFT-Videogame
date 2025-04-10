using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float thrustForce = 5f;
    public float rotationSpeed = 200f;
    public float maxSpeed = 10f;
    public float friction = 0.98f;
    public ParticleSystem engineParticles;

    [Header("Dodge Settings")]
    public float dodgeForce = 20f;
    public float dodgeDuration = 0.3f;
    public float dodgeCooldown = 1f;

    private bool isDodging = false;
    private bool canDodge = true;
    private Rigidbody2D rb;
    private Vector2 screenBounds;
    private PlayerHealth playerHealth; // To set invincibility

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealth>();

        if (rb == null) Debug.LogError("Rigidbody2D missing!");
        if (playerHealth == null) Debug.LogError("PlayerHealth missing!");

        Camera cam = Camera.main;
        Vector3 screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));
        screenBounds = new Vector2(screenTopRight.x, screenTopRight.y);
    }

    void Update()
    {
        if (!isDodging)
        {
            float rotationInput = Input.GetAxisRaw("Horizontal");
            transform.Rotate(0, 0, -rotationInput * rotationSpeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.W))
            {
                ApplyThrust(thrustForce);
                if (engineParticles != null && !engineParticles.isPlaying)
                    engineParticles.Play();
            }
            else if (engineParticles != null && engineParticles.isPlaying)
            {
                engineParticles.Stop();
            }
        }

        if (Input.GetMouseButtonDown(1) && canDodge)
        {
            StartCoroutine(PerformDodge());
        }
    }

    void FixedUpdate()
    {
        if (!isDodging)
        {
            rb.velocity *= friction;
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }

        ClampPositionToScreen();
    }

    void ApplyThrust(float force)
    {
        Vector2 thrustDirection = transform.up;
        rb.AddForce(thrustDirection * force);
    }

    void ClampPositionToScreen()
    {
        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, -screenBounds.x, screenBounds.x);
        position.y = Mathf.Clamp(position.y, -screenBounds.y, screenBounds.y);
        transform.position = position;
    }

    System.Collections.IEnumerator PerformDodge()
    {
        isDodging = true;
        canDodge = false;

        if (playerHealth != null) playerHealth.SetInvincible(true);

        Vector2 dodgeDirection = transform.up; // Dodge in the ship's facing direction

        rb.velocity = Vector2.zero;
        rb.AddForce(dodgeDirection * dodgeForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(dodgeDuration);

        isDodging = false;
        if (playerHealth != null) playerHealth.SetInvincible(false);

        yield return new WaitForSeconds(dodgeCooldown);
        canDodge = true;
    }

}
