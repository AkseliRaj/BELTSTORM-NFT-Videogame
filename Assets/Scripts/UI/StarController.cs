using UnityEngine;
using UnityEngine.UI;  // for UI Image. Remove if using SpriteRenderer.

[RequireComponent(typeof(RectTransform))]
public class StarController : MonoBehaviour
{
    [Header("Fade Settings")]
    [Tooltip("Seconds for full fade-in → fade-out cycle.")]
    public float fadeCycleTime = 2f;

    [Header("Scale Settings")]
    [Tooltip("Peak scale multiplier. 1 = no change.")]
    public float maxScaleMultiplier = 1.5f;
    [Tooltip("Seconds for full grow → shrink cycle.")]
    public float scaleCycleTime = 3f;

    private Graphic uiGraphic;       // for UI Image/Text
    private SpriteRenderer sr;       // for world-space Sprite
    private Color baseColor;
    private Vector3 baseScale;

    void Awake()
    {
        // Cache components
        uiGraphic = GetComponent<Graphic>();
        if (uiGraphic != null)
            baseColor = uiGraphic.color;
        else if ((sr = GetComponent<SpriteRenderer>()) != null)
            baseColor = sr.color;
        else
            Debug.LogWarning("StarController needs an Image/Graphic or SpriteRenderer!");

        baseScale = transform.localScale;
    }

    void Update()
    {
        // Fade: alpha = PingPong(t) in [0,1]
        float alpha = Mathf.PingPong(Time.time / fadeCycleTime, 1f);
        Color c = baseColor;
        c.a = alpha;
        if (uiGraphic != null) uiGraphic.color = c;
        else if (sr != null) sr.color = c;

        // Scale: factor = Lerp(1, maxScale, PingPong(t))
        float s = Mathf.PingPong(Time.time / scaleCycleTime, 1f);
        float scaleFactor = Mathf.Lerp(1f, maxScaleMultiplier, s);
        transform.localScale = baseScale * scaleFactor;
    }
}
