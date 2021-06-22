using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRangeBehavour : EnemyAttackBehavour
{
    [Header("Range Settings")]
    public float m_shotForce = 50.0f;
    public float m_shotDelay = 3.0f;
    public float m_shotDamage = 10.0f;
    public GameObject m_projPrefab;

    protected override void Awake()
    {
        m_preferedPersonalDistance = 45f;
        m_preferedAttackDistance = 50.0f;
    }

    //Inherited by EnemyAttackBehavour
    public override Vector3 GetTargetVector(GameObject target)
    {
        if (target == null)
            return Vector3.zero;

        //Move towards a close enough position.
        Vector3 targetLoc = target.transform.position + (transform.position - target.transform.position).normalized * m_preferedPersonalDistance;
        m_gizmosPosition = targetLoc; //For rendering in base class.

        float distanceToTarget = (targetLoc - transform.position).magnitude;

        //Calculate current speed.
        m_currentSpeed = Mathf.Clamp((m_myMaxSpeed * distanceToTarget) / m_preferedPersonalDistance, 0.0f, m_myMaxSpeed);
        
        m_target = target;

        return (targetLoc - transform.position).normalized * m_currentSpeed;
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
            m_target = target;

            //Raycast forward to predict if it will hit the target
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, m_preferedAttackDistance))
            {
                //It will hit? so shoot projectile:
                if (hit.collider.gameObject.layer == (int)Mathf.Log(m_TargetTag.value, 2))
                {
                    //Start Animation
                    GetComponentInChildren<Animator>()?.SetTrigger("Attack");
                }
            }
        }
    }

    //Inherited by EnemyAttackBehavour
    public override void DealDamage(GameObject target)
    {
        if (m_projPrefab != null)
        {
            m_delay = m_shotDelay;

            //Spawn Projectile
            GameObject newProj = GameObject.Instantiate(m_projPrefab, transform.position, Quaternion.identity);

            //Set inheritance velocity.
            //newProj.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
            
            //Set orientation of projectile:
            newProj.transform.up = transform.forward;

            //Apply shot force forward
            newProj.GetComponent<Rigidbody>().AddForce(transform.forward * m_shotForce, ForceMode.Impulse);

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
