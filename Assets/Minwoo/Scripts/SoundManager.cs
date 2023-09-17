using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    public static SoundManager Instance { get { return instance; } }

    public bool isMute;

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
        audioSource.mute = isMute;
        audioSource.clip = clip;
        audioSource.Play();
        Destroy(soundObject, clip.length);
    }
    public void PlaySound(AudioClip clip, float volume)
    {
        GameObject soundObject = new GameObject("Sound");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
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
    public void Play3DSoundAtLocation(AudioClip clip, Vector3 location, float volume = 1.0f)
    {
        GameObject soundObject = new GameObject("Sound");
        soundObject.transform.position = location;

        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;

        audioSource.spatialBlend = 1.0f;
        audioSource.minDistance = 1.0f;
        audioSource.maxDistance = 10.0f;

        audioSource.Play();


        Destroy(soundObject, clip.length);
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
            audioSource.Stop();
            //if (audioSource.clip == clip)
            //{
            //    audioSource.Stop();
            //}
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

    public void SoundOnOff()
    {
        if (isMute)
        {
            isMute = false;
        }
        else isMute = true;
    }
}
