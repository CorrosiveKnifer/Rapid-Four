using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerUp;

/// <summary>
/// Rachael
/// </summary>
public class FrostShotType : ShotType
{
    private float timer = 0.0f;

    protected override void Start()
    {
        Instantiate(Resources.Load<GameObject>("VFX/FrostBullet"), transform);

        if (gameObject.GetComponentInParent<PlayerController>() != null)
        {
            gameObject.GetComponentInParent<LineRenderer>().enabled = false;
            Instantiate(Resources.Load<GameObject>("Prefabs/BasicFrost"), transform);
        }
    }

    protected override void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Asteroid" && (gameObject.GetComponentInParent<PlayerController>() != null))
        {
            other.GetComponent<Rigidbody>().AddForce(transform.up * force, ForceMode.Acceleration);
            other.gameObject.GetComponent<Astroid>().Slow(30.0f * Time.deltaTime);
            //spawning ammo
            if (Random.Range(0.0f, 100.0f) < probability && timer <= 0.0f)
            {
                GameObject AmmoBox = Instantiate(Resources.Load<GameObject>("Prefabs/PowerUpCube"), other.gameObject.transform.position, Quaternion.identity);
                AmmoBox.GetComponent<PowerUpPickUp>().isAmmoDrop = true; //setting the ammodrop to true
                AmmoBox.GetComponent<Rigidbody>().AddForce((other.gameObject.transform.position - transform.position).normalized * 5.0f, ForceMode.Acceleration);
                AmmoBox.transform.position = new Vector3(AmmoBox.transform.position.x, AmmoBox.transform.position.y, 0.0f);
                timer = delay;
            }
        }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Asteroid")
        {
            other.gameObject.GetComponent<Astroid>().DealDamage(damage);

            if (gameObject.GetComponentInParent<PlayerController>() == null)
            {
                other.gameObject.GetComponent<Astroid>().Slow(3.0f); //3 second
                Destroy(gameObject);
            }
        }
    }
}
