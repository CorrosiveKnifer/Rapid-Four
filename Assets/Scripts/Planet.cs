﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    float m_fMaxHealth = 1000.0f;
    float m_fHealth;
    public float m_fRotationSpeedMult = 1.0f;
    public GameObject minimapSprite;

    private void Start()
    {
        m_fHealth = m_fMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0.0f, m_fRotationSpeedMult * Time.deltaTime, 0.0f));
        GameManager.instance.SetPlanetHealthBar(m_fHealth / m_fMaxHealth);
        minimapSprite.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }

    public void DealDamage(float _damage)
    {
        m_fHealth -= _damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Asteroid"))
        {
            Astroid asteroid = other.gameObject.GetComponentInParent<Astroid>();
            if (asteroid != null)
            {
                m_fHealth -= (int)(asteroid.transform.localScale.x * 15.0f);

                Destroy(other.gameObject);
            }
        }
    }
}
