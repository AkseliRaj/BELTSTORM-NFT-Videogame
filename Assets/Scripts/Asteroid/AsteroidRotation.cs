using UnityEngine;

public class AsteroidRotation : MonoBehaviour
{
    public float rotationSpeed = 50f; // Rotation speed

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
