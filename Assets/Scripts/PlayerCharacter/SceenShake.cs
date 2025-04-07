using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceenShake : MonoBehaviour
{
    public float duration = 1f; 

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Shaking() {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            transform.position = startPosition + Random.insideUnitSphere;
            yield return null;
        }

        transform.position = startPosition;
    }
}
