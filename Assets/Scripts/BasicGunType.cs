using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerUp;

public class BasicGunType : GunType
{
    private GameObject proj;
    private float force = 50.0f;

    public void Start()
    {
        proj = Resources.Load<GameObject>("Prefabs/BasicShot");
    }

    public override void Fire(ShotType type)
    {
        GameObject gObject = Instantiate(proj, transform.position, Quaternion.identity);
        gObject.AddComponent(type.GetType());
        gObject.transform.up = transform.up;
        gObject.GetComponent<Rigidbody>().AddForce(transform.up * force, ForceMode.Impulse);
    }

    public override void UnFire()
    {
        //Do Nothing
    }
}
