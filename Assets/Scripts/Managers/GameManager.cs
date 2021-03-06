using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

/// <summary>
/// Michael Jordan, William de Beer, Rachael Colaco
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager instance = null;

    public static GameManager GetInstance()
    {
        if (instance == null)
        {
            GameObject loader = new GameObject();
            instance = loader.AddComponent<GameManager>();
            return loader.GetComponent<GameManager>();

        }

        return instance;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
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
    public static float HighScore { get; set; } = 0.0f;

    public static int player1Controls = 0;
    public static int player2Controls = 1;

    public static int[] Score { get; set; } = new int[2];
    public int TotalScore;

    public int AsteroidDestroyScore = 10;

    public bool DefaultSettings = false;

    public static float GameTime { get; set; } = 0.0f;

    public static uint[] Kills { get; set; } = new uint[2];
    public static uint[] Deaths { get; set; } = new uint[2];
    public static uint CurrentWave { get; set; } = 0;
    public static float PlanetHp { get; set; } = 1.0f;
    public static bool HasWon { get; set; } = false;

    [Header("Ship prefabs")]
    [ReadOnly]
    public GameObject[] playerShipPrefabs;

    private List<PlayerController> players;
    public int GetCombinedScore()
    {
        return Score[0] + Score[1];
    }

    private void Start()
    {
        playerShipPrefabs = Resources.LoadAll("PlayerShips", typeof(GameObject)).Cast<GameObject>().ToArray();

        if(DefaultSettings)
        {
            Debug.LogWarning("Using settings: default controllers");
            InputManager.GetInstance().DefaultAssignControllers();
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameTime += Time.deltaTime;
        TotalScore = Score[0] + Score[1];
    }

    private void RestartScores()
    {
        HasWon = false;
        CurrentWave = 0;
        PlanetHp = 1.0f;
        for (int i = 0; i < 2; i++)
        {
            Score[i] = 0;
            Kills[i] = 0;
            Deaths[i] = 0;
        }
    }

    public void SpawnPlayers()
    {
        RestartScores();

        Vector3 pos1 = new Vector3(-80, -3, 0);
        Vector3 pos2 = new Vector3(80, -3, 0);

        int shipId1 = InputManager.GetInstance().GetPlayerControl(0).shipID;
        int shipId2 = InputManager.GetInstance().GetPlayerControl(1).shipID;

        GameObject ship1 = Instantiate(playerShipPrefabs[shipId1], pos1, Quaternion.Euler(new Vector3(-90, 0, 0)));
        CameraManager.instance.SetCameraFocus(0, ship1);
        ship1.GetComponent<PlayerController>().ID = 0;

        GameObject ship2 = Instantiate(playerShipPrefabs[shipId2], pos2, Quaternion.Euler(new Vector3(-90, 0, 0)));
        CameraManager.instance.SetCameraFocus(1, ship2);
        ship2.GetComponent<PlayerController>().ID = 1;

        players = new List<PlayerController>();
        players.Add(ship1.GetComponent<PlayerController>());
        players.Add(ship2.GetComponent<PlayerController>());
    }

    public void AddToScore(float _asteroidScale)
    {
        TotalScore += (int)(AsteroidDestroyScore * _asteroidScale);
    }

    public float ClosestPlayerDistance(Vector3 testPosition)
    {
        float dist = 10000;
        foreach (var player in players)
        {
            dist = Mathf.Min(Vector3.Distance(player.gameObject.transform.position, testPosition), dist);
        }
        return dist;
    }
}

