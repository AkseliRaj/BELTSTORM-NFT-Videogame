using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    float elapsedTime;
    private bool isTimerRunning = true; // Add a flag to control the timer

    void Update()
    {
        if (isTimerRunning) // Only update if the timer is running
        {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    // Public function to stop the timer
    public void StopTimer()
    {
        isTimerRunning = false;
        Debug.Log("Timer stopped!");
    }
}