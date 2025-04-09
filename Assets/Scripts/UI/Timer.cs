using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    float elapsedTime;
    private bool isTimerRunning = true;

    // Public property to allow other scripts to read the elapsed time.
    public float ElapsedTime
    {
        get { return elapsedTime; }
    }

    void Update()
    {
        if (isTimerRunning)
        {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void StopTimer()
    {
        isTimerRunning = false;
        Debug.Log("Timer stopped!");
    }
}
