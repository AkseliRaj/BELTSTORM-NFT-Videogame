using UnityEngine;
using TMPro;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;
    public GameObject pickupPopupPrefab;  // assign your PopupText prefab here

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CurrencyManager.Instance.AddCoins(coinValue);

            // Spawn the popup
            if (pickupPopupPrefab != null)
            {
                // Convert world to screen position
                Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

                // Instantiate under canvas
                GameObject popup = Instantiate(pickupPopupPrefab, screenPos, Quaternion.identity, 
                    FindObjectOfType<Canvas>().transform);
            }

            Destroy(gameObject);
        }
    }
}
