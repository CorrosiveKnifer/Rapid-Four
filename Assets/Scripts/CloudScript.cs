using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScript : MonoBehaviour
{
    public float radius;
    public float miniRadius;
    public float damage = 5.0f;
    public float shrinkTime = 3.0f;
    public Camera MiniMapCamera;

    private PlayerController[] players;
    public bool IsShrinking = false;
    private float trueRadius;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        trueRadius = radius;
    }

    // Update is called once per frame
    void Update()
    {
        players = GameObject.FindObjectsOfType<PlayerController>();

        foreach (var player in players)
        {
            float distance = Vector3.Distance(Vector3.zero, player.gameObject.transform.position);

            if(distance >= trueRadius)
            {
                player.DealDamage(Time.deltaTime * damage * Mathf.Pow(distance, 1.2f)/ trueRadius);
            }
        }

        MiniMapCamera.orthographicSize = trueRadius;

        if (IsShrinking)
        {
            trueRadius = Mathf.Lerp(radius, miniRadius, timer / shrinkTime);
            timer += Time.deltaTime;
            if (timer >= shrinkTime)
            {
                trueRadius = miniRadius;
                IsShrinking = false;
                timer = 0.0f;
            }
        }
    }

    public void StartSmallArena()
    {
        IsShrinking = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(Vector3.zero, radius);
        Gizmos.DrawWireSphere(Vector3.zero, trueRadius);
        Gizmos.DrawWireSphere(Vector3.zero, miniRadius);
    }
}
