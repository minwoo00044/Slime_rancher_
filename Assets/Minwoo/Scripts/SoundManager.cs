using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    public static SoundManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        GameObject soundObject = new GameObject("Sound");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
        Destroy(soundObject, clip.length);
    }

    public void PauseSound()
    {
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }
    }

    public void StopSound()
    {
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.Stop();
            audioSource.mute = true;
        }
    }
    public void StopSound(AudioClip clip)
    {
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.clip == clip)
            {
                audioSource.Stop();
            }
        }
    }


    public bool IsPlaying(AudioClip clip)
    {
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.clip == clip && audioSource.isPlaying)
            {
                return true;
            }
        }
        return false;
    }
}
