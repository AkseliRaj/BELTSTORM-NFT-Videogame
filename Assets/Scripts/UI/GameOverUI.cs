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
    [SerializeField] private TextMeshProUGUI newBestText;
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
            newBestText.gameObject.SetActive(false); // Ensure it's hidden initially
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
        if (gameTimer != null)
            gameTimer.StopTimer();

        if (playerUI != null)
            playerUI.SetActive(false);

        gameOverPanel.SetActive(true);

        // Update coins
        int coins = CurrencyManager.Instance != null ? CurrencyManager.Instance.CoinTotal : 0;
        coinsText.text = $"{coins}";

        // Update time
        float elapsed = gameTimer != null ? gameTimer.ElapsedTime : 0f;

        // Check for new best
        float bestTime = PlayerPrefs.GetFloat("BestTime", 0f);
        bool isNewBest = elapsed > bestTime;

        if (isNewBest)
        {
            PlayerPrefs.SetFloat("BestTime", elapsed);
            PlayerPrefs.Save();
            Debug.Log("New best time saved: " + elapsed);
        }

        // Format and display time
        int m = Mathf.FloorToInt(elapsed / 60f);
        int s = Mathf.FloorToInt(elapsed % 60f);
        timeText.text = $"{m:00}:{s:00}";

        // Show NEW BEST if applicable
        if (newBestText != null)
            newBestText.gameObject.SetActive(isNewBest);

        // Setup buttons
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
}
