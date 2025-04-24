using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameOverUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject playerUI;         
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button mainMenuButton;

    private Timer gameTimer;

    void Awake()
    {
        gameTimer = FindObjectOfType<Timer>();
        if (gameTimer == null)
            Debug.LogError("No Timer found in scene!");

        // Ensure Game Over is hidden on start
        gameOverPanel.SetActive(false);
    }

    void OnEnable()
    {
        PlayerHealth.OnPlayerDied += ShowGameOver;
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerDied -= ShowGameOver;
    }

    private void ShowGameOver()
    {
        // stop timer
        if (gameTimer != null)
            gameTimer.StopTimer();

        // HIDE gameplay HUD
        if (playerUI != null)
            playerUI.SetActive(false);

        // SHOW game over UI
        gameOverPanel.SetActive(true);

        // populate coins
        int coins = CurrencyManager.Instance != null
            ? CurrencyManager.Instance.CoinTotal
            : 0;
        coinsText.text = $"{coins}";

        // populate time
        float elapsed = (gameTimer != null) 
            ? gameTimer.ElapsedTime 
            : 0f;
        int m = Mathf.FloorToInt(elapsed / 60f);
        int s = Mathf.FloorToInt(elapsed % 60f);
        timeText.text = $"{m:00}:{s:00}";

        // hook buttons
        retryButton.onClick.RemoveAllListeners();
        retryButton.onClick.AddListener(RestartLevel);

        mainMenuButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    private void RestartLevel()
    {
        // When the level reloads, Awake() will re-enable your HUD
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1f; // make sure timeScale is reset
        SceneManager.LoadScene("MainMenu");
    }
}
