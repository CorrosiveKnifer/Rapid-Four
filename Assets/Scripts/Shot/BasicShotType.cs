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
    //private GameObject pushVFX;

    protected override void Start()
    {
        Instantiate(Resources.Load<GameObject>("VFX/Bullet"), transform);
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

            RaycastHit hit;

            //if(RayCastToFirst(out hit))
            //{
            //    pushVFX.transform.up = -1 * hit.normal;
            //    pushVFX.transform.position = hit.point;
            //    pushVFX.SetActive(true);
            //}
            //else
            //{
            //    pushVFX.SetActive(false);
            //}
        }
    }

    //private bool RayCastToFirst(out RaycastHit closest)
    //{
    //    RaycastHit[] hits = Physics.RaycastAll(transform.position + transform.up, transform.up, transform.localScale.y);
    //    closest = hits[0];
    //    if (hits.Length > 0)
    //    {
    //        foreach (var hit in hits)
    //        {
    //            if(closest.distance > hit.distance)
    //            {
    //                closest = hit;
    //            }
    //        }
    //        return true;
    //    }
    //    
    //    return false;
    //}

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
            other.gameObject.GetComponent<EnemyAI>().HurtEnemy(damage);
            Destroy(gameObject);
        }
    }
}
