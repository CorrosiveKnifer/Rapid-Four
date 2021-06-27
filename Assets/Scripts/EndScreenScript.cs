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

    public Text Highscore;
    public Text Score;

    public Text GameTime;

    public Text Player1Kills;
    public Text Player2Kills;
    public Text Player1Deaths;
    public Text Player2Deaths;

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
}
