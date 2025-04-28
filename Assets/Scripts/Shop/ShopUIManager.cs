using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ShopUIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image shipPreview;
    [SerializeField] private Button leftArrow, rightArrow;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button buyButton;
    [SerializeField] private TextMeshProUGUI buyButtonText;

    private List<ShopItem> items;
    private int currentIndex = 0;

    void Start()
    {
        items = ShopManager.Instance.items;
        leftArrow.onClick.AddListener(PrevSkin);
        rightArrow.onClick.AddListener(NextSkin);
        buyButton.onClick.AddListener(OnBuy);

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
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    private void Equip(string id)
    {
        PlayerPrefs.SetString("EquippedSkin", id);
        PlayerPrefs.Save();
    }
}
