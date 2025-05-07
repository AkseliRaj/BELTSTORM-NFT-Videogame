// PauseMenu.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject settingsPanel; // Reference to the settings panel
    [SerializeField] private Button mainMenuButton;    // assign this in inspector

    private bool isPaused = false;

    void Start()
    {
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        gameplayUI?.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        gameplayUI?.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneFader.Instance.FadeToScene("MainMenu");
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
            pauseMenuUI.SetActive(true);
        }
    }
}
