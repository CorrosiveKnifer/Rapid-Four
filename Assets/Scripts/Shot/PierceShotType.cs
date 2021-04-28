using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerUp;

/// <summary>
/// Rachael Colaco, Michael Jordan
/// </summary>
public class PierceShotType : ShotType
{
    int endurance = 1;
    private float timer = 0.0f;
    // Start is called before the first frame update

    protected override void Start()
    {
        if (gameObject.GetComponentInParent<PlayerController>() != null)
        {
            transform.localScale = new Vector3(1.5f, 15.0f, 1.0f);
        }
        else
        {
            Instantiate(Resources.Load<GameObject>("VFX/Bullet"), transform);
        }

        isQuitting = false;
    }
    protected override void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Asteroid" && gameObject.GetComponentInParent<PlayerController>() != null)
        {
            if (other.GetComponent<Astroid>().Endurance != 5)
                other.GetComponent<Rigidbody>().AddForce(transform.up * force, ForceMode.Acceleration);
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
                if (endurance == -1)
                {
                    Instantiate(Resources.Load<GameObject>("VFX/RockHit"), transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                endurance--;
            }
        }
    }
}
