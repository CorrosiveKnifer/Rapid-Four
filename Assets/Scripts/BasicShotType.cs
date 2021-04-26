﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerUp;

public class BasicShotType : ShotType
{
    private float force = 100.0f;
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
            else
            {
                
                other.GetComponent<Rigidbody>().AddForce(transform.up * force, ForceMode.Acceleration);
            }
        }
    }
}
