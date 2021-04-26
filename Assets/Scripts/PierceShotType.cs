using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerUp;

/// <summary>
/// Rachael
/// </summary>
public class PierceShotType : ShotType
{
    int endurance = 1;
    // Start is called before the first frame update

    protected override void Start()
    {
        if(IsLaser)
        {
            transform.localScale = new Vector3(1.5f, 15.0f, 1.0f);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Asteroid")
        {
            other.gameObject.GetComponent<Astroid>().DealDamage(damage);

            if (!IsLaser)
            {
                if (endurance == -1)
                {
                    Destroy(gameObject);
                }
                endurance--;
            }
        }
    }
}
