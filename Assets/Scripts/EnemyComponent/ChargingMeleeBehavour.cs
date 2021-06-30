using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingMeleeBehavour : BasicMeleeBehavour
{
    [Header("Charging Settings")]
    public float m_chargePreferedEndDistance = 8.0f;
    public float chargeSpeedMultiplier = 4.0f;
    private bool m_isCharging = false;

    private Vector3 m_targetLoc;

    protected override void Awake()
    {
        m_preferedPersonalDistance = 50.0f;
        m_preferedAttackDistance = 55.0f;
    }

    public void StartCharging()
    {
        m_isCharging = true;
        m_targetLoc = m_target.transform.position;
    }

    public void EndCharging()
    {
        m_isCharging = false;
    }

    public override Vector3 GetTargetVector()
    { 
        if(m_isCharging)
        {
            Vector3 targetLoc = m_targetLoc + (transform.position - m_targetLoc).normalized * m_chargePreferedEndDistance;
            m_gizmosPosition = targetLoc;

            float distanceToTarget = (targetLoc - transform.position).magnitude;
            //Calculate current speed.
            m_currentSpeed = m_myMaxSpeed * chargeSpeedMultiplier;

            return (targetLoc - transform.position).normalized * m_currentSpeed;
        }

        return base.GetTargetVector();
    }

    public override void StartAttack()
    {
        if(m_isCharging)
        {
             if (Vector3.Distance(transform.position, m_targetLoc) <= m_chargePreferedEndDistance * 2.5)
             {
                 GetComponentInChildren<Animator>().SetBool("Charging", false);
            }
        }
        else
        {
            Vector3 targetLoc = m_target.transform.position + (transform.position - m_target.transform.position).normalized * m_preferedPersonalDistance;
            if (Vector3.Distance(transform.position, targetLoc) <= m_preferedAttackDistance - m_preferedPersonalDistance)
            {
                GetComponentInChildren<Animator>().SetBool("Charging", true);
            }
        }
    }

    //Inherited by EnemyAttackBehavour
    public override void DealDamage()
    {
        float dist = Vector3.Distance(transform.position, m_target.transform.position);

        if (dist > m_chargePreferedEndDistance * 2.5f)
            return;

        GetComponent<AudioAgent>().Play3DSoundEffect("MeleeCharge", false, 255, Random.Range(0.75f, 1.25f));

        if (m_target.GetComponentInParent<PlayerController>() != null)
        {
            m_target.GetComponentInParent<PlayerController>().DealDamage(m_myDamage);
        }
        if (m_target.GetComponent<Planet>() != null)
        {
            m_target.GetComponentInParent<PlayerController>().DealDamage(m_myDamage);
        }

        m_delay = m_attackDelay;
    } 

    public override Quaternion IdealRotation()
    {
        Vector3 targetLoc = (m_isCharging) ? m_targetLoc : m_target.transform.position;

        if ((targetLoc - transform.position).x > 0)
            return Quaternion.LookRotation(targetLoc - transform.position, Vector3.up);
        else
            return Quaternion.LookRotation(targetLoc - transform.position, -Vector3.up);
    }
}
