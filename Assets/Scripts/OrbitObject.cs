using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// William de Beer
/// </summary>

public class OrbitObject : MonoBehaviour
{
    const float m_GravityMult = 0.1f;
    public Rigidbody m_Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        if(m_Rigidbody == null)
            m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        OrbitObject[] orbitobjects = FindObjectsOfType<OrbitObject>();
        foreach (OrbitObject orbitobject in orbitobjects)
        {
            if (orbitobject != this)
                OrbitGravity(orbitobject);
        }
        if (transform.position.z != 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }

    void OrbitGravity(OrbitObject _other)
    {
        Rigidbody otherRB = _other.m_Rigidbody;

        Vector3 direction = m_Rigidbody.position - otherRB.position;
        float distance = direction.magnitude;

        if (distance == 0)
        {
            return;
        }

        float mag = m_GravityMult * (m_Rigidbody.mass * otherRB.mass) / Mathf.Pow(distance, 0.3f);
        otherRB.AddForce(direction.normalized * mag);
    }
}
