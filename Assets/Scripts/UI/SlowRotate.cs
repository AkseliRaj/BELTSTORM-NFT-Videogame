using UnityEngine;

public class SlowRotate : MonoBehaviour
{
    // Voit s‰‰t‰‰ t‰t‰ Inspectorissa
    [SerializeField] private float rotationSpeed = 10f;

    void Update()
    {
        // Pyˆritet‰‰n objektia hitaasti z-akselin ymp‰ri (2D UI elementit yleens‰ t‰ss‰ suunnassa)
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
