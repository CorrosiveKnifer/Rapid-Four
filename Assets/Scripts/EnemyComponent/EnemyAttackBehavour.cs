using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Michael Jordan
/// </summary>
public abstract class EnemyAttackBehavour : MonoBehaviour
{
    [Header("BehavourStatistics")]
    public float m_myMaxSpeed = 10.0f;
    public LayerMask m_TargetTag;
    public bool m_planetKiller = false;

    [ReadOnly]
    public float m_currentSpeed = 0.0f;

    [ReadOnly]
    public float m_delay = 0.0f;

    public GameObject m_target { protected get; set; }

    protected float m_preferedPersonalDistance = 0.0f;
    protected float m_preferedAttackDistance = 0.0f;
    protected Vector3 m_gizmosPosition;

    protected virtual void Awake()
    {
        m_gizmosPosition = transform.position;
    }

    protected virtual void Update()
    {
        if (m_delay > 0)
            m_delay = Mathf.Clamp(m_delay - Time.deltaTime, 0.0f, 10.0f);
    }

    public virtual bool IsWithinPreferedDistance()
    {
        return Vector3.Distance(transform.position, m_target.transform.position) <= m_preferedAttackDistance;
    }

    /// <summary>
    /// Start Attacking based on this behavour.
    /// </summary>
    /// <param name="target">Target entity to attack.</param>
    public abstract void StartAttack();

    /// <summary>
    /// Calculates the forward vector to the prefered target location for this behavour.
    /// </summary>
    /// <param name="target">Target entity to attack.</param>
    /// <returns> Force Vector to apply towards the ideal location. </returns>
    public abstract Vector3 GetTargetVector();

    /// <summary>
    /// Start Dealing damage towards the target based on this behavour.
    /// </summary>
    /// <param name="target">Target entity to attack.</param>
    public abstract void DealDamage();

    public virtual Quaternion IdealRotation()
    {
        if (m_target == null)
            return Quaternion.identity;

        if ((m_target.transform.position - transform.position).x > 0)
            return Quaternion.LookRotation(m_target.transform.position - transform.position, Vector3.up);
        else
            return Quaternion.LookRotation(m_target.transform.position - transform.position, -Vector3.up);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(m_gizmosPosition, 0.5f);
    }
}
