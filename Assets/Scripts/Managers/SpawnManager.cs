using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject asteroidPrefab;

    public float m_fSpawnDistance = 160.0f;

    public float m_fSpawnInterval = 10.0f;
    public float m_fSpawnTimer;

    float m_fMaxInterval = 10.0f;
    float m_fMinInterval = 0.3f;
    float m_fChangeSpeed = 0.01f; 

    // Start is called before the first frame update
    void Start()
    {
        m_fSpawnTimer = m_fMaxInterval;        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_fSpawnTimer <= 0.0f)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f);
            spawnPosition = spawnPosition.normalized * m_fSpawnDistance;

            Instantiate(asteroidPrefab, spawnPosition, Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f)));

            m_fSpawnTimer = m_fSpawnInterval;
        }
        else
        {
            m_fSpawnTimer -= Time.deltaTime;
        }

        m_fSpawnInterval = (m_fMaxInterval / (1 + m_fChangeSpeed * GameManager.instance.TotalScore)) + m_fMinInterval;
        Debug.Log(m_fSpawnInterval);
    }
}
