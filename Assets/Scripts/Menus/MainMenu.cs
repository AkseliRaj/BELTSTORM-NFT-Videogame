// MainMenu.cs
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject infoUI;
    public SettingsMenu settingsMenu;
    [SerializeField] private TextMeshProUGUI bestTimeText;

    private void Start()
    {
        ShowBestTime();
    }

    public void StartGame()
    {
        CurrencyManager.Instance.ResetSessionCoins();
        SceneFader.Instance.FadeToScene("GameScene");
    }

    public void OpenShop()
    {
        SceneFader.Instance.FadeToScene("ShopScene");
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

    public void QuitGame()
    {
        Debug.Log("Quit button clicked");
        Application.Quit();
    }

    public void ResetProgress()
    {
        // clear saved data
        PlayerPrefs.DeleteKey("TotalCoins");
        PlayerPrefs.DeleteKey("BestTime");
        if (ShopManager.Instance != null)
        {
            foreach (var item in ShopManager.Instance.items)
                PlayerPrefs.DeleteKey("Unlocked_" + item.id);
        }
        PlayerPrefs.Save();

        // reset session and UI
        CurrencyManager.Instance.ResetSessionCoins();
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
