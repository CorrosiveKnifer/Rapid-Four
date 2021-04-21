using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    float m_fHealth = 1000.0f;
    public float m_fRotationSpeedMult = 1.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0.0f, m_fRotationSpeedMult * Time.deltaTime, 0.0f));
    }

    public void DealDamage(float _damage)
    {
        m_fHealth -= _damage;
    }
}
