using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Michael Jordan
/// </summary>
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
    public GameObject[] Buttons;
    public GameObject GoBackButton;
    private int index = 0;
    private bool IsMenuScreen = true;

    // Start is called before the first frame update
    void Start()
    {
        ApplicationManager.GetInstance();

        Master.value = GameManager.MasterVolume;
        Music.value = GameManager.BackGroundVolume;
        SoundEffects.value = GameManager.SoundEffectVolume;

        isIgnore = false;
        ShowTitle();
        
        FadeToColor(Buttons[index].GetComponent<Button>().colors.highlightedColor, Buttons[index]);
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<AudioAgent>().IsAudioStopped("StartScreen"))
        {
            GetComponent<AudioAgent>().PlayBackground("StartScreen", true, 10);
        }


        FadeToColor(Buttons[index].GetComponent<Button>().colors.highlightedColor, Buttons[index]);
        FadeToColor(GoBackButton.GetComponent<Button>().colors.highlightedColor, GoBackButton);
        if(IsMenuScreen)
        {
            MenuMechanic();
        }
        else
        {
            SettingMechanic();
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
    void FadeToColor(Color color, GameObject currentButton)
    {
        Graphic graphic = currentButton.GetComponent<Graphic>();
        Button button = currentButton.GetComponent<Button>();
        if (graphic != null)
            graphic.CrossFadeColor(color, button.colors.fadeDuration, true, true);
    }

    void MenuMechanic()
    {
        for (int i = 0; i < 2; i++)
        {

            if (InputManager.GetInstance().GetStickDirection(InputManager.StickDirection.RIGHT, i))
            {
                Debug.Log("right");
                FadeToColor(Buttons[index].GetComponent<Button>().colors.normalColor, Buttons[index]);
                index = Mathf.Clamp(index + 1, 0, 2);
                break;

            }
            if (InputManager.GetInstance().GetStickDirection(InputManager.StickDirection.LEFT, i))
            {
                Debug.Log("left");

                FadeToColor(Buttons[index].GetComponent<Button>().colors.normalColor, Buttons[index]);
                index = Mathf.Clamp(index - 1, 0, 2);
                break;

            }

            if (InputManager.GetInstance().GetKeyDown(InputManager.ButtonType.BUTTON_SOUTH, i))
            {
                Debug.Log("press");

                Buttons[index].GetComponent<Button>().onClick.Invoke();

                break;
            }
        }
    }
    void SettingMechanic()
    {
        for (int i = 0; i < 2; i++)
        {
            if (InputManager.GetInstance().GetKeyDown(InputManager.ButtonType.BUTTON_SOUTH, i))
            {
                Debug.Log("press");

                GoBackButton.GetComponent<Button>().onClick.Invoke();

                break;
            }
        }
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
        IsMenuScreen = true;
        Menu.SetActive(true);
        Settings.SetActive(false);
    }

    public void ShowSettings()
    {
        IsMenuScreen = false;
        Menu.SetActive(false);
        Settings.SetActive(true);
    }
}
