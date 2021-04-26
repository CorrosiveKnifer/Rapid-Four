using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerUp;

/// <summary>
/// Rachael
/// </summary>
public class PierceShotType : ShotType
{
    int endurance = 1;
    // Start is called before the first frame update
    private float force = 100.0f;

    protected override void Start()
    {

    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Asteroid")
        {
            other.gameObject.GetComponent<Astroid>().DealDamage(damage);

            if (!IsLaser)
            {
                if (endurance == -1)
                {
                    Destroy(gameObject);
                }
                endurance--;
            }
            else
            {

                other.GetComponent<Rigidbody>().AddForce(transform.up * force, ForceMode.Acceleration);
            }
        }
    }
}
