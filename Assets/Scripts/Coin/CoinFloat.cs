using UnityEngine;

public class CoinFloat : MonoBehaviour
{
    public float floatAmplitude = 0.25f;   // how high it moves up/down
    public float floatFrequency = 2f;      // speed of the motion

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = startPos + new Vector3(0, yOffset, 0);
        transform.Rotate(Vector3.forward * 100f * Time.deltaTime);

    }
}
