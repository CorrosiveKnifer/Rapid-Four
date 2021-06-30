using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// William de Beer, Rachael Colaco
/// </summary>
public class EndScreenScript : MonoBehaviour
{
    public static float ScoreToDisplay = 0f;

    public Text ResultText;

    public Text HP_Planet;

    public Text Wave;

    public Text GameTime;

    public Text Player1Score;
    public Text Player2Score;

    public Text Player1Kills;
    public Text Player2Kills;

    public Text Player1Deaths;
    public Text Player2Deaths;

    public GameObject GoBackButton;

    // Start is called before the first frame update
    void Start()
    {
        ApplicationManager.GetInstance();


        if (GameManager.HasWon)
        {
            ResultText.text = "VICTORY";
            GetComponent<AudioAgent>().PlayBackground("Victory", true, 10);
        }
        else
        {
            ResultText.text = "GAMEOVER";
            GetComponent<AudioAgent>().PlayBackground("GameOver", true, 10);
        }

        int HP = (int)(GameManager.PlanetHp * 100.0f);
        HP_Planet.text = HP.ToString();
        Wave.text = GameManager.CurrentWave.ToString();
        GameTime.text = GameManager.GameTime.ToString();

        Player1Score.text = GameManager.Score[0].ToString();
        Player2Score.text = GameManager.Score[1].ToString();
        Player1Kills.text = GameManager.Kills[0].ToString();
        Player2Kills.text = GameManager.Kills[1].ToString();
        Player1Deaths.text = GameManager.Deaths[0].ToString();
        Player2Deaths.text = GameManager.Deaths[1].ToString();

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

    public void GoBackToMenu()
    {
        GameObject.FindObjectOfType<LevelLoader>().LoadNextLevel();
    }
}
