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

    public RadioBoxScript player1Box;
    public RadioBoxScript player2Box;

    public Text[] player1Controls;
    public Text[] player2Controls;

    private bool isIgnore = true;

    // Start is called before the first frame update
    void Start()
    {
        ApplicationManager.GetInstance();

        Master.value = GameManager.MasterVolume;
        Music.value = GameManager.BackGroundVolume;
        SoundEffects.value = GameManager.SoundEffectVolume;

        isIgnore = false;
        ShowTitle();
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<AudioAgent>().IsAudioStopped("StartScreen"))
        {
            GetComponent<AudioAgent>().PlayBackground("StartScreen", true, 10);
        }

        player1Controls[0].enabled = false;
        player1Controls[1].enabled = false;
        player1Controls[player1Box.selected].enabled = true;
        player2Controls[0].enabled = false;
        player2Controls[1].enabled = false;
        player2Controls[player2Box.selected].enabled = true;

        GameManager.MasterVolume = Master.value;
        GameManager.BackGroundVolume = Music.value;
        GameManager.SoundEffectVolume = SoundEffects.value;

        GameManager.player1Controls = player1Box.selected;
        GameManager.player2Controls = player2Box.selected;
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
