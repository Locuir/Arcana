using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public AudioSource audioSource;

    public AudioClip normalWaveMusic;
    public AudioClip intenseWaveMusic;
    public AudioClip bossMusic;
    public AudioClip shopMusic;
    public AudioClip prepareMusic;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null)
            return;

        if (audioSource.clip == clip && audioSource.isPlaying)
            return;

        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayNormal()
    {
        PlayMusic(normalWaveMusic);
    }

    public void PlayIntense()
    {
        PlayMusic(intenseWaveMusic);
    }

    public void PlayBoss()
    {
        PlayMusic(bossMusic);
    }

    public void PlayShop()
    {
        PlayMusic(shopMusic);
    }

    public void PlayPrepare()
    {
        PlayMusic(prepareMusic);
    }
}