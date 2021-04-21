using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PowerUp;

public class BasicGunType : GunType
{
    private GameObject proj;

    public void Start()
    {
        proj = Resources.Load<GameObject>("Prefabs/BasicShot");
    }

    public override void Fire()
    {
        GameObject gObject = Instantiate(proj, transform.position, Quaternion.identity) as GameObject;
        gObject.transform.up = transform.up;
    }
}
