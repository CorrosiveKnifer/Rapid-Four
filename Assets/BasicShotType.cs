using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerUp;

public class BasicShotType : ShotType
{
    private float speed = 50.0f;

    // Start is called before the first frame update
    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.up * speed, ForceMode.Impulse);
    }
    private void Update()
    {
        if (transform.position.x < minDist.x || transform.position.x > maxDist.x)
        {
            Destroy(gameObject);
        }
        if (transform.position.y < minDist.y || transform.position.y > maxDist.y)
        {
            Destroy(gameObject);
        }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Asteroid")
        {
            Destroy(gameObject);
        }
    }
}
