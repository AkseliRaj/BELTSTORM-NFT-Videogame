// ShopUIManager.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
// add this so you can call SceneFader.Instance
using UnityEngine.SceneManagement;

public class ShopUIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image shipPreview;
    [SerializeField] private Button leftArrow, rightArrow;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button buyButton;
    [SerializeField] private TextMeshProUGUI buyButtonText;
    [SerializeField] private Button backButton; // assign this in inspector

    private List<ShopItem> items;
    private int currentIndex = 0;

    void Start()
    {
        items = ShopManager.Instance.items;
        leftArrow.onClick.AddListener(PrevSkin);
        rightArrow.onClick.AddListener(NextSkin);
        buyButton.onClick.AddListener(OnBuy);

        // hook up back button to fade
        backButton.onClick.AddListener(BackToMainMenu);

        RefreshUI();
    }

    void RefreshUI()
    {
        var item = items[currentIndex];
        shipPreview.sprite = item.previewImage;

        bool unlocked = ShopManager.Instance.IsUnlocked(item.id);
        int coins    = CurrencyManager.Instance.CoinTotal;

        if (unlocked)
        {
            priceText.text         = "Owned";
            buyButtonText.text     = "Equip";
            buyButton.interactable = true;
        }
        else
        {
            priceText.text         = item.price.ToString();
            buyButtonText.text     = "Buy";
            buyButton.interactable = (coins >= item.price);
        }
    }

    public void NextSkin()
    {
        currentIndex = (currentIndex + 1) % items.Count;
        RefreshUI();
    }

    public void PrevSkin()
    {
        currentIndex = (currentIndex - 1 + items.Count) % items.Count;
        RefreshUI();
    }

    private void OnBuy()
    {
        var item = items[currentIndex];
        if (ShopManager.Instance.IsUnlocked(item.id))
        {
            Equip(item.id);
        }
        else if (ShopManager.Instance.TryPurchase(item.id))
        {
            Equip(item.id);
        }
        RefreshUI();
    }

    public void BackToMainMenu()
    {
        // instead of immediate load, fade out → load → fade in
        SceneFader.Instance.FadeToScene("MainMenu");
    }

    private void Equip(string id)
    {
        PlayerPrefs.SetString("EquippedSkin", id);
        PlayerPrefs.Save();
    }
}
