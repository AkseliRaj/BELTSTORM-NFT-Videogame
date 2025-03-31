using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player ship
    private Vector2 movement;   // Store the movement input

    void Update()
    {
        // Get input from WASD keys
        movement.x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        movement.y = Input.GetAxisRaw("Vertical");   // W/S or Up/Down

        // Rotate the ship to point in the direction of movement
        if (movement != Vector2.zero) // Avoid rotating when no input is given
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg; // Get angle in degrees
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); // Rotate to point forward
        }
    }

    void FixedUpdate()
    {
        // Normalize the movement vector to fix diagonal speed issue
        Vector2 moveDirection = movement.normalized; // Normalize to make length = 1

        // Move the player ship
        transform.Translate(moveDirection * moveSpeed * Time.fixedDeltaTime, Space.World);

        // Clamp position within the screen bounds
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -8f, 8f); // Adjust bounds
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -4f, 4f); // Adjust bounds
        transform.position = clampedPosition;
    }
}
