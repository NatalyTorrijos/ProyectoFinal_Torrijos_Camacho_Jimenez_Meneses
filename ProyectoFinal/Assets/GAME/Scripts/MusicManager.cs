using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Audio Source principal")]
    public AudioSource audioSource;

    [Header("Música de la escena")]
    public AudioClip normalMusic;
    public AudioClip bossMusic;

    [Header("Opciones de transición")]
    public float fadeDuration = 1.5f; 

    private bool isBossMusic = false;

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

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        PlayMusic(normalMusic);
    }

    public void PlayMusic(AudioClip newClip)
    {
        if (audioSource.clip == newClip) return;
        StopAllCoroutines();
        StartCoroutine(FadeMusic(newClip));
    }

    private IEnumerator FadeMusic(AudioClip newClip)
    {
        float startVolume = audioSource.volume;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.Play();

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayBossMusic();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayNormalMusic();
        }
    }
}
