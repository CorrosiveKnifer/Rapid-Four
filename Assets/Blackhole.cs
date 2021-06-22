using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole : MonoBehaviour
{
    public float lifetime = 10.0f;
    public float pullPower = 10.0f;

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
        if (other.gameObject.tag == "Enemy")
        {
            if (!other.gameObject.GetComponent<EnemyAI>())
                return;

            other.gameObject.GetComponent<EnemyAI>().StunTarget(0.1f);
            if (other.gameObject.GetComponent<Rigidbody>())
            {
                Vector3 force = transform.position - other.gameObject.transform.position;
                other.gameObject.GetComponent<Rigidbody>().velocity = force * pullPower;
            }
        }
    }

}
