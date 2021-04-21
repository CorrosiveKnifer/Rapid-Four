using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLink : MonoBehaviour
{
    GameObject Player1;
    GameObject Player2;

    public LineRenderer laser;
    float m_fMaxPlayerDistance = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (Player2.transform.position - Player1.transform.position).normalized;
        Debug.DrawRay(Player1.transform.position, direction * m_fMaxPlayerDistance, Color.magenta, 0.1f);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, m_fMaxPlayerDistance);

        if (hits.Length == 0)
        {
            return;
        }

        RaycastHit closestHit = hits[0];

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.tag == "Asteroid")
            {
                Debug.Log("HIT ASTEROID");

            }
        }
    }
}
