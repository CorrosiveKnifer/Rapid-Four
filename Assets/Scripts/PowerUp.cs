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
        public abstract void Spawn(Vector3 origin, Vector3 direction);
        protected void OnCollisionEnter(Collision collision) { }
    }
}

