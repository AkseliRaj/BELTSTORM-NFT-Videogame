using UnityEngine;
using TMPro;    // ← add this

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    public int CoinTotal { get; private set; }
    public TextMeshProUGUI coinUIText;  // ← use TextMeshProUGUI

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddCoins(int amount)
    {
        CoinTotal += amount;
        if (coinUIText != null)
            coinUIText.text = CoinTotal.ToString();
    }
}
