using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class StarController : MonoBehaviour
{
    [Header("Fade Settings")]
    public float fadeCycleTime = 2f;

    [Header("Scale Settings")]
    public float maxScaleMultiplier = 1.5f;
    public float scaleCycleTime = 3f;

    [Header("Rotation Settings")]
    [Tooltip("Degrees per second (can be negative for counter-clockwise).")]
    public float rotationSpeed = 15f;

    private Graphic uiGraphic;
    private SpriteRenderer sr;
    private Color baseColor;
    private Vector3 baseScale;

    void Awake()
    {
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
        // Fade
        float alpha = Mathf.PingPong(Time.time / fadeCycleTime, 1f);
        Color c = baseColor;
        c.a = alpha;
        if (uiGraphic != null) uiGraphic.color = c;
        else if (sr != null) sr.color = c;

        // Scale
        float s = Mathf.PingPong(Time.time / scaleCycleTime, 1f);
        float scaleFactor = Mathf.Lerp(1f, maxScaleMultiplier, s);
        transform.localScale = baseScale * scaleFactor;

        // Spin
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
