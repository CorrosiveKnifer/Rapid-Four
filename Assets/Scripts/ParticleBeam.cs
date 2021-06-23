using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBeam : MonoBehaviour
{
    public float damage = 100.0f;
    private float lifetime = 1.5f;

    List<Collider> hitList = new List<Collider>();

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<Animator>().SetTrigger("Fire");
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        Debug.Log(lifetime);
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (hitList.Contains(other) || other.gameObject == null)
            return;

        if (lifetime <= 1.5f - 1.05)
        {
            if (other.gameObject.tag == "Enemy" && !hitList.Contains(other) && other.gameObject.GetComponentInParent<EnemyAI>())
            {
                other.gameObject.GetComponentInParent<EnemyAI>().HurtEnemy(damage);
            }
            if (other.gameObject != null)
                hitList.Add(other);
        }
    }
}
