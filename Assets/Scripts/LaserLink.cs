using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// William de Beer
/// </summary>
public class LaserLink : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;

    public LineRenderer laser;
    public float m_fLaserDamage = 10.0f;
    public float m_fMaxPlayerDistance = 30.0f;

    public float m_fMinTransferRate = 0.2f;
    public float m_fMaxTransferRate = 0.02f;
    float m_fTransferTimer = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        m_fTransferTimer = m_fMinTransferRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (LaserFire())
        {
            laser.enabled = true;
            AmmoTransfer();
        }
        else
        {
            laser.enabled = false;
            m_fTransferTimer = m_fMinTransferRate;
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
        laser.SetPosition(0, Player1.transform.position + Vector3.forward * 0.5f);
        laser.SetPosition(1, Player2.transform.position + Vector3.forward * 0.5f);
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

    void AmmoTransfer()
    {
        m_fTransferTimer -= Time.deltaTime;
        if (m_fTransferTimer <= 0)
        {
            if (Player1.GetComponent<PlayerController>().maxAmmo > Player1.GetComponent<PlayerController>().Ammo && 
                0 < Player2.GetComponent<PlayerController>().Ammo)
            {
                Player1.GetComponent<PlayerController>().Ammo++;
                Player2.GetComponent<PlayerController>().Ammo--;

                //Debug.Log("P1: " + Player1.GetComponent<PlayerController>().Ammo + ", P2: " + Player2.GetComponent<PlayerController>().Ammo);
                float distance = (Player2.transform.position - Player1.transform.position).magnitude;
                m_fTransferTimer = Mathf.Lerp(m_fMaxTransferRate, m_fMinTransferRate, distance / m_fMaxPlayerDistance);
            }
        }
    }
    
}
