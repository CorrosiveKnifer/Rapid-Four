using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingMeleeBehavour : BasicMeleeBehavour
{
    [Header("Charging Settings")]
    public float m_chargePreferedEndDistance = 8.0f;
    public float chargeSpeedMultiplier = 4.0f;
    private bool m_isCharging = false;

    protected override void Awake()
    {
        m_preferedPersonalDistance = 50.0f;
        m_preferedAttackDistance = 55.0f;
    }

    public override Vector3 GetTargetVector(GameObject target)
    { 
        if(m_isCharging)
        {
            Vector3 targetLoc = target.transform.position + (transform.position - target.transform.position).normalized * m_chargePreferedEndDistance;
            m_gizmosPosition = targetLoc;

            float distanceToTarget = (targetLoc - transform.position).magnitude;
            //Calculate current speed.
            m_currentSpeed = m_myMaxSpeed * chargeSpeedMultiplier;

            return (targetLoc - transform.position).normalized * m_currentSpeed;
        }

        return base.GetTargetVector(target);
    }

    public override void StartAttack(GameObject target)
    {
        if(m_isCharging)
        {
            if (Vector3.Distance(transform.position, target.transform.position) <= m_chargePreferedEndDistance + 1.0f)
            {
                DealDamage(target);
                m_isCharging = false;
            }
        }
        else
        {
            Vector3 targetLoc = target.transform.position + (transform.position - target.transform.position).normalized * m_preferedPersonalDistance;
            if (Vector3.Distance(transform.position, targetLoc) <= m_preferedAttackDistance - m_preferedPersonalDistance)
            {
                m_isCharging = true;
            }
        }
    }
}
