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
    public int currentWave = 0;

    public bool isSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0 && m_waves.Length > currentWave + 1 && !isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnWave(m_waves[currentWave++]));
        }
        else if(GameObject.FindGameObjectsWithTag("Enemy").Length <= 0 && currentWave >= m_waves.Length)
        {
            FindObjectOfType<LevelLoader>().LoadNextLevel();
            Destroy(this);
        }
    }

    public IEnumerator SpawnWave(Wave wave)
    {
        float time = 0.0f;

        while (time < 2.0f)
        {
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }

        GetComponent<AudioAgent>().PlaySoundEffect("EnemyWaveStart");
        uint shipCount = 0;
        for (int i = 0; i < wave.enemyInfo.Length; i++)
        {
            shipCount += wave.enemyInfo[i];
        }

        while(shipCount > 0)
        {
            for (int i = 0; i < wave.enemyInfo.Length; i++)
            {
                if(wave.enemyInfo[i] > 0)
                {
                    Vector2 temp = new Vector2(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f)).normalized;
                    Instantiate(m_enemyPrefab[i], temp * spawnRadius, Quaternion.identity);
                    wave.enemyInfo[i]--;
                    shipCount--;
                }
            }
        }
        isSpawning = false;
        yield return null;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(Vector3.zero, spawnRadius);
    }
}
