using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Method to load the game scene when 'Start' is clicked.
    public void StartGame()
{
    var music = GameObject.FindObjectOfType<MainMenuMusic>();
    if (music != null)
        music.FadeOutAndDestroy();

    SceneManager.LoadScene("GameScene");
}


    // Method to open the settings menu when 'Settings' is clicked.
    public void OpenSettings()
    {
        Debug.Log("Settings button clicked");
    }

    // Method to quit the game when 'Quit' is clicked.
    public void QuitGame()
    {
        Debug.Log("Quit button clicked");
        Application.Quit();
    }
}
