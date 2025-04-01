using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float screenBoundsBuffer = 1f; // Extra buffer to destroy asteroids off-screen

    void Update()
    {
        // Check if the asteroid is off-screen and destroy it
        if (IsOffScreen())
        {
            Destroy(gameObject); // Destroy the asteroid
        }
    }

    private bool IsOffScreen()
    {
        // Get the screen bounds
        Vector3 screenPosition = Camera.main.WorldToViewportPoint(transform.position);
        
        // Check if the object is outside the screen bounds, including the buffer
        return screenPosition.x < -screenBoundsBuffer || screenPosition.x > 1 + screenBoundsBuffer ||
               screenPosition.y < -screenBoundsBuffer || screenPosition.y > 1 + screenBoundsBuffer;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the asteroid was hit by a laser
        if (collision.CompareTag("Laser"))
        {
            Destroy(gameObject); // Destroy the asteroid
            Destroy(collision.gameObject); // Destroy the laser
            Debug.Log("Asteroid destroyed by laser!");
        }

        // Check if the asteroid hit the mothership
        if (collision.CompareTag("Mothership"))
        {
            Debug.Log("Mothership hit by asteroid!");
            Destroy(gameObject); // Destroy the asteroid
            // Optionally, reduce mothership health or trigger game over
        }
    }
}
