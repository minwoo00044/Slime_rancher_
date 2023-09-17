using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public GameObject menuUI;
    public GameObject soundUI;
    public bool isPaused;

    bool isSoundOn = true;
    public GameObject SoundImage;


    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Pause();
    }

    public void Pause()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Play();
            }
            else
            {
                Player.Instance.isStop = true;
                isPaused = true;
                Time.timeScale = 0;
                menuUI.SetActive(true);
            }
        }
    }

    public void Play()
    {
        Time.timeScale = 1.0f;
        Player.Instance.isStop = false;
        isPaused = false;
        menuUI.SetActive(false);
        soundUI.SetActive(false);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void StartSoundUI()
    {
        menuUI.SetActive(false);
        soundUI.SetActive(true);
    }

    public void EndSoundUI()
    {
        menuUI.SetActive(true);
        soundUI.SetActive(false);
    }

    public void SoundButton()
    {
        if (!isSoundOn)
        {
            SoundImage.SetActive(false);
            isSoundOn = true;
        }
        else
        {
            SoundImage.SetActive(true) ;
            isSoundOn = false;
        }
    }

    public void ReturnMenu()
    {
        soundUI.SetActive(false);
        menuUI.SetActive(true);
    }
}
