using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    float time;
    float minute = 0;
    float second;
    float day;
    TMP_Text timeText;

    public Image timeImage;
    public Sprite sunImage;
    public Sprite moonImage;

    public Material dayMaterial;
    public Material nightMaterial;

    public float skyboxRotateSpeed;
    public Light light;
    // Start is called before the first frame update
    void Start()
    {
        timeText = GetComponentInChildren<TMP_Text>();
        RenderSettings.skybox = nightMaterial;
        nightMaterial.SetFloat("_Exposure", 0.5f);
    }

    // Update is called once per frame
    void Update()
    { 
        second += Time.deltaTime*60;

        if(minute < 6)
        {
            nightMaterial.SetFloat("_Exposure", 0.5f + (second + minute * 60) / 360 * 0.5f);
        }
        else if(minute < 12)
        {
            nightMaterial.SetFloat("_Exposure", 1f + (second + minute * 60 - 360) / 360 * 3f);
        }
        else if(minute < 18)
        {
            nightMaterial.SetFloat("_Exposure", 4f - (second + minute * 60 - 720) / 360 * 3f);
        }
        else
        {
            nightMaterial.SetFloat("_Exposure", 1f - (second + minute * 60 - 1080) / 360 * 0.5f);
        }

        RenderSettings.skybox.SetFloat("_Rotation", (second + minute*60) * skyboxRotateSpeed);
        if(second >= 60)
        {
            second -= 60;
            minute += 1;
        }
        if(minute >= 24)
        {
            minute -= 24;
            day += 1;
        }
        if(minute >= 6 && minute <= 18)
        {
            timeImage.sprite = sunImage;
 //           light.intensity -= (0.85f / 720) * Time.deltaTime*60;
            
        }
        else
        {
            timeImage.sprite = moonImage;
        }
        timeText.text = minute.ToString("00") + " : " + Mathf.Floor(second).ToString("00");
    }
}
