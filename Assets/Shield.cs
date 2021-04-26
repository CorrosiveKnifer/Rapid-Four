using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float shieldRepulsion = 100.0f;
    public float reachargeTime = 5.0f;
    public bool IsActive = true;
    private MeshCollider meshCollider;
    private MeshRenderer meshRenderer;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        meshCollider = GetComponent<MeshCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        meshCollider.enabled = IsActive;
        meshRenderer.enabled = IsActive;

        if (!IsActive)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                IsActive = true;
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

            Vector3 direct = (other.transform.position - transform.position).normalized;
            other.GetComponent<Rigidbody>().AddForce(direct * shieldRepulsion, ForceMode.Impulse);
            timer = reachargeTime;
        }
    }
}
