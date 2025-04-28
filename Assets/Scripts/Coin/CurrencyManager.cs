using UnityEngine;
using System;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    /// Fired when total coins change
    public event Action<int> OnCoinsChanged;
    /// Fired when session coins change
    public event Action<int> OnSessionCoinsChanged;

    private const string PLAYERPREFS_COINS = "TotalCoins";

    int totalCoins;
    int sessionCoins;
    public int CoinTotal   => totalCoins;
    public int SessionCoins => sessionCoins;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        totalCoins = PlayerPrefs.GetInt(PLAYERPREFS_COINS, 0);
        sessionCoins = 0;
    }

    /// <summary>Call this at the very start of each new game session</summary>
    public void ResetSessionCoins()
    {
        sessionCoins = 0;
        OnSessionCoinsChanged?.Invoke(sessionCoins);
    }

    /// <summary>Adds coins both to your saved total and to the current session.</summary>
    public void AddCoins(int amount)
    {
        // 1) Persistent total
        totalCoins += amount;
        PlayerPrefs.SetInt(PLAYERPREFS_COINS, totalCoins);
        PlayerPrefs.Save();
        OnCoinsChanged?.Invoke(totalCoins);

        // 2) Session-only
        sessionCoins += amount;
        OnSessionCoinsChanged?.Invoke(sessionCoins);
    }

    /// <summary>Spend from your persistent total (sessionCoins unaffected).</summary>
    public bool SpendCoins(int amount)
    {
        if (totalCoins < amount) return false;
        totalCoins -= amount;
        PlayerPrefs.SetInt(PLAYERPREFS_COINS, totalCoins);
        PlayerPrefs.Save();
        OnCoinsChanged?.Invoke(totalCoins);
        return true;
    }
}
