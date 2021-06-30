using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Michael Jordan
/// </summary>
public class StrafingRangedBehavour : BasicRangeBehavour
{
    [Header("Strafing Behavour Settings")]
    public float m_strafingRange = 5.0f;
    public float m_strafingSpeed = 5.0f;

    private bool IsStrafing = false;

    //Inherited by EnemyAttackBehavour
    public override Vector3 GetTargetVector()
    {
        Vector3 targetLoc = m_target.transform.position + (transform.position - m_target.transform.position).normalized * m_preferedPersonalDistance;

        //Strafing mechanics
        if (Vector3.Distance(transform.position, targetLoc) <= m_strafingRange)
        {
            //Get Perpendicular vector
            Vector3 perpDirect = Vector3.Cross(transform.position - m_target.transform.position, Vector3.forward).normalized;

            m_gizmosPosition = transform.position + perpDirect * m_strafingSpeed; //For drawing

            //Update speed
            m_currentSpeed = m_strafingSpeed;

            IsStrafing = true;

            return perpDirect * m_strafingSpeed;
        }

        IsStrafing = true;
        return base.GetTargetVector();
    }

    //Inherited by EnemyAttackBehavour
    public override void DealDamage()
    {
        if (m_projPrefab != null)
        {
            m_delay = m_shotDelay;

            GameObject newProj = GameObject.Instantiate(m_projPrefab, transform.position, Quaternion.identity);
            
            //Set inheritance velocity.
            newProj.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
            
            //Set orientation of projectile:
            newProj.transform.up = transform.forward;
            
            Vector3 force = transform.forward;
            if (IsStrafing)
            {
                //Adust the aim to negate inherited velocity.
                Vector3 perpDirect = Vector3.Cross(transform.position - m_target.transform.position, Vector3.forward).normalized;
                force = (transform.forward + (0.1f * perpDirect)).normalized;
            }

            //Apply shot force forward
            newProj.GetComponent<Rigidbody>().AddForce(force * m_shotForce, ForceMode.Impulse);
            
            //Update damage value
            newProj.GetComponent<Projectile>().m_damage = m_shotDamage;

            //Ignore collision with self
            foreach (var collider in GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(collider, newProj.GetComponent<Collider>());
            }
        }
    }
}
