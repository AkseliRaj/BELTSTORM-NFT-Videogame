using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject settingsPanel; // Reference to the settings panel

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        if (gameplayUI != null)
            gameplayUI.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        if (gameplayUI != null)
            gameplayUI.SetActive(false); // Optionally hide gameplay UI
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void CloseSettings()
    {
        // Close the settings panel and return to the pause menu
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
            pauseMenuUI.SetActive(true); // Show pause menu again
        }
    }
}
