using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    public int CoinTotal { get; private set; }
    [SerializeField] private TextMeshProUGUI coinUIText;

    private const string CoinsKey = "Coins";

    void Awake()
    {
        // Singleton + persist across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadCoins();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadCoins()
    {
        CoinTotal = PlayerPrefs.GetInt(CoinsKey, 0);
        UpdateUI();
    }

    private void SaveCoins()
    {
        PlayerPrefs.SetInt(CoinsKey, CoinTotal);
        PlayerPrefs.Save();
    }

    public void AddCoins(int amount)
    {
        CoinTotal += amount;
        UpdateUI();
        SaveCoins();
    }

    private void UpdateUI()
    {
        if (coinUIText != null)
            coinUIText.text = CoinTotal.ToString();
    }

    // Optionally also save on quit/ pause
    void OnApplicationQuit()    => SaveCoins();
    void OnApplicationPause(bool paused)
    {
        if (paused) SaveCoins();
    }
}
