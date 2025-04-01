using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float parallaxFactor = 0.05f; // The amount of background movement (lower = subtler)
    public float lerpSpeed = 5f; // How quickly the background follows the player

    private Vector3 initialPosition; // Background's starting position
    private Vector3 playerInitialPosition; // Player's starting position

    void Start()
    {
        // Record the initial positions
        initialPosition = transform.position;
        playerInitialPosition = player.position;
    }

    void Update()
    {
        if (player != null)
        {
            // Calculate the player's movement relative to its initial position
            Vector3 playerDelta = player.position - playerInitialPosition;

            // Determine the target position for the background
            Vector3 targetPosition = initialPosition + playerDelta * parallaxFactor;

            // Smoothly move the background towards the target position using Lerp
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed);
        }
    }
}
