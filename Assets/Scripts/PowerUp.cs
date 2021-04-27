using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PowerUp
{
    public abstract class GunType : MonoBehaviour
    {
        public int ammoCost = 0;
        public abstract void Fire(System.Type effectType, int costPayed);
        public abstract void UnFire();
        public abstract int AmmoCount();
    }

    public abstract class ShotType : MonoBehaviour
    {
        public float lifetime = 15.0f;
        public float damage = 0.0f;
        protected float force = 200.0f;
        public string powerUpIcon;
        public bool IsLaser = false;

        protected virtual void Start() { }

        protected virtual void Update()
        {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0)
            {
                Destroy(this);
            }
        }
        protected virtual void OnTriggerEnter(Collider other) { }

    }
}
