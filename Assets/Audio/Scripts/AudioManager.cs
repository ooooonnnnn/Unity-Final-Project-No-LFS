using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioManager instance;

    private void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip[] musicClips;
    [SerializeField] private AudioClip[] sfxClips;

    public void SetMusicVolume(float value)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }

    public void SetSFXVolume(float value)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
    }

    /// <param name="index">0 = Main Menu, 1 = Level 1, 2 = Level 2, etc.</param>
    public void PlayMusic(int index)
    {
        if (index < 0 || index >= musicClips.Length) return;
        musicSource.clip = musicClips[index];
        musicSource.Play();
    }

    public void PlaySfx(int index)
    {
        if (index < 0 || index >= sfxClips.Length) return;
        sfxSource.PlayOneShot(sfxClips[index]);
    }

    // For unity events, we can have specific methods for each music and sfx to avoid needing to pass indices
    public void PlayMainMenuMusic() => PlayMusic(0);
    public void PlayLevel1Music() => PlayMusic(1);
    public void PlayLevel2Music() => PlayMusic(2);
    public void PlayLevel3Music() => PlayMusic(3);
    
    public void PlayCastSfx() => PlaySfx(0);
    public void PlaySpellHitSfx() => PlaySfx(1);
    public void PlayCastFailedSfx() => PlaySfx(2);
    public void PlayPlayerHitSfx() => PlaySfx(3);
}