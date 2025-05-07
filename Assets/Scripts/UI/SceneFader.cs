// SceneFader.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public static SceneFader Instance;

    [Tooltip("Drag your fullscreen black Image here")]
    public Image fadeImage;

    [Tooltip("Seconds to fade in/out")]
    public float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // start invisible and disabled
        var c = fadeImage.color;
        fadeImage.color = new Color(c.r, c.g, c.b, 0f);
        fadeImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// Immediately show the black Image, then start fading.
    /// </summary>
    public void FadeToScene(string sceneName)
    {
        // <-- activate here so there's no lag
        fadeImage.gameObject.SetActive(true);
        StartCoroutine(DoFade(sceneName));
    }

    private IEnumerator DoFade(string sceneName)
    {
        // FADE OUT (0 → 1)
        yield return StartCoroutine(FadeAlpha(1f));

        // LOAD
        var op = SceneManager.LoadSceneAsync(sceneName);
        while (!op.isDone) yield return null;

        // FADE IN (1 → 0)
        yield return StartCoroutine(FadeAlpha(0f));

        // DISABLE until next time
        fadeImage.gameObject.SetActive(false);
    }

    private IEnumerator FadeAlpha(float targetAlpha)
    {
        float start = fadeImage.color.a;
        float elapsed = 0f;
        Color c = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float a = Mathf.Lerp(start, targetAlpha, elapsed / fadeDuration);
            fadeImage.color = new Color(c.r, c.g, c.b, a);
            yield return null;
        }
        fadeImage.color = new Color(c.r, c.g, c.b, targetAlpha);
    }
}
