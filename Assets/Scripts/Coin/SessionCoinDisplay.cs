using UnityEngine;
using TMPro;

public class SessionCoinDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;

    void Start()
    {
        // initialize
        coinText.text = CurrencyManager.Instance.SessionCoins.ToString();
        // subscribe
        CurrencyManager.Instance.OnSessionCoinsChanged += UpdateDisplay;
    }

    void OnDestroy()
    {
        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.OnSessionCoinsChanged -= UpdateDisplay;
    }

    private void UpdateDisplay(int newSessionTotal)
    {
        coinText.text = newSessionTotal.ToString();
    }
}
