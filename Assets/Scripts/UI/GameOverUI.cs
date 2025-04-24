using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject playerUI;         
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI newBestText;  // Optional: Drag in inspector
    [SerializeField] private Button retryButton;
    [SerializeField] private Button mainMenuButton;

    private Timer gameTimer;

    void Awake()
    {
        gameTimer = FindObjectOfType<Timer>();
        if (gameTimer == null)
            Debug.LogError("No Timer found in scene!");

        gameOverPanel.SetActive(false);

        if (newBestText != null)
            newBestText.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        PlayerHealth.OnPlayerDied += ShowGameOver;
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerDied -= ShowGameOver;
    }

    // This ensures no lingering subscriptions cause errors
    void OnDestroy()
    {
        PlayerHealth.OnPlayerDied -= ShowGameOver;
    }

    private void ShowGameOver()
    {
        if (gameTimer != null)
            gameTimer.StopTimer();

        if (playerUI != null)
            playerUI.SetActive(false);

        gameOverPanel.SetActive(true);

        int coins = CurrencyManager.Instance != null ? CurrencyManager.Instance.CoinTotal : 0;
        coinsText.text = $"{coins}";

        float elapsed = gameTimer != null ? gameTimer.ElapsedTime : 0f;
        int m = Mathf.FloorToInt(elapsed / 60f);
        int s = Mathf.FloorToInt(elapsed % 60f);
        timeText.text = $"{m:00}:{s:00}";

        // Handle new best logic
        float bestTime = PlayerPrefs.GetFloat("BestTime", 0f);
        bool isNewBest = elapsed > bestTime;

        if (isNewBest)
        {
            PlayerPrefs.SetFloat("BestTime", elapsed);
            PlayerPrefs.Save();
        }

        if (newBestText != null)
            newBestText.gameObject.SetActive(isNewBest);

        retryButton.onClick.RemoveAllListeners();
        retryButton.onClick.AddListener(RestartLevel);

        mainMenuButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    void OnSceneLoaded(Scene s, LoadSceneMode m) {
    gameOverPanel = GameObject.Find("GameOverPanel");
    gameOverPanel.SetActive(false);
}

}
