using UnityEngine;
using TMPro;

public class CoinDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;

    void Start()
    {
        // Initialize with current value
        coinText.text = CurrencyManager.Instance.CoinTotal.ToString();
        // Subscribe
        CurrencyManager.Instance.OnCoinsChanged += UpdateDisplay;
    }

    void OnDestroy()
    {
        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.OnCoinsChanged -= UpdateDisplay;
    }

    private void UpdateDisplay(int newTotal)
    {
        coinText.text = newTotal.ToString();
    }
}
