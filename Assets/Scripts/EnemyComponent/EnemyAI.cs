using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Michael Jordan
/// </summary>
public class EnemyAI : MonoBehaviour
{
    [Header("EnemySettings")]
    [Range(0.0f, 1.0f)]
    public float m_RotationSlerp = 0.35f; //Slerp time value for rotation
    public float m_PrefDist = 25.0f; //Prefered distance away from the currentTarget
    public float m_AttackDist = 5.0f; //Distance from the targetLocation, when to start shooting.
    public float m_maxAttackDelay = 3.0f; //Delay inbetween attacks.
    public GameObject m_healthBar;
    public float m_startingHealth = 100.0f;

    [Header("Enemy Prefabs")]
    public GameObject m_deathPrefab;

    [Header("Read Only Variables")]
    [ReadOnly]
    public GameObject m_CurrentTarget;
    [ReadOnly]
    public float m_CurrentHealth;

    private float m_maxSpeed; //Maximum speed of the ship.
    private List<GameObject> m_Targets;
    private List<GameObject> m_NeighbourhoodList;
    private Quaternion m_TargetRot;
    private Vector3 m_ForwardVector;
    private LayerMask m_TargetTag; //Layer of the target fields.

    [Header("Stun timer")]
    private float m_stunTimer = 0.0f;
    private GameObject planet;
    // Start is called before the first frame update
    void Start()
    {
        m_TargetTag = GetComponent<EnemyAttackBehavour>().m_TargetTag;
        if (m_TargetTag.value == 0)
        {
            m_TargetTag = LayerMask.NameToLayer("Player");
            m_Targets = new List<GameObject>(GameObject.FindGameObjectsWithTag(LayerMask.LayerToName(m_TargetTag.value)));
        }
        else
        {
            m_Targets = new List<GameObject>(GameObject.FindGameObjectsWithTag(LayerMask.LayerToName((int)Mathf.Log(m_TargetTag.value, 2))));
        }

        if(!m_Targets.Contains(GameObject.FindGameObjectWithTag("Planet")))
            m_Targets.Add(GameObject.FindGameObjectWithTag("Planet"));

        m_maxSpeed = GetComponent<EnemyAttackBehavour>().m_myMaxSpeed;
        m_CurrentHealth = m_startingHealth;
        m_CurrentTarget = null;
        m_NeighbourhoodList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateClosestTarget();
        GetComponent<EnemyAttackBehavour>().m_target = m_CurrentTarget;
        GetComponent<EnemyAttackBehavour>().m_planetKiller = m_CurrentTarget.GetComponentInParent<Planet>() != null;
        m_TargetRot = GetComponent<EnemyAttackBehavour>().IdealRotation();

        //transform.position = Vector3.Lerp(transform.position, targetPosition, moveLerp);
        transform.rotation = Quaternion.Slerp(transform.rotation, m_TargetRot, m_RotationSlerp);

        if (GetComponent<EnemyAttackBehavour>().IsWithinPreferedDistance())
        {
            GetComponent<EnemyAttackBehavour>().StartAttack();
        }
        else
        {
            GetComponentInChildren<Animator>().SetBool("Attack", false);
        }

        //For each neighbour
        foreach (var neighbour in m_NeighbourhoodList)
        {
            if (neighbour != null)
            {
                //Calculate the direction away from the neighbour
                Vector3 direct = (transform.position - neighbour.transform.position).normalized;
                m_ForwardVector += direct * m_maxSpeed;
            }
        }

        //Add the force towards the target location
        m_ForwardVector += GetComponent<EnemyAttackBehavour>().GetTargetVector();

        //Normalise to get the actual direction
        m_ForwardVector.Normalize();

        if (m_stunTimer > 0.0f)
        {
            m_stunTimer -= Time.deltaTime;
        }
        else
        {
           GetComponent<Rigidbody>().velocity = m_ForwardVector * GetComponent<EnemyAttackBehavour>().m_currentSpeed;
        }

        if(m_healthBar != null)
        {
            m_healthBar.transform.parent.gameObject.SetActive(m_CurrentHealth / m_startingHealth != 1);
            m_healthBar.transform.localScale = new Vector3(m_CurrentHealth / m_startingHealth, 1, 1);
        }
            
    }

    public void SetTarget(GameObject newTarget)
    {
        m_Targets.Add(newTarget);
    }

    public void StunTarget(float duration)
    {
        if (duration > m_stunTimer)
            m_stunTimer = duration;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != (int)Mathf.Log(m_TargetTag.value, 2))
        {
            m_NeighbourhoodList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != (int)Mathf.Log(m_TargetTag.value, 2))
        {
            m_NeighbourhoodList.Remove(other.gameObject);
        }
    }

    /// <summary>
    /// Updates the currentTarget to the closest player.
    /// </summary>
    /// <returns> Status of if the closest player has changed. </returns>
    private bool UpdateClosestTarget()
    {
        //Get current distance to the target
        float currDist = (m_CurrentTarget != null) ? Vector3.Distance(transform.position, m_CurrentTarget.transform.position) : 1000000;
        GameObject closestPlayer = null;

        if (m_CurrentTarget != null 
            && (m_CurrentTarget.GetComponentInParent<Planet>() != null 
            || m_CurrentTarget.GetComponentInParent<PlayerController>().CheckAlive()
            || m_CurrentTarget.GetComponentInParent<Decoy>()))
        {
            closestPlayer = m_CurrentTarget;
        }

        //For each player in the scene
        foreach (var target in m_Targets)
        {
            if (target == null)
            {
                m_Targets.Remove(target);
            }
        }

        foreach (var player in m_Targets)
        {
            if(player.GetComponentInParent<PlayerController>() != null && player.GetComponentInParent<PlayerController>().CheckAlive())
            {
                continue;
            }
            //Calculate the distance.
            float playerDist = Vector3.Distance(transform.position, player.transform.position);

            if (currDist > playerDist)
            {
                //Less than the current lowest, remember this player.
                currDist = playerDist;
                closestPlayer = player;
            }
        }
        //Switch current target to the lowest
        GameObject oldTarget = m_CurrentTarget;
        m_CurrentTarget = closestPlayer;

        //Return true if there is a change.
        return oldTarget != m_CurrentTarget;
    }

    /// <summary>
    /// Hurt the enemy.
    /// </summary>
    /// <param name="damage"> Damage to deal to the enemy</param>
    public void HurtEnemy(float damage, uint playerID = 0)
    {
        m_CurrentHealth -= damage;
        if (m_CurrentHealth <= 0.0f)
        {
            if(m_deathPrefab != null)
            {
                GameObject.Instantiate(m_deathPrefab, transform.position, Quaternion.identity);
            }
            GameManager.Score[playerID] += (int)m_startingHealth;
            Destroy(gameObject);
        }
    }
}
