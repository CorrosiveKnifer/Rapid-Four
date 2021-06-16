﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBeam : MonoBehaviour
{
    public float damage = 100.0f;
    private float lifetime = 0.5f;

    List<Collider> hitList = new List<Collider>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Astroid>() && !hitList.Contains(other))
        {
            other.gameObject.GetComponent<Astroid>().DealDamage(damage);
        }
        hitList.Add(other);
    }
}
