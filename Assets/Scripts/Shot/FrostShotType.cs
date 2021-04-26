using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerUp;

/// <summary>
/// Rachael
/// </summary>
public class FrostShotType : ShotType
{
    protected override void Start()
    {
        if (!IsLaser)
            Instantiate(Resources.Load<GameObject>("VFX/FrostBullet"), transform);
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
        if (other.gameObject.tag == "Asteroid")
        {
            other.gameObject.GetComponent<Astroid>().DealDamage(damage);
            other.gameObject.GetComponent<Astroid>().maxSpeed *= 0.5f;

            if (!IsLaser)
            {
                Destroy(gameObject);
            }
        }
    }
}
