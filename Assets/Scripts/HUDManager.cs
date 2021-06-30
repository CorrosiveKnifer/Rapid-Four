using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    #region Singleton

    public static HUDManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Second Instance of HUDManager was created, this instance was destroyed.");
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    #endregion

    public double GameTime = 0.0;

    [Header("UI Objects")]
    public GameObject WarningText;
    public GameObject ObjectiveText;
    float ObjectiveTextDecayTime = 10.0f;

    public Text scoreText;
    public Text player1Ammo;
    public Text player2Ammo;
    public Image planetHealth;
    
    public RespawnTimer respawnTimer;

    public HealthBar[] playerBars;

    public Image[] player1Ability;
    public Image[] player2Ability;

    private PlayerController player1;
    private PlayerController player2;

    private void Start()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Asteroid"), LayerMask.NameToLayer("PowerUp"));

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            if (player.GetComponentInParent<PlayerController>()?.ID == 0)
            {
                player1 = player.GetComponentInParent<PlayerController>();
            }
            else if (player.GetComponentInParent<PlayerController>()?.ID == 1)
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

        scoreText.text = GameManager.GetInstance().GetCombinedScore().ToString();

        //player1Ammo.text = player1.Ammo.ToString();
        //player2Ammo.text = player2.Ammo.ToString();


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

    public RespawnTimer GetRespawnTimer()
    {
        return respawnTimer;
    }

    public void SetHealthDisplay(int id, float currentHealth, float currentShield, float currentOverheat = 0)
    {
        playerBars[id].healthCurrent = currentHealth;
        playerBars[id].shieldCurrent = currentShield;
        playerBars[id].overheatCurrent = currentOverheat;
    }

    public void SetHealthMax(int id, float maxHealth, float maxShield)
    {
        playerBars[id].healthMax = maxHealth;
        playerBars[id].shieldMax = maxShield;
    }

    public void SetAbilityUI(int id, Sprite Alt, Sprite Utility, Sprite Ult)
    {
        switch (id)
        {
            default:
            case 0:
                player1Ability[0].sprite = Alt;
                player1Ability[1].sprite = Utility;
                player1Ability[2].sprite = Ult;
                break;
            case 1:
                player2Ability[0].sprite = Alt;
                player2Ability[1].sprite = Utility;
                player2Ability[2].sprite = Ult;
                break;
        }
    }

    public void SetPlanetHP(float m_max, float m_current)
    {
        planetHealth.fillAmount = m_current / m_max;
        planetHealth.color = Color.Lerp(Color.green, Color.red, 1.0f - m_current / m_max);
    }
}
