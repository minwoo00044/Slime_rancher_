using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    float time;
    float minute;
    float second;
    float day;
    TMP_Text timeText;

    public Image timeImage;
    public Sprite sunImage;
    public Sprite moonImage;


    // Start is called before the first frame update
    void Start()
    {
        timeText = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    { 
        second += Time.deltaTime*60;
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
        }
        else
        {
            timeImage.sprite = moonImage;
        }
        timeText.text = minute.ToString("00") + " : " + Mathf.Floor(second).ToString("00");
    }
}
