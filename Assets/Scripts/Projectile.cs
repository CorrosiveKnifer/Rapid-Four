using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public float m_damage = 0.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Send damage information to other
        collision.gameObject.GetComponent<PlayerController>()?.DealDamage(m_damage);
        Destroy(gameObject);
    }
}
