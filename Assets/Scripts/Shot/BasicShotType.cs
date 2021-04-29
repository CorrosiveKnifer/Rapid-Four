using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerUp;

public class BasicShotType : ShotType
{
    private float timer = 0.0f;

    protected override void Start()
    {
        if (!IsLaser)
            Instantiate(Resources.Load<GameObject>("VFX/Bullet"), transform);
    }

    protected override void Update()
    {
        if(timer > 0)
            timer -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Asteroid" && (gameObject.GetComponentInParent<PlayerController>() != null))
        {
            other.GetComponent<Rigidbody>().AddForce(transform.up * force, ForceMode.Acceleration);
            //spawning ammo
            if (Random.Range(0.0f, 100.0f) < probability && timer <= 0.0f)
            {
                GameObject AmmoBox = Instantiate(Resources.Load<GameObject>("Prefabs/PowerUpCube"), other.gameObject.transform.position, Quaternion.identity);

                AmmoBox.GetComponent<PowerUpPickUp>().isAmmoDrop = !(Random.Range(0.0f, 100.0f) < 15.0f); //setting the ammodrop to true
                AmmoBox.GetComponent<Rigidbody>().AddForce((other.gameObject.transform.position - transform.position).normalized * 5.0f, ForceMode.Acceleration);
                AmmoBox.transform.position = new Vector3(AmmoBox.transform.position.x, AmmoBox.transform.position.y, 0.0f);
                timer = delay;
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Asteroid")
        {
            other.gameObject.GetComponent<Astroid>().DealDamage(damage);

            if((gameObject.GetComponentInParent<PlayerController>() == null))
            {
                Destroy(gameObject);
            }            
        }
    }
}
