using UnityEngine;
using UnityEngine.UI;  // only if your panel is UI-based

public class StarSpawner : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Panel (with RectTransform) inside Canvas.")]
    public RectTransform spawnArea;
    [Tooltip("Prefab with an Image or SpriteRenderer + StarController.")]
    public GameObject starPrefab;

    [Header("Spawn Timing")]
    public float minSpawnInterval = 0.2f;
    public float maxSpawnInterval = 1f;

    void Start()
    {
        if (spawnArea == null || starPrefab == null)
        {
            Debug.LogError("Assign spawnArea and starPrefab in the Inspector!");
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
        // Instantiate as child of spawnArea (so it’s in UI space)
        GameObject star = Instantiate(starPrefab, spawnArea);
        star.transform.SetParent(spawnArea, false);

        // Random position inside the panel
        Rect r = spawnArea.rect;
        float x = Random.Range(r.xMin, r.xMax);
        float y = Random.Range(r.yMin, r.yMax);
        star.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);

        // Optionally destroy after longest cycle so it cleans up
        StarController ctrl = star.GetComponent<StarController>();
        if (ctrl != null)
        {
            float life = Mathf.Max(ctrl.fadeCycleTime, ctrl.scaleCycleTime);
            Destroy(star, life + 0.1f);
        }
        else
        {
            Destroy(star, 2f);
        }
    }
}
