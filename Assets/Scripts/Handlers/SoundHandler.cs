using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundHandler : MonoBehaviour
{
    internal static SoundHandler instance;
    internal int SoundEffectsVolume { get; set; }
    internal int MusicVolume { get; set; }
    [SerializeField] internal AudioSource[] skillSources;
    [SerializeField] internal AudioSource musicSource;
    [SerializeField] internal AudioSource hitSource;
    [SerializeField] internal AudioSource cursorSource;
    [SerializeField] private AudioClip[] hitSounds;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        int[] volumes = IOHandler.LoadSoundVolume();

        ChangeSoundEffectsVolume(volumes[0]);
        ChangeMusicVolume(volumes[1]);
    }

    internal void ChangeMusic(AudioClip music)
    {
        musicSource.clip = music;
        musicSource.volume = MusicVolume / 7f;
        musicSource.Play();
    }

    internal void PauseMusic()
    {
        if(musicSource.isPlaying)
        {
            musicSource.Pause();
        }
    }

    internal void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }

    internal void PlayMusic()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    internal void PlayHitSound()
    {
        if(!hitSource.isPlaying)
        {
            hitSource.clip = hitSounds[Random.Range(0, hitSounds.Length)];
            hitSource.volume = SoundEffectsVolume / 7f;
            hitSource.Play();
        }
    }

    internal void PlaySoundEffect(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.volume = SoundEffectsVolume / 7f;
        source.Play();
    }

    internal void PlayCursor()
    {
        cursorSource.volume = SoundEffectsVolume / 7f;
        cursorSource.Play();
    }

    internal void ChangeMusicVolume(int volume)
    {
        MusicVolume = volume;
        musicSource.volume = MusicVolume / 7f;
    }

    internal void ChangeSoundEffectsVolume(int volume)
    {
        SoundEffectsVolume = volume;
    }

    internal void MusicFadeOut()
    {
        StartCoroutine(MusicFadeOutCoroutine());
    }

    private IEnumerator MusicFadeOutCoroutine()
    {
        while(musicSource.volume > 0)
        {
            musicSource.volume -= 0.001f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
