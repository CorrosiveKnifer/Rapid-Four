using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Michael Jordan
/// </summary>
public class BasicMeleeBehavour : EnemyAttackBehavour
{
    [Header("Melee Settings")]
    public float m_attackDelay = 0.5f;
    public float m_myDamage = 10.0f;

    protected override void Awake()
    {
        m_preferedPersonalDistance = 6.5f;
        m_preferedAttackDistance = 9.0f;
        base.Awake();
    }

    //Inherited by EnemyAttackBehavour
    public override void StartAttack(GameObject target)
    {
        if (m_delay > 0.0f)
        {
            return;
        }

        if (Vector3.Distance(transform.position, target.transform.position) <= m_preferedAttackDistance)
        {
            //Start Animation

            //Will be replaced with animation event:
            DealDamage(target);
        }
    }

    //Inherited by EnemyAttackBehavour
    public override void DealDamage(GameObject target)
    {
        //Damage the player directly.
        target.GetComponent<PlayerController>().DealDamage(m_myDamage);
        m_delay = m_attackDelay;
    }

    //Inherited by EnemyAttackBehavour
    public override Vector3 GetTargetVector(GameObject target)
    {
        if (target == null)
            return Vector3.zero;

        //Move towards the target.
        Vector3 targetLoc = target.transform.position + (transform.position - target.transform.position).normalized * m_preferedPersonalDistance;
        m_gizmosPosition = targetLoc;

        float distanceToTarget = (targetLoc - transform.position).magnitude;
        //Calculate current speed.
        m_currentSpeed = Mathf.Clamp((m_myMaxSpeed * distanceToTarget) / m_preferedPersonalDistance, 0.0f, m_myMaxSpeed); //Adjust for arrival range.

        return (targetLoc - transform.position).normalized * m_currentSpeed;
    }
}
