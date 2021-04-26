﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerUp;
/// <summary>
/// Rachael
/// </summary>
public class FrostShotType : ShotType
{
    private float force = 100.0f;
    protected override void Start()
    {

    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Asteroid")
        {
            other.gameObject.GetComponent<Astroid>().DealDamage(damage);
            other.gameObject.GetComponent<Astroid>().maxSpeed *= 0.5f;
            if (!IsLaser)
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
