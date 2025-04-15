using UnityEngine;
using System.Collections;

public class MainMenuAsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public float spawnInterval = 1.5f;
    public float asteroidSpeed = 3f;
    public float spawnDistance = 10f;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnAsteroidsRoutine());
    }

    IEnumerator SpawnAsteroidsRoutine()
    {
        while (true)
        {
            SpawnAsteroid();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnAsteroid()
    {
        Vector2 spawnPosition = GetRandomEdgePosition();
        Vector2 direction = GetRandomDirectionThroughScreen(spawnPosition);

        GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = direction * asteroidSpeed;
        }
    }

    Vector2 GetRandomEdgePosition()
    {
        // Get screen boundaries in world coordinates
        float camHeight = 2f * mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        // Choose a random edge: 0 = left, 1 = right, 2 = top, 3 = bottom
        int edge = Random.Range(0, 4);
        Vector2 position = Vector2.zero;

        switch (edge)
        {
            case 0: // Left
                position = new Vector2(-camWidth / 2 - spawnDistance, Random.Range(-camHeight / 2, camHeight / 2));
                break;
            case 1: // Right
                position = new Vector2(camWidth / 2 + spawnDistance, Random.Range(-camHeight / 2, camHeight / 2));
                break;
            case 2: // Top
                position = new Vector2(Random.Range(-camWidth / 2, camWidth / 2), camHeight / 2 + spawnDistance);
                break;
            case 3: // Bottom
                position = new Vector2(Random.Range(-camWidth / 2, camWidth / 2), -camHeight / 2 - spawnDistance);
                break;
        }

        return mainCamera.transform.position + (Vector3)position;
    }

    Vector2 GetRandomDirectionThroughScreen(Vector2 fromPosition)
    {
        // Aim at a random point slightly inside the screen to get more natural movement
        Vector2 screenCenter = (Vector2)mainCamera.transform.position;
        Vector2 randomOffset = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
        Vector2 targetPoint = screenCenter + randomOffset;

        return (targetPoint - fromPosition).normalized;
    }
}
