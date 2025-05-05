using UnityEngine;
using UnityEngine.UI;

public class StarSpawner : MonoBehaviour
{
    [Header("Star Types (define your 4 sizes here)")]
    public StarType[] starTypes;

    [Header("Spawn Area & Prefab")]
    public RectTransform spawnArea;
    public GameObject starPrefab;

    [Header("Spawn Timing")]
    public float minSpawnInterval = 0.2f;
    public float maxSpawnInterval = 1f;

    [Header("Rotation Randomization")]
    [Tooltip("Minimum rotation speed (degrees/second). Can be negative.")]
    public float minRotationSpeed = -20f;
    [Tooltip("Maximum rotation speed (degrees/second).")]
    public float maxRotationSpeed = 20f;

    void Start()
    {
        if (starTypes == null || starTypes.Length == 0 || spawnArea == null || starPrefab == null)
        {
            Debug.LogError("Assign starTypes, spawnArea, and starPrefab!");
            enabled = false;
            return;
        }
        StartCoroutine(SpawnLoop());
    }

    private System.Collections.IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));
            SpawnOne();
        }
    }

    private void SpawnOne()
    {
        GameObject starGO = Instantiate(starPrefab, spawnArea, false);

        Rect area = spawnArea.rect;
        float x = Random.Range(area.xMin, area.xMax);
        float y = Random.Range(area.yMin, area.yMax);
        starGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);

        StarType def = GetRandomStarType();

        // Assign sprite
        Image img = starGO.GetComponent<Image>();
        SpriteRenderer sr = starGO.GetComponent<SpriteRenderer>();
        if (img != null) img.sprite = def.sprite;
        else if (sr != null) sr.sprite = def.sprite;

        // Set controller properties
        StarController ctrl = starGO.GetComponent<StarController>();
        if (ctrl != null)
        {
            ctrl.fadeCycleTime      = def.fadeCycleTime;
            ctrl.scaleCycleTime     = def.scaleCycleTime;
            ctrl.maxScaleMultiplier = def.maxScaleMultiplier;
            ctrl.rotationSpeed      = Random.Range(minRotationSpeed, maxRotationSpeed);

            float life = Mathf.Max(def.fadeCycleTime, def.scaleCycleTime);
            Destroy(starGO, life + 0.1f);
        }
        else
        {
            Destroy(starGO, 2f);
        }
    }

    private StarType GetRandomStarType()
    {
        float totalWeight = 0f;
        foreach (var t in starTypes)
            totalWeight += t.spawnWeight;

        float r = Random.Range(0f, totalWeight);
        float accum = 0f;

        foreach (var t in starTypes)
        {
            accum += t.spawnWeight;
            if (r <= accum)
                return t;
        }

        return starTypes[starTypes.Length - 1]; // fallback
    }
}
