using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerUp;

/// <summary>
/// Rachael Colaco, Michael Jordan, William de Beer
/// </summary>
public class HomingShotType : ShotType
{
    public GameObject target;
    public GameObject[] enemies;
    public float range = 10.0f;
    private Vector3 original;
    private float timer = 0.0f;
    private float startingLife;
    public uint playerID = 0;
    protected override void Start()
    {
        //if(!IsLaser)
        //    Instantiate(Resources.Load<GameObject>("VFX/Bullet"), transform);

        isQuitting = false;
        startingLife = lifetime;
    }

    protected override void Update()
    {
        if (lifetime < startingLife - 0.5f)
            homingBullet();

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(this);
        }

        if (timer > 0)
            timer -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Asteroid" && IsLaser)
        {
            if (other.GetComponent<Astroid>().Endurance != 5)
                other.GetComponent<Rigidbody>().AddForce(transform.up * force, ForceMode.Acceleration);

            ////spawning ammo
            //if (Random.Range(0.0f, 100.0f) < probability && timer <= 0.0f)
            //{
            //    GameObject AmmoBox = Instantiate(Resources.Load<GameObject>("Prefabs/PowerUpCube"), other.gameObject.transform.position, Quaternion.identity);
            //    AmmoBox.GetComponent<PowerUpPickUp>().isAmmoDrop = true; //setting the ammodrop to true
            //    AmmoBox.GetComponent<Rigidbody>().AddForce((other.gameObject.transform.position - transform.position).normalized * 5.0f, ForceMode.Acceleration);
            //    AmmoBox.transform.position = new Vector3(AmmoBox.transform.position.x, AmmoBox.transform.position.y, 0.0f);
            //    timer = delay;
            //}
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();

            foreach (var enemy in enemies)
            {
                if (Vector3.Distance(enemy.gameObject.transform.position, transform.position) < range)
                {
                    enemy.HurtEnemy(damage, playerID);
                    Instantiate(Resources.Load<GameObject>("VFX/Detonate"), transform.position, Quaternion.identity);
                }
            }


            if (gameObject.GetComponentInParent<PlayerController>() == null)
            {
                Instantiate(Resources.Load<GameObject>("VFX/Detonate"), transform.position, Quaternion.identity);
                
                Destroy(gameObject);
            }
        }
    }

    void homingBullet()
    {
        //finding all enemies constantly
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        TargetCloset();
        bulletUpdateMovement();
    }

    void bulletUpdateMovement()
    {
        if (target != null)
        {
            if (gameObject.GetComponentInParent<PlayerController>() == null)
            {                
                Vector3 direction = (target.transform.position - transform.position).normalized;
                transform.up = direction;
                GetComponent<Rigidbody>().velocity = direction * 50.0f;
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
            float enemydist = Vector3.Distance(enemy.transform.position, gameObject.transform.position);
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
