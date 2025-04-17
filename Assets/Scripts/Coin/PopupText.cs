using UnityEngine;
using TMPro;

public class PopupText : MonoBehaviour
{
    public float floatSpeed = 40f;
    public float fadeDuration = 0.5f;

    private TextMeshProUGUI text;
    private float elapsed;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        elapsed += Time.deltaTime;

        // Move upward
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        // Fade out
        float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);

        // Destroy when finished
        if (elapsed >= fadeDuration)
            Destroy(gameObject);
    }
}
