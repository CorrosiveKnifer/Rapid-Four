﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// William de Beer, Rachael Colaco
/// </summary>
public class EndScreenScript : MonoBehaviour
{
    public static float ScoreToDisplay = 0f;

    public Text Highscore;
    public Text Score;

    public Text GameTime;

    public Text Player1Kills;
    public Text Player2Kills;
    public Text Player1Deaths;
    public Text Player2Deaths;

    public GameObject GoBackButton;

    // Start is called before the first frame update
    void Start()
    {
        ApplicationManager.GetInstance();
        GetComponent<AudioAgent>().PlayBackground("EndMusic", true, 10);
        Score.text = ScoreToDisplay.ToString();
        Highscore.text = GameManager.HighScore.ToString();

        GameTime.text = GameManager.GameTime.ToString();
        Player1Kills.text = GameManager.GameTime.ToString();
        Player2Kills.text = GameManager.GameTime.ToString();
        Player1Deaths.text = GameManager.GameTime.ToString();
        Player2Deaths.text = GameManager.GameTime.ToString();

    }
    void Update()
    {
        FadeToColor(GoBackButton.GetComponent<Button>().colors.highlightedColor, GoBackButton);
        EndingMechanic();

    }

    void FadeToColor(Color color, GameObject currentButton)
    {
        Graphic graphic = currentButton.GetComponent<Graphic>();
        Button button = currentButton.GetComponent<Button>();
        if (graphic != null)
            graphic.CrossFadeColor(color, button.colors.fadeDuration, true, true);
    }

    void EndingMechanic()
    {
        for (int i = 0; i < 2; i++)
        {
            if (InputManager.GetInstance().GetKeyDown(InputManager.ButtonType.BUTTON_SOUTH, i))
            {
                Debug.Log("press");
                PlaySoundEffect();
                GoBackButton.GetComponent<Button>().onClick.Invoke();

                break;
            }
        }
    }

    public void PlaySoundEffect()
    {
        GetComponent<AudioAgent>().PlaySoundEffect("ShootPew");
    }
}
