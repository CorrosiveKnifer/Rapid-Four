using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerUp;

/// <summary>
/// Michael Jordan, Rachael Colaco
/// </summary>
public class BasicShotType : ShotType
{
    private float timer = 0.0f;
    public uint playerID = 0;
    //private GameObject pushVFX;

    protected override void Start()
    {
        //Instantiate(Resources.Load<GameObject>("VFX/Bullet"), transform);
        //if (gameObject.GetComponentInParent<PlayerController>() != null)
        //{
        //    pushVFX = Instantiate(Resources.Load<GameObject>("VFX/Push"), transform);
        //    pushVFX.SetActive(false);
        //}
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
            if (other.GetComponent<Astroid>().Endurance != 5)
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
                Instantiate(Resources.Load<GameObject>("VFX/RockHit"), transform.position, Quaternion.identity);
                Destroy(gameObject);
            }            
        }
        if(other.gameObject.tag == "Enemy")
        {
            Instantiate(Resources.Load<GameObject>("VFX/RockHit"), transform.position, Quaternion.identity);
            other.gameObject.GetComponentInParent<EnemyAI>().StunTarget(0.16f);
            other.gameObject.GetComponentInParent<EnemyAI>().HurtEnemy(damage, playerID);
            Destroy(gameObject);
        }
    }
}
