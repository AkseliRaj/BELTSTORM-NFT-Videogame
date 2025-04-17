using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MainMenuMusic : MonoBehaviour
{
    private static MainMenuMusic instance;
    private AudioSource audioSource;
    public float fadeDuration = 2f;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0f;
        audioSource.loop = true;
        audioSource.Play();

        StartCoroutine(FadeIn());
    }

    public void FadeOutAndDestroy()
    {
        StartCoroutine(FadeOutThenDestroy());
    }

    private IEnumerator FadeIn()
    {
        float targetVolume = 1f;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(0f, targetVolume, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    private IEnumerator FadeOutThenDestroy()
    {
        float startVolume = audioSource.volume;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = 0f;
        Destroy(gameObject);
    }
}
