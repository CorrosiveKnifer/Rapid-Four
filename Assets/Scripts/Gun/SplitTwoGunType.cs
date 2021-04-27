﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerUp;
using System;

public class SplitTwoGunType : GunType
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
        ammoCost = 2;
        proj = Resources.Load<GameObject>("Prefabs/BasicShot");
        laser = Resources.Load<GameObject>("Prefabs/BasicLaser");

        playerID = GetComponentInParent<PlayerController>().ID;
    }

    protected void OnDestroy()
    {
        if (laser1 != null)
            Destroy(laser1);

        if (laser2 != null)
            Destroy(laser2);
    }
    public override void Fire(System.Type etype, int costPayed)
    {
        if (!etype.IsSubclassOf(typeof(ShotType)))
            return;

        switch (playerID)
        {
            default:
            case 0: // ship

                SpawnChild(etype, costPayed);
                break;

            case 1: //Sucker Ship
                if (laserObject == null)
                {
                    SpawnLaserChild(etype);
                }
                break;
        }
    }

    public override void UnFire()
    {
        if (laser1 != null)
            Destroy(laser1);
        if (laser2 != null)
            Destroy(laser2);
    }

    void SpawnLaserChild(System.Type type)
    {
        //Set thoseponteial directions
        Vector3 FirstDir = Quaternion.AngleAxis(15, Vector3.forward) * transform.up;
        Vector3 SecondDir = Quaternion.AngleAxis(-15, Vector3.forward) * transform.up;

        //first laser from the left---------------------------------------

        //Create laser 1
        laser1 = Instantiate(laser, transform) as GameObject;
        laser1.AddComponent(type);
        laser1.transform.localScale = new Vector3(1.5f, 10.0f, 1.0f);
        laser1.GetComponent<ShotType>().damage = damage * Time.deltaTime;

        //set direction to the left
        laser1.transform.up = FirstDir;
        laser1.GetComponent<ShotType>().IsLaser = true;


        //second laser---------------------------------------

        //Create laser 2
        laser2 = Instantiate(laser, transform) as GameObject;
        laser2.AddComponent(type);
        laser2.transform.localScale = new Vector3(1.5f, 10.0f, 1.0f);
        laser2.GetComponent<ShotType>().damage = damage * Time.deltaTime;

        //set direction to the right
        laser2.transform.up = SecondDir;
        laser2.GetComponent<ShotType>().IsLaser = true;

    }
    void SpawnChild(System.Type type, int costPayed)
    {
        //Set thoseponteial directions
        Vector3 FirstDir = Quaternion.AngleAxis(15, Vector3.forward) * transform.up;
        Vector3 SecondDir = Quaternion.AngleAxis(-15, Vector3.forward) * transform.up;
        Vector3 interpolatedPosition;
        //first bullet---------------------------------------
        if (costPayed >= 1)
        {
            //Create projectile
            GameObject bullet1 = Instantiate(proj, transform.position, Quaternion.AngleAxis(45, Vector3.forward));
            bullet1.AddComponent(type);
            bullet1.transform.up = FirstDir;

            //left direction
            interpolatedPosition = Vector3.Lerp(FirstDir, SecondDir, 0);
            bullet1.GetComponent<ShotType>().damage = damage;
            //set force
            bullet1.GetComponent<Rigidbody>().AddForce(FirstDir * force, ForceMode.Impulse);
        }

        //second bullet---------------------------------------
        if (costPayed >= 2)
        {
            //Create projectile
            GameObject bullet3 = Instantiate(proj, transform.position, Quaternion.AngleAxis(-45, Vector3.forward));
            bullet3.AddComponent(type);
            bullet3.transform.up = SecondDir;

            //right direction
            interpolatedPosition = Vector3.Lerp(FirstDir, SecondDir, 1);
            bullet3.GetComponent<ShotType>().damage = damage;
            //set force
            bullet3.GetComponent<Rigidbody>().AddForce(SecondDir * force, ForceMode.Impulse);
        }
    }

    public override int AmmoCount()
    {
        return 12;
    }
}
