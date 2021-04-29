using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public Slider Master;
    public Slider Music;
    public Slider SoundEffects;

    public GameObject Menu;
    public GameObject Settings;
    private bool isIgnore = true;
    // Start is called before the first frame update
    void Start()
    {
        ApplicationManager.GetInstance();

        Master.value = GameManager.MasterVolume;
        Music.value = GameManager.BackGroundVolume;
        SoundEffects.value = GameManager.SoundEffectVolume;

        isIgnore = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<AudioAgent>().IsAudioStopped("StartScreen"))
        {
            GetComponent<AudioAgent>().PlayBackground("StartScreen", true, 10);
        }

        GameManager.MasterVolume = Master.value;
        GameManager.BackGroundVolume = Music.value;
        GameManager.SoundEffectVolume = SoundEffects.value;
    }

    public void ResetScore()
    {
        GameManager.HighScore = 0f;
    }
    public void PlaySoundEffect()
    {
        if(!isIgnore)
        {
            if (GetComponent<AudioAgent>().IsAudioStopped("ShootPew"))
                GetComponent<AudioAgent>().PlaySoundEffect("ShootPew");
        }
    }

    public void ShowTitle()
    {
        Menu.SetActive(true);
        Settings.SetActive(false);
    }

    public void ShowSettings()
    {
        Menu.SetActive(false);
        Settings.SetActive(true);
    }
}
