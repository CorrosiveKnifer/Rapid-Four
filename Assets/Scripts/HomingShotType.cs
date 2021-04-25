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

    protected override void Start()
    {

    }

    private void Update()
    {

        homingBullet();

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(this);
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
                transform.LookAt(target.transform);
                Vector3 direction = target.transform.position - transform.position;
                GetComponent<Rigidbody>().velocity = direction.normalized * 10.0f;
            }
            else
            {
                transform.up= target.transform.position - transform.position;
                Debug.Log("homing");
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
