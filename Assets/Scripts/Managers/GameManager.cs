using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

/// <summary>
/// Michael Jordan, William de Beer
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

    public int[] Score; // Not used anyore
    public int TotalScore;

    public int AsteroidDestroyScore = 10;

    public double GameTime = 0.0;

    [Header("Ship prefabs")]
    [ReadOnly]
    public GameObject[] playerShipPrefabs;

    public int GetCombinedScore()
    {
        return 0;
    }

    private void Start()
    {
        playerShipPrefabs = Resources.LoadAll("PlayerShips", typeof(GameObject)).Cast<GameObject>().ToArray();

        //If on game scene Default:
        if(!SceneManager.GetActiveScene().name.Contains("Lobby"))
        {
            InputManager.GetInstance().DefaultAssignControllers();
        }

        SpawnPlayers();
    }

    // Update is called once per frame
    void Update()
    {
        GameTime += Time.deltaTime;
    }

    public void SpawnPlayers()
    {
        Vector3 pos1 = new Vector3(-80, -3, 0);
        Vector3 pos2 = new Vector3(80, -3, 0);

        int shipId1 = InputManager.GetInstance().GetPlayerControl(0).shipID;
        int shipId2 = InputManager.GetInstance().GetPlayerControl(1).shipID;

        GameObject ship1 = Instantiate(playerShipPrefabs[shipId1], pos1, Quaternion.Euler(new Vector3(-90, 0, 0)));
        CameraManager.instance.SetCameraFocus(0, ship1);
        ship1.GetComponent<PlayerController>().ID = 0;

        GameObject ship2 = Instantiate(playerShipPrefabs[shipId2], pos2, Quaternion.Euler(new Vector3(-90, 0, 0)));
        CameraManager.instance.SetCameraFocus(1, ship2);
        ship2.GetComponent<PlayerController>().ID = 0;

        //CameraManager.instance.SetCameraFocus(1, Instantiate(playerShipPrefabs[shipId2], pos1, Quaternion.Euler(new Vector3(-90, 0, 0))));
        //Instantiate(playerShipPrefabs[shipId2], pos2, Quaternion.identity);
    }

    public void AddToScore(float _asteroidScale)
    {
        TotalScore += (int)(AsteroidDestroyScore * _asteroidScale);
    }
}

