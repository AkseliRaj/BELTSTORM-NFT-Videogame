using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public AudioClip menuTheme;
    public AudioClip gameTheme;
    public AudioClip shopTheme;


    public float fadeDuration = 2f;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        if (menuTheme != null)
        {
            audioSource.clip = menuTheme;
            audioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
            audioSource.Play();
            StartCoroutine(FadeIn());
        }

        // Listen for scene changes
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    if (scene.name == "GameScene")
        {
            if (gameTheme != null && audioSource.clip != gameTheme)
            {
                StartCoroutine(SwitchTrack(gameTheme));
            }
        }
        else if (scene.name == "ShopScene")
        {
            if (shopTheme != null && audioSource.clip != shopTheme)
            {
                StartCoroutine(SwitchTrack(shopTheme));
            }
        }
        else if (scene.name == "MainMenu")
        {
            if (menuTheme != null && audioSource.clip != menuTheme)
            {
                StartCoroutine(SwitchTrack(menuTheme));
            }
        }
}


    private IEnumerator SwitchTrack(AudioClip newClip)
    {
        float timer = 0f;
        float startVolume = audioSource.volume;

        while (timer < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.Play();

        timer = 0f;
        while (timer < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(0f, startVolume, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = startVolume;
    }

    private IEnumerator FadeIn()
    {
        float targetVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        Debug.Log("Loaded Music Volume (FadeIn target): " + targetVolume);

        float timer = 0f;

        while (timer < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(0f, targetVolume, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

}
