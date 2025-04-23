using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [Range(0f, 1f)]
    public float globalVolume = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        globalVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    public void SetGlobalVolume(float volume)
    {
        globalVolume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void PlaySound(AudioClip clip, Vector3 position)
    {
        if (clip == null) return;

        AudioSource.PlayClipAtPoint(clip, position, globalVolume);
    }

    public void PlaySoundWithPitch(AudioClip clip, Vector3 position, float pitch)
    {
        if (clip == null) return;

        GameObject temp = new GameObject("TempSFX");
        AudioSource source = temp.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = globalVolume;
        source.pitch = pitch;
        source.Play();

        Destroy(temp, clip.length / Mathf.Abs(pitch));
    }
}
