using UnityEngine;

public class LaserShooting : MonoBehaviour
{
    public GameObject laserPrefab; // Drag the laser prefab here
    public Transform firePoint;   // Position from where the laser is fired
    public float laserSpeed = 10f; // Speed of the laser
    public float fireRate = 0.2f; // Time between shots

    private float nextFireTime = 0f; // Tracks when the player can fire next

    void Update()
    {
        // Check for shooting input (Left Mouse Button)
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            ShootLaser();
            nextFireTime = Time.time + fireRate; // Set the next allowed fire time
        }
    }

    void ShootLaser()
    {
        // Instantiate a laser at the firePoint position and rotation
        GameObject laser = Instantiate(laserPrefab, firePoint.position, firePoint.rotation);

        // Set the laser's velocity
        Rigidbody2D rb = laser.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = firePoint.up * laserSpeed; // Use firePoint's "up" direction
        }
    }
}
