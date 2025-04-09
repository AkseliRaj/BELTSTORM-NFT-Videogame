using UnityEngine;
using System.Collections;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab;     // The asteroid prefab to spawn
    public Transform playerCharacter;     // Reference to the PlayerCharacter
    public float spawnRate = 1.5f;          // Base time (in seconds) between spawns
    public float asteroidSpeed = 3f;        // Base speed of asteroids
    public float spawnRadius = 10f;         // Distance from the center to spawn asteroids
    public Timer timer;                   // Reference to the Timer script

    // Base values for adjustments:
    private float baseSpawnRate;
    private float baseAsteroidSpeed;

    void Start()
    {
        // Save the original spawn rate and asteroid speed for calculations
        baseSpawnRate = spawnRate;
        baseAsteroidSpeed = asteroidSpeed;

        // If not assigned in the inspector, try to find the Timer
        if (timer == null)
        {
            timer = FindObjectOfType<Timer>();
        }
        
        StartCoroutine(SpawnAsteroidsRoutine());
    }

    IEnumerator SpawnAsteroidsRoutine()
    {
        while (playerCharacter != null)
        {
            SpawnAsteroid();

            // Calculate difficulty multiplier based on elapsed time.
            // For example, every 120 seconds, the game increases difficulty.
            float elapsed = timer != null ? timer.ElapsedTime : 0f;

            // Adjust spawn interval: The wait time decreases over time.
            // Clamped so it never becomes faster than 50% of the base spawn rate.
            float spawnRateMultiplier = Mathf.Clamp(1f - (elapsed / 240f), 0.5f, 1f);
            float adjustedSpawnRate = baseSpawnRate * spawnRateMultiplier;

            // Adjust asteroid speed: Increase speed gradually.
            // For example, add 0.5 units per minute.
            float speedIncrease = (elapsed / 60f) * 0.5f;
            asteroidSpeed = baseAsteroidSpeed + speedIncrease;

            yield return new WaitForSeconds(adjustedSpawnRate);
        }
    }

    void SpawnAsteroid()
    {
        // Ensure the player still exists.
        if (playerCharacter == null)
        {
            return;
        }

        // Calculate a random spawn position on a circle around the player.
        Vector2 spawnPosition = (Vector2)playerCharacter.position + Random.insideUnitCircle.normalized * spawnRadius;

        // Instantiate the asteroid.
        GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);

        // Determine the direction toward the player.
        Vector2 directionToPlayer = ((Vector2)playerCharacter.position - (Vector2)asteroid.transform.position).normalized;

        // Set the asteroid's velocity.
        Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = directionToPlayer * asteroidSpeed;
        }
    }
}
