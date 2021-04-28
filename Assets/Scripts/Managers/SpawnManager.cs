using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] asteroidPrefab;
    GameObject bossteroid;

    public float m_fSpawnDistance = 160.0f;

    public float m_fSpawnInterval = 10.0f;
    float m_fSpawnTimer;

    public float m_fSpawningGraceDuration = 50.0f;
    float m_fSpawningGrace;
    bool m_bSpawningGrace = false; 

    public float m_fBossSpawnDuration = 120.0f;
    float m_fBossSpawnTimer;
    // bool m_bSpawningGrace = false;

    Animator animDanger;

    float m_fMaxInterval = 10.0f;
    float m_fMinInterval = 0.3f;
    float m_fChangeSpeed = 0.01f; 

    // Start is called before the first frame update
    void Start()
    {
        m_fSpawnTimer = 0.0f;
        m_fSpawningGrace = 0.0f;
        m_fBossSpawnTimer = 0.0f;
        animDanger = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_fSpawnTimer <= 0.0f && !m_bSpawningGrace && bossteroid == null)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f);
            spawnPosition = spawnPosition.normalized * m_fSpawnDistance;

            Instantiate(asteroidPrefab[Random.Range(0, asteroidPrefab.Length)], spawnPosition, Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f)));

            m_fSpawnTimer = m_fSpawnInterval;
        }
        else
        {
            m_fSpawnTimer -= Time.deltaTime;
        }

        m_fSpawnInterval = (m_fMaxInterval / (1 + m_fChangeSpeed * GameManager.instance.TotalScore)) + m_fMinInterval;

        BossSpawnUpdate();

        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnBossteroid();
        }
    }

    private void BossSpawnUpdate()
    {
        if (bossteroid == null)
        {
            m_fBossSpawnTimer += Time.deltaTime;
        }
        Debug.Log(m_fBossSpawnTimer);
        if (!m_bSpawningGrace && (m_fBossSpawnTimer > (m_fBossSpawnDuration - 20.0f)))
        {
            // Activate spawning grace (turns off spawning for the duration)
            m_bSpawningGrace = true;
            m_fSpawningGrace = 0.0f;
            GameManager.instance.WarningText.SetActive(true);
            animDanger.SetTrigger("Start");
            GetComponent<AudioAgent>().PlaySoundEffect("Alarm", true);
        }
        if (m_fBossSpawnTimer >= m_fBossSpawnDuration)
        {
            m_fBossSpawnTimer = 0.0f;
            SpawnBossteroid();
        }

        if (m_bSpawningGrace)
        {
            m_fSpawningGrace += Time.deltaTime;
            if (m_fSpawningGrace > m_fSpawningGraceDuration)
            {
                m_bSpawningGrace = false;
                m_fSpawningGrace = 0.0f;
            }

            if (bossteroid != null)
            {
                GameManager.instance.WarningText.SetActive(false);
                GetComponent<AudioAgent>().StopAudio("Alarm");
                animDanger.SetTrigger("Reset");
            }
        }
    }
    private void SpawnBossteroid()
    {
        if (bossteroid != null)
            return;
        
        Vector3 spawnPosition = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f);
        spawnPosition = spawnPosition.normalized * m_fSpawnDistance;

        bossteroid = Instantiate(asteroidPrefab[Random.Range(0, asteroidPrefab.Length)], spawnPosition, Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f)));

        bossteroid.transform.localScale = new Vector3(3, 3, 3);
        bossteroid.GetComponent<Rigidbody>().mass = Mathf.Pow(2.0f * transform.localScale.x, 3);
        bossteroid.GetComponent<Astroid>().Health = Mathf.Sqrt(bossteroid.GetComponent<Rigidbody>().mass) * 100.0f;

        bossteroid.GetComponent<Astroid>().Endurance = 5;
        bossteroid.GetComponent<Astroid>().maxSpeed = 2.0f;
    }
}
