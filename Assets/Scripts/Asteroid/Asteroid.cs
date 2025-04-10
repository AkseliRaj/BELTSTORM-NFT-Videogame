using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float screenBoundsBuffer = 1f; // Extra buffer to destroy asteroids off-screen

    void Update()
    {
        // Check if the asteroid is off-screen and destroy it
        if (IsOffScreen())
        {
            Destroy(gameObject);
        }
    }

    private bool IsOffScreen()
    {
        Vector3 screenPosition = Camera.main.WorldToViewportPoint(transform.position);
        return screenPosition.x < -screenBoundsBuffer || screenPosition.x > 1 + screenBoundsBuffer ||
               screenPosition.y < -screenBoundsBuffer || screenPosition.y > 1 + screenBoundsBuffer;
    }

    void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Laser"))
    {
        Destroy(gameObject);
        Destroy(collision.gameObject);
        Debug.Log("Asteroid destroyed by laser!");
    }
    else if (collision.CompareTag("Player"))
    {
        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

        // Only damage and shake if player is not invincible
        if (playerHealth != null && !playerHealth.IsInvincible())
        {
            Debug.Log("Player hit by asteroid!");

            playerHealth.TakeDamage(1);

            Shake cameraShake = Camera.main.GetComponent<Shake>();
            if (cameraShake != null)
            {
                cameraShake.start = true;
            }

            Destroy(gameObject);
        }
    }
}

}