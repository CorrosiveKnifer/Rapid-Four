using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Michael Jordan, William de Beer
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Second Instance of GameManager was created, this instance was destroyed.");
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;

        EndScreenScript.ScoreToDisplay = TotalScore;
    }

    #endregion

    //Volume Settings
    public static float MasterVolume { get; set; } = 1.0f;
    public static float SoundEffectVolume { get; set; } = 1.0f;
    public static float BackGroundVolume { get; set; } = 1.0f;
    public static float HighScore { get; set; } = 0.0f;

    public int[] Score; // Not used anyore
    public int TotalScore;

    public int AsteroidDestroyScore = 10;

    public double GameTime = 0.0;

    [Header("UI Objects")]
    public GameObject ObjectiveText;
    float ObjectiveTextDecayTime = 10.0f;

    public Text scoreText;
    public Text player1Ammo;
    public Text player2Ammo;
    public Image planetHealth;

    public RespawnTimer respawnTimer;

    private PlayerController player1;
    private PlayerController player2;

    private void Start()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Asteroid"), LayerMask.NameToLayer("PowerUp")); 

        Score = new int[2];
        Score[0] = 0;
        Score[1] = 0;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            if (player.GetComponentInParent<PlayerController>()?.ID == 0)
            {
                player1 = player.GetComponentInParent<PlayerController>();
            }
            else if(player.GetComponentInParent<PlayerController>()?.ID == 1)
            {
                player2 = player.GetComponentInParent<PlayerController>();
            }
        }

        GetComponent<AudioAgent>().PlayBackground("InGameMusic", true, 10);
    }

    // Update is called once per frame
    void Update()
    {
        GameTime += Time.deltaTime;
        //TotalScore = Score[0] + Score[1];

        scoreText.text = TotalScore.ToString();

        player1Ammo.text = player1.Ammo.ToString();
        player2Ammo.text = player2.Ammo.ToString();

        if(TotalScore > HighScore)
        {
            HighScore = TotalScore;
            scoreText.color = new Color(255/255f * 0.8f,215/255f * 0.8f,0);
        }

        if (GameTime < ObjectiveTextDecayTime)
        {
            ObjectiveText.GetComponentInChildren<Image>().color = new Color(1, 1, 1, ((ObjectiveTextDecayTime - (float)GameTime) / ObjectiveTextDecayTime));
            ObjectiveText.GetComponentInChildren<Text>().color = new Color(1, 1, 1, ((ObjectiveTextDecayTime - (float)GameTime) / ObjectiveTextDecayTime));
        }
        else
        {
            ObjectiveText.SetActive(false);
        }
    }
    public void AddToScore(float _asteroidScale)
    {
        TotalScore += (int)(AsteroidDestroyScore * _asteroidScale);
    }
    public void SetPlanetHealthBar(float _health)
    {
        planetHealth.fillAmount = _health;
    }

    public RespawnTimer GetRespawnTimer()
    {
        return respawnTimer;
    }
}

