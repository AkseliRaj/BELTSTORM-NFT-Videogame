using UnityEngine;
using TMPro;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;
    public GameObject pickupPopupPrefab;

    public AudioClip[] coinPickupSounds; // assign your coin pickup sounds
    public Vector2 pitchRange = new Vector2(0.95f, 1.05f); // Optional: pitch variety

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CurrencyManager.Instance.AddCoins(coinValue);

            // Spawn the popup
            if (pickupPopupPrefab != null)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
                Instantiate(pickupPopupPrefab, screenPos, Quaternion.identity,
                    FindObjectOfType<Canvas>().transform);
            }

            // Play a random coin sound using SFXManager
            if (coinPickupSounds != null && coinPickupSounds.Length > 0 && SFXManager.Instance != null)
            {
                AudioClip selectedClip = coinPickupSounds[Random.Range(0, coinPickupSounds.Length)];
                float randomPitch = Random.Range(pitchRange.x, pitchRange.y);
                SFXManager.Instance.PlaySoundWithPitch(selectedClip, transform.position, randomPitch);
            }

            Destroy(gameObject);
        }
    }
}
