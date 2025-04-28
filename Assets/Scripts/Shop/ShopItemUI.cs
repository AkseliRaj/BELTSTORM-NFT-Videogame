using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    [Header("UI References")]
    public Image previewImage;
    public TextMeshProUGUI priceText;
    public Button actionButton;
    public TextMeshProUGUI actionButtonText;

    [Header("Item Data")]
    public string itemId;

    void Start()
    {
        Refresh();
        ShopManager.Instance.OnShopUpdated += Refresh;
    }

    void OnDestroy()
    {
        if (ShopManager.Instance != null)
            ShopManager.Instance.OnShopUpdated -= Refresh;
    }

    public void Refresh()
    {
        var item = ShopManager.Instance.items.Find(i => i.id == itemId);
        previewImage.sprite = item.previewImage;
        bool unlocked = ShopManager.Instance.IsUnlocked(itemId);

        if (unlocked)
        {
            priceText.text = "Owned";
            actionButtonText.text = "Equip";
            actionButton.interactable = true;
        }
        else
        {
            priceText.text = item.price.ToString();
            actionButtonText.text = "Buy";
            actionButton.interactable = CurrencyManager.Instance.CoinTotal >= item.price;
        }
        
        actionButton.onClick.RemoveAllListeners();
        actionButton.onClick.AddListener(() =>
        {
            if (unlocked)
                EquipSkin(itemId);
            else
                TryBuy(itemId);
        });
    }

    private void TryBuy(string id)
    {
        if (ShopManager.Instance.TryPurchase(id))
            Refresh();
    }

    private void EquipSkin(string id)
    {
        // Your logic to set the player’s skin, e.g.:
        PlayerPrefs.SetString("EquippedSkin", id);
        // Possibly notify your ship renderer, etc.
    }
}
