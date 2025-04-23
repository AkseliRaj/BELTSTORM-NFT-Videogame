using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject infoUI;
    public SettingsMenu settingsMenu;


    // Method to load the game scene when 'Start' is clicked.
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    // Method to open the settings menu when 'Settings' is clicked.
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


    // Method to quit the game when 'Quit' is clicked.
    public void QuitGame()
    {
        Debug.Log("Quit button clicked");
        Application.Quit();
    }

    // Method to show the Info UI and hide the Main Menu UI
    public void ShowInfoUI()
    {
        mainMenuUI.SetActive(false);
        infoUI.SetActive(true);
    }

    // Optional: Method to go back to the Main Menu from Info UI
    public void BackToMainMenu()
    {
        infoUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }
}
