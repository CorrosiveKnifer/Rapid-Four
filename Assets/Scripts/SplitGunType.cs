using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerUp;

/// <summary>
/// RACHAEL
/// </summary>
public class SplitGunType : GunType
{
    private GameObject proj;
    private GameObject laser;
    private float force = 50.0f;
    private float damage = 30.0f;

    private int playerID;
    private GameObject laserObject;
    private GameObject laser1;
    private GameObject laser2;
    public void Start()
    {
        proj = Resources.Load<GameObject>("Prefabs/BasicShot");
        laser = Resources.Load<GameObject>("Prefabs/BasicLaser");

        playerID = GetComponentInParent<PlayerController>().ID;
    }
    protected void OnDestroy()
    {
        if (laserObject != null)
            Destroy(laserObject);
        if (laser1 != null)
            Destroy(laser1);
        if (laser2 != null)
            Destroy(laser2);
    }
    public override void Fire(ShotType type)
    {
        switch (playerID)
        {
            default:
            case 0: // ship

                 SpawnChild(type);
                break;

            case 1: //Sucker Ship
                if (laserObject == null)
                {
                    //Create Laser, which is parented by us
                    laserObject = Instantiate(laser, transform) as GameObject;
                    laserObject.AddComponent(type.GetType());
                    laserObject.transform.localScale = new Vector3(1.5f, 10.0f, 1.0f);
                    laserObject.transform.up = transform.up;
                    laserObject.GetComponent<ShotType>().damage = damage * Time.deltaTime;
                    laserObject.GetComponent<ShotType>().IsLaser = true;
                    SpawnLaserChild(type);

                }
                break;
        }
    }

    public override void UnFire()
    {
        if (laserObject != null)
            Destroy(laserObject);
        if (laser1 != null)
            Destroy(laser1);
        if (laser2 != null)
            Destroy(laser2);
    }

    void SpawnLaserChild(ShotType type)
    {
        //Set thoseponteial directions
        Vector3 FirstDir = Quaternion.AngleAxis(45, Vector3.forward) * transform.up;
        Vector3 SecondDir = Quaternion.AngleAxis(-45, Vector3.forward) * transform.up;

        //first laser from the left---------------------------------------

        //Create laser 1
        laser1 = Instantiate(laser, transform) as GameObject;
        laser1.AddComponent(type.GetType());
        laser1.transform.localScale = new Vector3(1.5f, 10.0f, 1.0f);
        laser1.GetComponent<ShotType>().damage = damage * Time.deltaTime;

        //set direction to the left
        laser1.transform.up = FirstDir;
        laser1.GetComponent<ShotType>().IsLaser = true;


        //second laser---------------------------------------

        //Create laser 2
        laser2 = Instantiate(laser, transform) as GameObject;
        laser2.AddComponent(type.GetType());
        laser2.transform.localScale = new Vector3(1.5f, 10.0f, 1.0f);
        laser2.GetComponent<ShotType>().damage = damage * Time.deltaTime;

        //set direction to the right
        laser2.transform.up = SecondDir;
        laser2.GetComponent<ShotType>().IsLaser = true;

    }
    void SpawnChild(ShotType type)
    {
        //Set thoseponteial directions
        Vector3 FirstDir = Quaternion.AngleAxis(15, Vector3.forward) * transform.up;
        Vector3 SecondDir = Quaternion.AngleAxis(-15, Vector3.forward) * transform.up;

        //first bullet---------------------------------------

        //Create projectile
        GameObject bullet1 = Instantiate(proj, transform.position, Quaternion.AngleAxis(45, Vector3.forward));
        bullet1.AddComponent(type.GetType());
        bullet1.transform.up = FirstDir;

        //left direction
        Vector3 interpolatedPosition = Vector3.Lerp(FirstDir, SecondDir, 0);
        bullet1.GetComponent<ShotType>().damage = damage;
        //set force
        bullet1.GetComponent<Rigidbody>().AddForce(FirstDir * force, ForceMode.Impulse);


        //second bullet---------------------------------------

        //Create projectile
        GameObject bullet2 = Instantiate(proj, transform.position, Quaternion.AngleAxis(0, Vector3.forward));
        bullet2.AddComponent(type.GetType());
        bullet2.transform.up = transform.up;

        //up direction
        interpolatedPosition = Vector3.Lerp(FirstDir, SecondDir, 0.5f);
        bullet2.GetComponent<ShotType>().damage = damage;
        //set force
        bullet2.GetComponent<Rigidbody>().AddForce(transform.up * force, ForceMode.Impulse);

        //third bullet---------------------------------------

        //Create projectile
        GameObject bullet3 = Instantiate(proj, transform.position, Quaternion.AngleAxis(-45, Vector3.forward));
        bullet3.AddComponent(type.GetType());
        bullet3.transform.up = SecondDir;

        //right direction
        interpolatedPosition = Vector3.Lerp(FirstDir, SecondDir, 1);
        bullet3.GetComponent<ShotType>().damage = damage;
        //set force
        bullet3.GetComponent<Rigidbody>().AddForce(SecondDir * force, ForceMode.Impulse);

    }
}
