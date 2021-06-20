using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// William de Beer
/// </summary>

[System.Serializable]
public class Wave
{
    public uint[] enemyInfo;
}

public class SpawnManager : MonoBehaviour
{
    public GameObject[] m_enemyPrefab;
    
    public Wave[] m_waves;

    public float spawnRadius = 200;
    //GameObject bossteroid;

    //public float m_fSpawnDistance = 160.0f;

    //public float m_fSpawnInterval = 10.0f;
    //float m_fSpawnTimer;

    //bool m_bWarning = false;

    //public float m_fSpawningGraceDuration = 50.0f;
    //float m_fSpawningGrace;
    //bool m_bSpawningGrace = false; 

    //public float m_fBossSpawnDuration = 120.0f;
    //float m_fBossSpawnTimer;
    // bool m_bSpawningGrace = false;

    //Animator animDanger;

    //float m_fMaxInterval = 10.0f;
    //float m_fMinInterval = 0.3f;
    //float m_fChangeSpeed = 0.01f; 

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector3.zero;
        //m_fSpawnTimer = 0.0f;
        //m_fSpawningGrace = 0.0f;
        //m_fBossSpawnTimer = 0.0f;
        //animDanger = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (m_fSpawnTimer <= 0.0f && !m_bSpawningGrace && bossteroid == null)
        //{
        //    Vector3 spawnPosition = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f);
        //    spawnPosition = spawnPosition.normalized * m_fSpawnDistance;
        //
        //    Instantiate(asteroidPrefab[Random.Range(0, asteroidPrefab.Length)], spawnPosition, Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f)));
        //
        //    m_fSpawnTimer = m_fSpawnInterval;
        //}
        //else
        //{
        //    m_fSpawnTimer -= Time.deltaTime;
        //}
        //
        //m_fSpawnInterval = (m_fMaxInterval / (1 + m_fChangeSpeed * GameManager.instance.TotalScore)) + m_fMinInterval;
        //
        //BossSpawnUpdate();
        /*
        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnBossteroid();
        }
        */
    }

    public void SpawnWave(Wave wave)
    {
        uint shipCount = 0;
        for (int i = 0; i < wave.enemyInfo.Length; i++)
        {
            shipCount += wave.enemyInfo[i];
        }

        while(shipCount > 0)
        {

        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(Vector3.zero, spawnRadius);
    }
}
