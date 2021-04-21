using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PowerUp
{
    public abstract class GunType : MonoBehaviour
    {
        //public abstract void Fire(ShotType type);
        public abstract void Fire();
    }
    public abstract class ShotType : MonoBehaviour
    {
        protected static Vector2 maxDist = new Vector2(20, 20);
        protected static Vector2 minDist = new Vector2(-20, -20);
        protected virtual void OnTriggerEnter(Collider other) { }
    }
}

