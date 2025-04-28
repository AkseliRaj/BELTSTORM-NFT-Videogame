using UnityEngine;
using System.Linq;            // Needed for FirstOrDefault()

public class PlayerSkinController : MonoBehaviour
{
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // 1) Load which skin was last equipped
        string equippedId = PlayerPrefs.GetString("EquippedSkin", "");
        if (string.IsNullOrEmpty(equippedId))
            return;

        // 2) Find the matching ShopItem in your ShopManager
        var item = ShopManager.Instance.items
                     .FirstOrDefault(i => i.id == equippedId);

        // 3) If found, apply its sprite
        if (!string.IsNullOrEmpty(item.id))
            sr.sprite = item.previewImage;
    }
}
