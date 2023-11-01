using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    internal static SoundHandler instance;
    [SerializeField] internal AudioSource[] skillSources;
    [SerializeField] internal AudioSource musicSource;
    [SerializeField] internal AudioSource hitSource;
    [SerializeField] private AudioClip[] hitSounds;

    private void Awake()
    {
        instance = this;
    }

    internal void ChangeMusic(AudioClip music)
    {
        musicSource.clip = music;
        musicSource.Play();
    }

    internal void PlayHitSound()
    {
        if(!hitSource.isPlaying)
        {
            hitSource.clip = hitSounds[Random.Range(0, hitSounds.Length)];
            hitSource.Play();
        }
    }
}
