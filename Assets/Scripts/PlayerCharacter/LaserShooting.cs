using UnityEngine;

public class LaserShooting : MonoBehaviour
{
    public GameObject laserPrefab;
    public Transform firePoint;
    public float laserSpeed = 10f;
    public float fireRate = 0.2f;

    [Header("Laser Sound")]
    public AudioClip laserSound;
    public float laserVolume = 0.8f;
    public float pitchMin = 0.95f;
    public float pitchMax = 1.05f;

    private float nextFireTime = 0f;

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            ShootLaser();
            nextFireTime = Time.time + fireRate;
        }
    }

    void ShootLaser()
    {
        GameObject laser = Instantiate(laserPrefab, firePoint.position, firePoint.rotation);

        Rigidbody2D rb = laser.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = firePoint.up * laserSpeed;
        }

        // Play sound with random pitch
        if (laserSound != null)
        {
            GameObject tempGO = new GameObject("LaserSound");
            tempGO.transform.position = firePoint.position;

            AudioSource aSource = tempGO.AddComponent<AudioSource>();
            aSource.clip = laserSound;
            aSource.volume = laserVolume;
            aSource.pitch = Random.Range(pitchMin, pitchMax);
            aSource.Play();

            Destroy(tempGO, laserSound.length / aSource.pitch);
        }
    }
}
