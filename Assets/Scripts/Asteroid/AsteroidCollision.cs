using UnityEngine;

public class AsteroidCollision : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        // If the asteroid hits the mothership or a laser, destroy it
        if (collision.CompareTag("Mothership") || collision.CompareTag("Laser"))
        {
            Destroy(gameObject); // Destroy the asteroid

            // Optionally, destroy the laser or apply damage to the mothership
            if (collision.CompareTag("Laser"))
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
