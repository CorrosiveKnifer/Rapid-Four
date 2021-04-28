using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerUp;
using System;

public class BasicGunType : GunType
{
    private GameObject proj;
    private GameObject laser;
    private float force = 50.0f;
    private float damage = 30.0f;

    private int playerID;
    private GameObject laserObject;
    private System.Type currentType;
    public void Start()
    {
        ammoCost = 1;
        proj = Resources.Load<GameObject>("Prefabs/BasicShot");
        laser = Resources.Load<GameObject>("Prefabs/BasicLaser");

        playerID = GetComponentInParent<PlayerController>().ID;


        //Spawn Laser
        currentType = typeof(BasicShotType);
        laserObject = Instantiate(laser, transform) as GameObject;
        laserObject.transform.localScale = new Vector3(1.5f, 10.0f, 1.0f);
        laserObject.transform.up = transform.up;
        laserObject.AddComponent(typeof(BasicShotType));
        laserObject.GetComponent<ShotType>().damage = damage * Time.deltaTime;
        laserObject.GetComponent<ShotType>().IsLaser = true;
        laserObject.GetComponent<ShotType>().damage = damage * Time.deltaTime;
        laserObject.GetComponent<ShotType>().IsLaser = true;
        laserObject.GetComponent<ShotType>().probability = 35.0f;
        laserObject.GetComponent<ShotType>().delay = 1.0f;
        laserObject.SetActive(false);
    }
    private void Update()
    {
        if(playerID == 1)
        {
            laserObject.GetComponent<ShotType>().IsLaser = true;
        }
    }
    protected void OnDestroy()
    {
        if (laserObject != null)
            Destroy(laserObject);
    }

    public override void Fire(System.Type etype, int costPayed)
    {
        if(!etype.IsSubclassOf(typeof(ShotType)))
            return;

        switch (playerID)
        {
            default:
            case 0: //Projectile ship
                if(costPayed >= ammoCost)
                {
                    //Create projectile
                    GameObject gObject = Instantiate(proj, transform.position, Quaternion.identity);
                    gObject.AddComponent(etype);
                    gObject.transform.up = transform.up;
                    //Send projectile
                    gObject.GetComponent<Rigidbody>().AddForce(transform.up * force, ForceMode.Impulse);
                    gObject.GetComponent<ShotType>().damage = damage;

                }
                break;

            case 1: //Sucker Ship
                if(currentType != etype)
                {
                    if(laserObject.GetComponent<ShotType>() != null)
                        Destroy(laserObject.GetComponent<ShotType>());
                    laserObject.AddComponent(etype);
                    laserObject.GetComponent<ShotType>().damage = damage * Time.deltaTime;
                    laserObject.GetComponent<ShotType>().IsLaser = true;
                    laserObject.GetComponent<ShotType>().probability = 35.0f;
                    laserObject.GetComponent<ShotType>().delay = 1.0f;
                }

                laserObject.SetActive(true);
                break;
        }
    }

    public override void UnFire()
    {
        if (laserObject != null)
            laserObject.SetActive(false);
    }

    public override int AmmoCount()
    {
        return 14;
    }
}
