using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    internal static SoundHandler instance;
    internal float soundEffectsVolume, musicVolume;
    [SerializeField] internal AudioSource[] skillSources;
    [SerializeField] internal AudioSource musicSource;
    [SerializeField] internal AudioSource hitSource;
    [SerializeField] internal AudioSource cursorSource;
    [SerializeField] private AudioClip[] hitSounds;

    private void Awake()
    {
        instance = this;
        soundEffectsVolume = 0.5f;
        musicVolume = 0.5f;
    }

    internal void ChangeMusic(AudioClip music)
    {
        musicSource.clip = music;
        musicSource.volume = musicVolume;
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
            hitSource.volume = soundEffectsVolume;
            hitSource.Play();
        }
    }

    internal void PlaySoundEffect(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.volume = soundEffectsVolume;
        source.Play();
    }

    internal void PlayCursor()
    {
        cursorSource.volume = soundEffectsVolume;
        cursorSource.Play();
    }

    internal void ChangeMusicVolume(float volume)
    {
        musicVolume = volume / 7f;
        musicSource.volume = musicVolume;
    }

    internal void ChangeSoundEffectsVolume(float volume)
    {
        soundEffectsVolume = volume / 7f;
    }
}
