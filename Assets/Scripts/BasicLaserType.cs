using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerUp;
public class BasicLaserType : GunType
{
    private GameObject proj;
    private GameObject laser;

    public void Start()
    {
        proj = Resources.Load<GameObject>("Prefabs/BasicLaser");
    }

    public override void Fire(ShotType type)
    {
        laser = Instantiate(proj, transform) as GameObject;
        laser.AddComponent(type.GetType());
        laser.transform.localScale = new Vector3(1.0f, 5.0f, 1.0f);
        laser.transform.up = transform.up;
    }

    public override void UnFire()
    {
        if(laser != null)
            Destroy(laser);
    }
}
