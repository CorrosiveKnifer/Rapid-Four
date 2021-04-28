using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public PlayerController player;
    public float shieldRepulsion = 100.0f;
    public float reachargeTime = 5.0f;
    public float invincibilityTime = 0.5f;
    public bool IsActive = true;
    private MeshCollider meshCollider;
    public ParticleSystem system;
    public float timer;

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
        SetShieldVisuals(IsActive);

        if (!IsActive)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                IsActive = true;
                SetShieldVisuals(IsActive);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!IsActive)
            return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Asteroid"))
        {
            player.SetInvincibilityTimer(invincibilityTime);
            IsActive = false;
            Vector3 direct = (other.transform.position - transform.position).normalized;
            if (other.GetComponent<Astroid>().Endurance != 5)
                other.GetComponent<Rigidbody>().velocity = direct * other.GetComponent<Astroid>().maxSpeed;
            timer = reachargeTime;
        }
    }

    public void SetShieldVisuals(bool _active)
    {
        if (!player.CheckAlive())
        {
            system.gameObject.SetActive(false);
            return;
        }

        system.gameObject.SetActive(_active);
        if (_active)
            system.Play();
    }
}
