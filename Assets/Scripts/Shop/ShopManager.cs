using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct ShopItem
{
    public string id;           // unique key, e.g. "Skin_Red"
    public int price;           // cost in coins
    public Sprite previewImage; // for UI
}

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }
    public List<ShopItem> items;

    public event Action OnShopUpdated;

    private const string PREFS_UNLOCK_PREFIX = "Unlocked_";

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public bool IsUnlocked(string itemId)
    {
        return PlayerPrefs.GetInt(PREFS_UNLOCK_PREFIX + itemId, 0) == 1;
    }

        public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public bool TryPurchase(string itemId)
    {
        var item = items.Find(i => i.id == itemId);
        if (item.id == null || CurrencyManager.Instance.CoinTotal < item.price) return false;

        bool success = CurrencyManager.Instance.SpendCoins(item.price);
        if (!success) return false;

        PlayerPrefs.SetInt(PREFS_UNLOCK_PREFIX + itemId, 1);
        PlayerPrefs.Save();
        OnShopUpdated?.Invoke();
        return true;
    }
}
