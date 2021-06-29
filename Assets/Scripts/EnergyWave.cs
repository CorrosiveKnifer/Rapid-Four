using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyWave : MonoBehaviour
{
    public float damage = 20.0f;
    public float duration = 1.0f;
    public float heal = 50.0f;
    public float knockback = 20.0f;
    private float lifetime = 1.0f;

    List<Collider> hitList = new List<Collider>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (hitList.Contains(other) || lifetime > 0.5)
            return;

       
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponentInParent<EnemyAI>().HurtEnemy(damage);
            other.gameObject.GetComponentInParent<EnemyAI>().StunTarget(duration);
            if (other.gameObject.GetComponentInParent<Rigidbody>())
            {
                Vector3 force = other.gameObject.transform.position - transform.position;
                other.gameObject.GetComponentInParent<Rigidbody>().velocity = force.normalized * knockback;
            }
        }

        if (other.gameObject.GetComponentInParent<PlayerController>() && !hitList.Contains(other))
        {
            // heal player hit
            other.gameObject.GetComponentInParent<PlayerController>().DealHeal(heal);
        }

        if (other.gameObject.GetComponentInParent<Decoy>() && !hitList.Contains(other))
        {
            Vector3 force = other.gameObject.transform.position - transform.position;
            other.gameObject.GetComponentInParent<Rigidbody>().velocity = force.normalized * knockback;
        }

        hitList.Add(other);
    }
}
