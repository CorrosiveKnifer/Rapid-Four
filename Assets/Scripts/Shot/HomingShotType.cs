using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerUp;

/// <summary>
/// Rachael
/// </summary>
public class HomingShotType : ShotType
{
    public GameObject target;
    public GameObject[] enemies;
    private Vector3 original;

    private float probability = 35.0f;
    private float timer = 0.0f;
    protected override void Start()
    {
        if(!IsLaser)
            Instantiate(Resources.Load<GameObject>("VFX/Bullet"), transform);
    }

    protected override void Update()
    {
        homingBullet();

        if (timer > 0)
            timer -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Asteroid" && IsLaser)
        {
            other.GetComponent<Rigidbody>().AddForce(transform.up * force, ForceMode.Acceleration);

            //spawning ammo
            if (Random.Range(0.0f, 100.0f) < probability && timer <= 0.0f)
            {
                GameObject AmmoBox = Instantiate(Resources.Load<GameObject>("Prefabs/PowerUpCube"), other.gameObject.transform.position, Quaternion.identity);
                AmmoBox.GetComponent<PowerUpPickUp>().isAmmoDrop = true; //setting the ammodrop to true
                AmmoBox.GetComponent<Rigidbody>().AddForce((other.gameObject.transform.position - transform.position).normalized * 5.0f, ForceMode.Acceleration);
                AmmoBox.transform.position = new Vector3(AmmoBox.transform.position.x, AmmoBox.transform.position.y, 0.0f);
                timer = 1.0f;
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Asteroid")
        {
            other.gameObject.GetComponent<Astroid>().DealDamage(damage);

            if (!IsLaser)
            {
                Destroy(gameObject);
            }
        }
    }

    void homingBullet()
    {
        //finding all enemies constantly
        enemies = GameObject.FindGameObjectsWithTag("Asteroid");
        TargetCloset();
        bulletUpdateMovement();

    }

    void bulletUpdateMovement()
    {
        if (target != null)
        {
            if (!IsLaser)
            {                
                Vector3 direction = (target.transform.position - transform.position).normalized;
                transform.up = direction;
                GetComponent<Rigidbody>().velocity = direction * 10.0f;
            }
            else
            {
                transform.up = (target.transform.position - transform.position).normalized;
            }
            
        }
        else
        {
            if (IsLaser)
            {
                transform.up = transform.parent.up;
            }
        }
    }

    private void TargetCloset()
    {
        target = null;
        float tempRadius = 50.0f;
        //checking each enemy
        foreach (GameObject enemy in enemies)
        {
            //calculate distance
            float enemydist = Vector3.Distance(enemy.transform.position, transform.position);
            //if its in tower range
            if (enemydist < tempRadius)
            {
                //marking this as the closet enemy
                tempRadius = enemydist;
                target = enemy;

            }

        }

    }
}
