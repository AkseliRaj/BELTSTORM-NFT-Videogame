using UnityEngine;
using System.Collections;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("Asteroid Prefabs")]
    public GameObject normalAsteroidPrefab;
    public GameObject goldenAsteroidPrefab;

    [Header("Spawn Settings")]
    public Transform playerCharacter;
    public float spawnRate = 1.5f;
    public float asteroidSpeed = 3f;
    public float spawnRadius = 10f;
    [Range(0f,1f)]
    public float goldenChance = 0.05f; // 5% of asteroids are golden

    public Timer timer;

    private float baseSpawnRate;
    private float baseAsteroidSpeed;

    void Start()
    {
        baseSpawnRate = spawnRate;
        baseAsteroidSpeed = asteroidSpeed;
        if (timer == null) timer = FindObjectOfType<Timer>();
        StartCoroutine(SpawnAsteroidsRoutine());
    }

   IEnumerator SpawnAsteroidsRoutine()
    {
        while (playerCharacter != null)
        {
            // Compute in advance
            Vector2 spawnPos = (Vector2)playerCharacter.position 
                               + Random.insideUnitCircle.normalized * spawnRadius;
            GameObject prefab = (Random.value < goldenChance)
                ? goldenAsteroidPrefab
                : normalAsteroidPrefab;

            // Show indicator
            IndicatorManager.Instance?.ShowIndicator(spawnPos);

            // Wait a bit before actually spawning
            yield return new WaitForSeconds(0.5f);

            // Spawn it
            GameObject asteroid = Instantiate(prefab, spawnPos, Quaternion.identity);
            Vector2 dir = ((Vector2)playerCharacter.position - spawnPos).normalized;
            asteroid.GetComponent<Rigidbody2D>().velocity = dir * asteroidSpeed;

            // Then wait your spawnRate
            float elapsed = timer?.ElapsedTime ?? 0f;
            float spawnRateMultiplier = Mathf.Clamp(1f - (elapsed / 240f), 0.5f, 1f);
            float adjusted = baseSpawnRate * spawnRateMultiplier;
            float speedInc = (elapsed / 60f) * 0.5f;
            asteroidSpeed = baseAsteroidSpeed + speedInc;
            yield return new WaitForSeconds(adjusted);
        }
    }

        void SpawnAsteroid()
    {
        if (playerCharacter == null) return;

        // Determine spawn position
        Vector2 spawnPos = (Vector2)playerCharacter.position 
                           + Random.insideUnitCircle.normalized * spawnRadius;

        // Show the indicator:
        if (IndicatorManager.Instance != null)
            IndicatorManager.Instance.ShowIndicator(spawnPos);

        // THEN spawn the asteroid as before:
        GameObject prefabToSpawn = (Random.value < goldenChance)
            ? goldenAsteroidPrefab
            : normalAsteroidPrefab;

        GameObject asteroid = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        Vector2 direction = ((Vector2)playerCharacter.position - spawnPos).normalized;
        Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = direction * asteroidSpeed;
    }

}
