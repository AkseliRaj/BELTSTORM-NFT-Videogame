using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float lifetime = 2f; // Time before the laser is destroyed

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
