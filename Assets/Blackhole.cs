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
        GetComponent<AudioAgent>().PlaySoundEffect("Blackhole");
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
            if (!other.gameObject.GetComponentInParent<EnemyAI>())
                return;

            other.gameObject.GetComponentInParent<EnemyAI>().StunTarget(0.1f);
            if (other.gameObject.GetComponentInParent<Rigidbody>())
            {
                Vector3 force = transform.position - other.gameObject.transform.position;
                other.gameObject.GetComponentInParent<Rigidbody>().velocity = force * pullPower;
            }
        }
    }

}
