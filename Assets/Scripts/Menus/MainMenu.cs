using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject infoUI;
    public SettingsMenu settingsMenu;

    [SerializeField] private TextMeshProUGUI bestTimeText;

    void Start()
    {
        ShowBestTime();
    }

    public void StartGame()
    {
        CurrencyManager.Instance.ResetSessionCoins();
        SceneManager.LoadScene("GameScene");
    }

    public void OpenSettings()
    {
        Debug.Log("Settings button clicked");

        if (settingsMenu != null)
        {
            settingsMenu.OpenSettings();
            mainMenuUI.SetActive(false);
        }
        else
        {
            Debug.LogWarning("SettingsMenu reference is missing!");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit button clicked");
        Application.Quit();
    }

    public void ShowInfoUI()
    {
        mainMenuUI.SetActive(false);
        infoUI.SetActive(true);
    }

    public void BackToMainMenu()
    {
        infoUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void OpenShop()
    {
        SceneManager.LoadScene("ShopScene");
    }

    /// Resets all player progress: coins, best time, and shop unlocks.
    public void ResetProgress()
    {
        // 1) Wipe saved coins
        PlayerPrefs.DeleteKey("TotalCoins");

        // 2) Wipe saved best time
        PlayerPrefs.DeleteKey("BestTime");

        // 3) Wipe all shop unlock flags
        if (ShopManager.Instance != null)
        {
            foreach (var item in ShopManager.Instance.items)
                PlayerPrefs.DeleteKey("Unlocked_" + item.id);
        }

        // 4) Persist deletions
        PlayerPrefs.Save();

        // 5) Reset in‑memory session coins
        CurrencyManager.Instance.ResetSessionCoins();

        // 6) Update the best‑time display
        ShowBestTime();

        Debug.Log("Player progress has been reset.");
    }

    private void ShowBestTime()
    {
        float bestTime = PlayerPrefs.GetFloat("BestTime", 0f);
        int minutes = Mathf.FloorToInt(bestTime / 60f);
        int seconds = Mathf.FloorToInt(bestTime % 60f);
        bestTimeText.text = $"Best Time {minutes:00}:{seconds:00}";
    }
}
