using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
    public static UIAudioManager Instance;

    public AudioClip hoverSound;
    public AudioClip clickSound;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayHoverSound()
    {
        if (hoverSound != null && SFXManager.Instance != null)
        {
            SFXManager.Instance.PlaySound(hoverSound, Camera.main.transform.position);
        }
    }

    public void PlayClickSound()
    {
        if (clickSound != null && SFXManager.Instance != null)
        {
            SFXManager.Instance.PlaySound(clickSound, Camera.main.transform.position);
        }
    }
}
