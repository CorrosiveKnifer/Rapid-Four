using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerUp;

public class BasicShotType : ShotType
{
    protected override void Start()
    {
        
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
