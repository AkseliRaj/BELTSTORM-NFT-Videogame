using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab; // The asteroid prefab to spawn
    public Transform mothership;     // Reference to the mothership (in the center)
    public float spawnRadius = 10f;  // Distance from the center to spawn asteroids
    public float spawnRate = 1.5f;   // Time between spawns
    public float asteroidSpeed = 3f; // Speed of asteroids

    void Start()
    {
        // Start spawning asteroids repeatedly
        InvokeRepeating("SpawnAsteroid", 1f, spawnRate);
    }

    void SpawnAsteroid()
    {
        // Randomize a spawn position off-screen
        Vector2 spawnPosition = (Vector2)mothership.position + Random.insideUnitCircle.normalized * spawnRadius;

        // Instantiate the asteroid
        GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);

        // Calculate direction toward the mothership
        Vector2 directionToMothership = (mothership.position - asteroid.transform.position).normalized;

        // Set the asteroid's velocity
        Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = directionToMothership * asteroidSpeed;
        }
    }
}
