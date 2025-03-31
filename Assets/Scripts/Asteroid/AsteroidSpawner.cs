using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab; // The asteroid prefab to spawn
    public float spawnRate = 1.5f;    // Time between spawns
    public float spawnRangeX = 8f;    // Horizontal range for spawning

    public float asteroidSpeed = 3f;  // Speed of asteroids

    void Start()
    {
        // Start spawning asteroids repeatedly
        InvokeRepeating("SpawnAsteroid", 1f, spawnRate);
    }

    void SpawnAsteroid()
    {
        // Randomize spawn position
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPosition = new Vector3(randomX, transform.position.y, 0f);

        // Instantiate the asteroid
        GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);

        // Set the asteroid's velocity
        Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.down * asteroidSpeed;
        }
    }
}
