using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// William de Beer
/// </summary>
public class Decoy : MonoBehaviour
{
    public float lifetime = 10.0f;
    public float explosionRange = 10.0f;
    public float damage = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();

        GetComponent<AudioAgent>().PlaySoundEffect("DecoySonar");

        foreach (var enemy in enemies)
        {
            enemy.SetTarget(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            ActivateDecoy();
        }
    }

    public void ActivateDecoy()
    {
        EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();

        foreach (var enemy in enemies)
        {
            if (Vector3.Distance(enemy.gameObject.transform.position, transform.position) < explosionRange)
            {
                enemy.HurtEnemy(damage);
            }
        }

        Instantiate(Resources.Load<GameObject>("VFX/Detonate"), transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
