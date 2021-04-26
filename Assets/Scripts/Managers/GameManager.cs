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
    }

    #endregion

    //Volume Settings
    public static float MasterVolume { get; set; } = 1.0f;
    public static float SoundEffectVolume { get; set; } = 1.0f;
    public static float BackGroundVolume { get; set; } = 1.0f;

    public int[] Score;
    public int TotalScore;

    public int AsteroidDestroyScore = 10;

    public double GameTime = 0.0;

    [Header("UI Objects")]
    public Text scoreText;
    public Image planetHealth;

    public RespawnTimer respawnTimer;

    private void Start()
    {
        Score = new int[2];
        Score[0] = 0;
        Score[1] = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GameTime += Time.deltaTime;
        //TotalScore = Score[0] + Score[1];

        scoreText.text = TotalScore.ToString();
    }

    public void AddToScore(float _asteroidScale)
    {
        TotalScore += (int)(AsteroidDestroyScore * _asteroidScale);
    }
    public void SetPlanetHealthBar(float _health)
    {
        planetHealth.fillAmount = _health;
    }

    public void PlayPowerUp()
    {
        GetComponent<AudioAgent>().PlaySoundEffect("PowerUp");
    }

    public RespawnTimer GetRespawnTimer()
    {
        return respawnTimer;
    }
}

