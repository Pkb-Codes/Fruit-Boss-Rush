using UnityEngine;
using System.Collections;

public class BossMusicController : MonoBehaviour
{
    public AudioClip musicSource;
    private AudioSource audioSource;
    public float fadeDuration = 2f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = musicSource;
    }

    public void StartBossMusic()
    {
        audioSource.volume = 0f;
        audioSource.Play();
        StartCoroutine(FadeIn());
    }

    public void StopBossMusic()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = 1f;
    }

    IEnumerator FadeOut()
    {
        float startVolume = audioSource.volume;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
