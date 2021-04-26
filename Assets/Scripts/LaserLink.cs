using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLink : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;

    public LineRenderer laser;
    public float m_fLaserDamage = 10.0f;
    public float m_fMaxPlayerDistance = 30.0f;

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
        if (!Player1.GetComponent<PlayerController>().CheckAlive() || !Player2.GetComponent<PlayerController>().CheckAlive())
        {
            return false;
        }

        Vector3 direction = (Player2.transform.position - Player1.transform.position).normalized;
        float distance = (Player2.transform.position - Player1.transform.position).magnitude;
        if (distance > m_fMaxPlayerDistance)
            return false;

        //Debug.DrawRay(Player1.transform.position, direction * distance, Color.magenta, 0.1f);
        laser.SetPosition(0, Player1.transform.position);
        laser.SetPosition(1, Player2.transform.position);
        RaycastHit[] hits = Physics.RaycastAll(Player1.transform.position, direction, distance);


        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.tag == "Planet")
            {
                return false;
            }
            if (hit.collider.tag == "Asteroid")
            {
                // Asteroid damage here.
                hit.collider.gameObject.GetComponent<Astroid>().DealDamage(m_fLaserDamage * Time.deltaTime);
            }
        }
        return true;
    }
}
