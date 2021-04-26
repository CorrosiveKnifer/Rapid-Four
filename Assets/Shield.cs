using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float shieldRepulsion = 100.0f;
    public float reachargeTime = 5.0f;
    public bool IsActive = true;
    private MeshCollider meshCollider;
    public ParticleSystem system;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        meshCollider = GetComponent<MeshCollider>();
        system.Play();
    }

    // Update is called once per frame
    void Update()
    {
        meshCollider.enabled = IsActive;

        if (!IsActive)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                IsActive = true;
                system.Play();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!IsActive)
            return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Asteroid"))
        {
            IsActive = false;
            system.Stop();
            Vector3 direct = (other.transform.position - transform.position).normalized;
            other.GetComponent<Rigidbody>().AddForce(direct * shieldRepulsion, ForceMode.Impulse);
            timer = reachargeTime;
        }
    }
}
