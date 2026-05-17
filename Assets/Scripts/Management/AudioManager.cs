using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField][Range(0f, 1f)] private float musicVolume = 0.25f;

    [Header("SFX")]
    [SerializeField] private AudioClip enemyHitClip;
    [SerializeField] private AudioClip playerHitClip;
    [SerializeField] private AudioClip sceneTransitionClip;
    [SerializeField] private AudioClip gameOverClip;
    [SerializeField][Range(0f, 1f)] private float sfxVolume = 0.8f;

    protected override void Awake()
    {
        base.Awake();
        ResolveAudioSources();
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayEnemyHit()
    {
        PlaySFX(enemyHitClip);
    }

    public void PlayPlayerHit()
    {
        PlaySFX(playerHitClip);
    }

    public void PlaySceneTransition()
    {
        PlaySFX(sceneTransitionClip);
    }

    public void PlayGameOver()
    {
        StopBackgroundMusic();
        PlaySFX(gameOverClip);
    }

    public void StopBackgroundMusic()
    {
        if (musicSource == null) { return; }

        musicSource.Stop();
    }

    private void ResolveAudioSources()
    {
        if (musicSource == null)
        {
            GameObject musicObject = new GameObject("Music Source");
            musicObject.transform.SetParent(transform);
            musicSource = musicObject.AddComponent<AudioSource>();
        }

        if (sfxSource == null)
        {
            GameObject sfxObject = new GameObject("SFX Source");
            sfxObject.transform.SetParent(transform);
            sfxSource = sfxObject.AddComponent<AudioSource>();
        }

        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = musicVolume;

        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        sfxSource.volume = sfxVolume;
    }

    private void PlayBackgroundMusic()
    {
        if (backgroundMusic == null || musicSource == null) { return; }
        if (musicSource.isPlaying && musicSource.clip == backgroundMusic) { return; }

        musicSource.clip = backgroundMusic;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) { return; }

        sfxSource.PlayOneShot(clip, sfxVolume);
    }
}
