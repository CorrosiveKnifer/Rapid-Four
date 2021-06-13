using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBeam : MonoBehaviour
{
    public float damage = 100.0f;
    private float lifetime = 0.5f;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Astroid>())
            other.gameObject.GetComponent<Astroid>().DealDamage(damage);
    }
}
