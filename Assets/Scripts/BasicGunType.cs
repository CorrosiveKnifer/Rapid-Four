using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerUp;

public class BasicGunType : GunType
{
    private GameObject proj;
    private GameObject laser;
    private float force = 50.0f;
    private float damage = 30.0f;

    private int playerID;
    private GameObject laserObject;
    public void Start()
    {
        proj = Resources.Load<GameObject>("Prefabs/BasicShot");
        laser = Resources.Load<GameObject>("Prefabs/BasicLaser");

        playerID = GetComponentInParent<PlayerController>().ID;
    }

    public override void Fire(ShotType type)
    {
        switch (playerID)
        {
            default:
            case 0: //Projectile ship

                //Create projectile
                GameObject gObject = Instantiate(proj, transform.position, Quaternion.identity);
                gObject.AddComponent(type.GetType());
                gObject.transform.up = transform.up;
                //Send projectile
                gObject.GetComponent<Rigidbody>().AddForce(transform.up * force, ForceMode.Impulse);
                gObject.GetComponent<ShotType>().damage = damage;
                break;

            case 1: //Sucker Ship
                if(laser != null)
                {
                    //Create Laser, which is parented by us
                    laserObject = Instantiate(laser, transform) as GameObject;
                    laserObject.AddComponent(type.GetType());
                    laserObject.transform.localScale = new Vector3(1.5f, 10.0f, 1.0f);
                    laserObject.transform.up = transform.up;
                    laserObject.GetComponent<ShotType>().damage = damage * Time.deltaTime;
                    laserObject.GetComponent<ShotType>().IsLaser = true;
                }
                break;
        }
    }

    public override void UnFire()
    {
        if (laserObject != null)
            Destroy(laserObject);
    }
}
