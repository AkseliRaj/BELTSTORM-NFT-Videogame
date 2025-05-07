using UnityEngine;
using UnityEngine.UI;

public class IndicatorManager : MonoBehaviour
{
    public static IndicatorManager Instance { get; private set; }

    [Header("References")]
    public Canvas canvas;                   // your UI Canvas
    public RectTransform arrowPrefab;       // the arrow Image prefab
    public Camera mainCamera;               // typically Camera.main

    [Header("Settings")]
    [Range(0, 0.5f)]
    public float screenEdgeBuffer = 0.05f;  // how far from the edge (in viewport coords)
    public float lifetime = 1f;             // how long the arrow stays on screen

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    /// <summary>
    /// Spawns an arrow on the canvas edge pointing to worldPos.
    /// </summary>
    public void ShowIndicator(Vector2 worldPos)
    {
        // Convert the world point to viewport (0..1, 0..1). Might be outside [0,1].
        Vector3 vp = mainCamera.WorldToViewportPoint(worldPos);
        // Markers only make sense for points behind the camera?
        if (vp.z < 0) vp = new Vector3(1 - vp.x, 1 - vp.y, vp.z);

        // Clamp to [buffer, 1-buffer]
        float vx = Mathf.Clamp(vp.x, screenEdgeBuffer, 1 - screenEdgeBuffer);
        float vy = Mathf.Clamp(vp.y, screenEdgeBuffer, 1 - screenEdgeBuffer);

        // Instantiate arrow
        RectTransform arrow = Instantiate(arrowPrefab, canvas.transform);
        // Anchor to the clamped viewport position
        arrow.anchorMin = arrow.anchorMax = new Vector2(vx, vy);
        arrow.anchoredPosition = Vector2.zero;

        // Compute rotation so it points TO the actual vp position from center (0.5,0.5)
        Vector2 fromCenter = new Vector2(vx - 0.5f, vy - 0.5f).normalized;
        float angle = Mathf.Atan2(fromCenter.y, fromCenter.x) * Mathf.Rad2Deg;
        arrow.localEulerAngles = new Vector3(0, 0, angle);

        // Auto‐destroy after lifetime
        Destroy(arrow.gameObject, lifetime);
    }
}
