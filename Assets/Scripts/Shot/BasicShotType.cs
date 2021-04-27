using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerUp;

public class BasicShotType : ShotType
{
    private float probability = 50.0f;

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
