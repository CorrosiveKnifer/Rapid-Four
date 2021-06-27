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
    public override void StartAttack()
    {
        if (m_delay > 0.0f)
        {
            return;
        }

        if (Vector3.Distance(transform.position, m_target.transform.position) <= m_preferedAttackDistance)
        {
            //Start Animation
            GetComponentInChildren<Animator>().SetBool("Attack", true);
        }
        else
        {
            GetComponentInChildren<Animator>().SetBool("Attack", false);
        }
    }

    //Inherited by EnemyAttackBehavour
    public override void DealDamage()
    {
        //Damage the player directly.
        float dist = Vector3.Distance(transform.position, m_target.transform.position);
        if (dist > m_preferedAttackDistance + 2.0f)
            return;

        if (m_target.GetComponentInParent<PlayerController>() != null)
        {
            m_target.GetComponentInParent<PlayerController>().DealDamage(m_myDamage);
        }
        if (m_target.GetComponent<Planet>() != null)
        {
            m_target.GetComponentInParent<PlayerController>().DealDamage(m_myDamage);
        }
        GetComponent<AudioAgent>().Play3DSoundEffect("MeleeAttack", false, 255, Random.Range(0.75f, 1.25f));
        m_delay = m_attackDelay;
    }

    //Inherited by EnemyAttackBehavour
    public override Vector3 GetTargetVector()
    {
        if (m_target == null)
            return Vector3.zero;

        //Move towards the target.
        Vector3 targetLoc = m_target.transform.position + (transform.position - m_target.transform.position).normalized * m_preferedPersonalDistance;
        m_gizmosPosition = targetLoc;

        float distanceToTarget = (targetLoc - transform.position).magnitude;
        //Calculate current speed.
        m_currentSpeed = Mathf.Clamp((m_myMaxSpeed * distanceToTarget) / m_preferedPersonalDistance, 0.0f, m_myMaxSpeed); //Adjust for arrival range.

        return (targetLoc - transform.position).normalized * m_currentSpeed;
    }
}
