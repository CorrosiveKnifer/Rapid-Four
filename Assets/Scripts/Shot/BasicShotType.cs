using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerUp;

public class BasicShotType : ShotType
{
    
    protected override void Start()
    {
        if (!IsLaser)
            Instantiate(Resources.Load<GameObject>("VFX/Bullet"), transform);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Asteroid" && IsLaser)
        {
            other.GetComponent<Rigidbody>().AddForce(transform.up * force, ForceMode.Acceleration);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Asteroid")
        {
            other.gameObject.GetComponent<Astroid>().DealDamage(damage);

            if(!IsLaser)
            {
                Destroy(gameObject);
            }
            
        }
    }
}
