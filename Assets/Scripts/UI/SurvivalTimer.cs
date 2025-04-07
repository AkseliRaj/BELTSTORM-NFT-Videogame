using UnityEngine;
using TMPro; // Use this if you're using TextMeshPro

public class SurvivalTimer : MonoBehaviour
{
    public TMP_Text timerText;           // Reference to the TextMeshPro UI Text element
    public PlayerHealth playerHealth;    // Reference to the player's health script

    private float startTime;             
    private bool isPlayerAlive = true;  

    void Start()
    {
        startTime = Time.time;
        UpdateTimerText(0f);
    }

    void Update()
    {
        if (isPlayerAlive)
        {
            if (playerHealth != null)
            {
                float elapsedTime = Time.time - startTime;
                UpdateTimerText(elapsedTime);
            }
            else
            {
                isPlayerAlive = false;
                float finalTime = Time.time - startTime;
                UpdateTimerText(finalTime);
                Debug.Log("Player survived for: " + finalTime.ToString("F2") + " seconds");
            }
        }
    }

    void UpdateTimerText(float time)
    {
        timerText.text = "Survived: " + time.ToString("F2") + " seconds";
    }
}
