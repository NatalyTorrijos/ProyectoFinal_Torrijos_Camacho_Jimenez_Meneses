using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public AudioSource audioSource; // Fuente principal de música

    public AudioClip normalMusic;   // Música normal
    public AudioClip bossMusic;     // Música del boss

    public float fadeDuration = 1.5f;

    private bool isBossMusic = false;

    private void Awake()
    {
        // Singleton
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

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        PlayMusic(normalMusic);
    }

    // Cambia la música con fundido
    public void PlayMusic(AudioClip newClip)
    {
        if (audioSource.clip == newClip) return;

        StopAllCoroutines();
        StartCoroutine(FadeMusic(newClip));
    }

    private IEnumerator FadeMusic(AudioClip newClip)
    {
        float startVolume = audioSource.volume;

        // Fade out
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.Play();

        // Fade in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = startVolume;
    }

    public void PlayNormalMusic()
    {
        if (!isBossMusic)
            PlayMusic(normalMusic);

        isBossMusic = false;
    }

    public void PlayBossMusic()
    {
        if (isBossMusic) return;

        PlayMusic(bossMusic);
        isBossMusic = true;
    }

    // Detectar entrada al área del boss
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayBossMusic();
        }
    }

    // Detectar salida
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayNormalMusic();
        }
    }
}
