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
            SpawnAsteroid();
            float elapsed = timer != null ? timer.ElapsedTime : 0f;
            float spawnRateMultiplier = Mathf.Clamp(1f - (elapsed / 240f), 0.5f, 1f);
            float adjustedSpawnRate = baseSpawnRate * spawnRateMultiplier;
            float speedIncrease = (elapsed / 60f) * 0.5f;
            asteroidSpeed = baseAsteroidSpeed + speedIncrease;
            yield return new WaitForSeconds(adjustedSpawnRate);
        }
    }

    void SpawnAsteroid()
    {
        if (playerCharacter == null) return;

        // Decide which prefab to use
        GameObject prefabToSpawn = (Random.value < goldenChance)
            ? goldenAsteroidPrefab
            : normalAsteroidPrefab;

        Vector2 spawnPos = (Vector2)playerCharacter.position + Random.insideUnitCircle.normalized * spawnRadius;
        GameObject asteroid = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

        Vector2 direction = ((Vector2)playerCharacter.position - spawnPos).normalized;
        Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = direction * asteroidSpeed;
    }
}
