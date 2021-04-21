using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLink : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;

    public LineRenderer laser;
    float m_fMaxPlayerDistance = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (LaserFire())
        {
            laser.enabled = true;
        }
        else
        {
            laser.enabled = false;
        }
    }

    bool LaserFire()
    {
        Vector3 direction = (Player2.transform.position - Player1.transform.position).normalized;
        float distance = (Player2.transform.position - Player1.transform.position).magnitude;
        if (distance > m_fMaxPlayerDistance)
            return false;

        //Debug.DrawRay(Player1.transform.position, direction * distance, Color.magenta, 0.1f);
        laser.SetPosition(0, Player1.transform.position);
        laser.SetPosition(1, Player2.transform.position);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, distance);


        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.tag == "Asteroid")
            {
                Debug.Log("HIT ASTEROID");

            }
        }
        return true;
    }
}
