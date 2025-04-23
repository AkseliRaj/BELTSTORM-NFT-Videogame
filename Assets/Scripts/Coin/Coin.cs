using UnityEngine;
using TMPro;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;
    public GameObject pickupPopupPrefab;

    public AudioClip[] coinPickupSounds; // assign your two (or more) coin pickup sounds here
    public float volume = 1f;

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

            // Play a random coin sound
            if (coinPickupSounds != null && coinPickupSounds.Length > 0)
            {
                AudioClip selectedClip = coinPickupSounds[Random.Range(0, coinPickupSounds.Length)];
                AudioSource.PlayClipAtPoint(selectedClip, transform.position, volume);
            }

            Destroy(gameObject);
        }
    }
}
