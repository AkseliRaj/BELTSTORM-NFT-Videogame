using UnityEngine;

public class LogoRock : MonoBehaviour
{
    public float rockAngle = 10f;          // Max rotation angle in degrees
    public float rockSpeed = 2f;           // Speed of rocking

    private float startRotationZ;

    void Start()
    {
        // Save the initial rotation so we can rock around it
        startRotationZ = transform.rotation.eulerAngles.z;
    }

    void Update()
    {
        float angle = Mathf.Sin(Time.time * rockSpeed) * rockAngle;
        transform.rotation = Quaternion.Euler(0f, 0f, startRotationZ + angle);
    }
}
