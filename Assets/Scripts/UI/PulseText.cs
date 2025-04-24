using UnityEngine;

public class PulseText : MonoBehaviour
{
    [SerializeField] private float pulseSpeed = 2f;     // How fast it pulses
    [SerializeField] private float pulseStrength = 0.1f; // How much it scales up/down

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        float scale = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseStrength;
        transform.localScale = originalScale * scale;
    }
}
