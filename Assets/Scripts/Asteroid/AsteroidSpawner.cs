using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab; // The asteroid prefab to spawn
    public Transform playerCharacter; // Reference to the PlayerCharacter
    public float spawnRadius = 10f;   // Distance from the center to spawn asteroids
    public float spawnRate = 1.5f;    // Time between spawns
    public float asteroidSpeed = 3f;  // Speed of asteroids

    void Start()
    {
        // Start spawning asteroids repeatedly
        InvokeRepeating("SpawnAsteroid", 1f, spawnRate);
    }

    void SpawnAsteroid()
    {
        // Check if playerCharacter is still valid
        if (playerCharacter == null)
        {
            // If the player has been destroyed, stop spawning asteroids
            CancelInvoke("SpawnAsteroid");
            return;
        }

        // Randomize a spawn position off-screen
        Vector2 spawnPosition = (Vector2)playerCharacter.position + Random.insideUnitCircle.normalized * spawnRadius;

        // Instantiate the asteroid
        GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);

        // Calculate direction toward the playerCharacter
        Vector2 directionToPlayerCharacter = ((Vector2)playerCharacter.position - (Vector2)asteroid.transform.position).normalized;

        // Set the asteroid's velocity
        Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = directionToPlayerCharacter * asteroidSpeed;
        }
    }
}
