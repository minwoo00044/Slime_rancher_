using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public AudioMixer master;
    public Slider slider;

    public void AudioControl()
    {
        float sound = slider.value;
        if (sound == 40f) master.SetFloat("Master", -80);
        else
        {
            master.SetFloat("Master", sound);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
