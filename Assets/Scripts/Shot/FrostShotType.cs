using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerUp;

/// <summary>
/// Rachael
/// </summary>
public class FrostShotType : ShotType
{
    private float probability = 50.0f;

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
            other.gameObject.GetComponent<Astroid>().Slow(30.0f * Time.deltaTime);
        }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Asteroid")
        {
            other.gameObject.GetComponent<Astroid>().DealDamage(damage);

            if (!IsLaser)
            {
                other.gameObject.GetComponent<Astroid>().Slow(3.0f); //3 second
                Destroy(gameObject);
            }
            else
            {
                //spawning ammo
                if (Random.Range(0.0f, 100.0f) < probability)
                {
                    GameObject AmmoBox = Instantiate(Resources.Load<GameObject>("Prefabs/PowerUpCube"), other.gameObject.transform.position, Quaternion.identity);
                    AmmoBox.GetComponent<PowerUpPickUp>().isAmmoDrop = true; //setting the ammodrop to true

                }

            }
        }
    }
}
