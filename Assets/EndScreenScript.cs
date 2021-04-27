using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenScript : MonoBehaviour
{
    public static float ScoreToDisplay = 0f;

    public Text Highscore;
    public Text Score;

    // Start is called before the first frame update
    void Start()
    {
        ApplicationManager.GetInstance();

        Score.text = ScoreToDisplay.ToString();
        Highscore.text = GameManager.HighScore.ToString();
    }
}
