using UnityEngine;

public class LaserCollision : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Example: Destroy the laser and the object it hits
        if (collision.CompareTag("Asteroid"))
        {
            Destroy(collision.gameObject); // Destroy the asteroid
            Destroy(gameObject);          // Destroy the laser
        }
    }
}
