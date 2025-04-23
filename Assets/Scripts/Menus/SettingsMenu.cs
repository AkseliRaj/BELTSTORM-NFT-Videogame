using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject mainMenuUI;
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        // Initialize with saved values (or defaults)
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        ApplyVolumes(); // Apply immediately on start
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.SetVolume(value);
        }
    }

   public void SetSFXVolume(float value)
{
    PlayerPrefs.SetFloat("SFXVolume", value);

    // You can apply this to sound effects manually later if needed
    // SFXManager.SetGlobalVolume(value);
}


    private void ApplyVolumes()
    {
        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);
    }
}
