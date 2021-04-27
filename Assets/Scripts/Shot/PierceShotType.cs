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
    private float probability = 50.0f;
    // Start is called before the first frame update

    protected override void Start()
    {
        if(IsLaser)
        {
            transform.localScale = new Vector3(1.5f, 15.0f, 1.0f);
        }
        else
        {
            Instantiate(Resources.Load<GameObject>("VFX/Bullet"), transform);
        }
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
