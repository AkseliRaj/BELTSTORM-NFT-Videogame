using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float thrustForce = 5f;    // Forward/backward thrust strength
    public float rotationSpeed = 200f; // Rotation speed of the ship
    public float maxSpeed = 10f;      // Maximum speed limit
    public float friction = 0.98f;    // Friction factor (closer to 1 = more slippery)
    public ParticleSystem engineParticles;


    private Rigidbody2D rb;          // Rigidbody2D for physics-based movement
    private Vector2 screenBounds;    // Calculated screen boundaries

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component missing from the Player object!");
        }

        // Calculate screen boundaries based on the camera's view
        Camera cam = Camera.main;
        Vector3 screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

        // Store the horizontal and vertical limits
        screenBounds = new Vector2(screenTopRight.x, screenTopRight.y);
    }

    void Update()
{
    // Rotate the ship using A (left) and D (right)
    float rotationInput = Input.GetAxisRaw("Horizontal");
    transform.Rotate(0, 0, -rotationInput * rotationSpeed * Time.deltaTime);

    // Apply forward (W) or backward (S) thrust and handle particle effects
    if (Input.GetKey(KeyCode.W))
    {
        ApplyThrust(thrustForce);
        if (engineParticles != null && !engineParticles.isPlaying)
            engineParticles.Play();
    }
    else if (Input.GetKey(KeyCode.S))
    {
        ApplyThrust(-thrustForce);
        if (engineParticles != null && !engineParticles.isPlaying)
            engineParticles.Play();
    }
    else
    {
        // Optionally, stop the particle effect when not thrusting
        if (engineParticles != null && engineParticles.isPlaying)
            engineParticles.Stop();
    }
}


    void FixedUpdate()
    {
        // Apply friction-like effect
        rb.velocity *= friction;

        // Limit the velocity to the max speed
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);

        // Clamp the position to stay within screen bounds
        ClampPositionToScreen();
    }

    void ApplyThrust(float force)
    {
        // Calculate the thrust force in the ship's forward direction
        Vector2 thrustDirection = transform.up; // "Up" in Unity is the forward direction of the sprite
        rb.AddForce(thrustDirection * force);
    }

    void ClampPositionToScreen()
    {
        // Get the current position
        Vector3 position = transform.position;

        // Clamp the position within the calculated screen bounds
        position.x = Mathf.Clamp(position.x, -screenBounds.x, screenBounds.x);
        position.y = Mathf.Clamp(position.y, -screenBounds.y, screenBounds.y);

        // Apply the clamped position back to the transform
        transform.position = position;
    }
}
