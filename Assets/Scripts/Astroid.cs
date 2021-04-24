using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rachael, William
/// </summary>
public class Astroid : MonoBehaviour
{
    public int Endurance = 1;
    public int ChildNum = 1;

    public float Health = 100.0f;

    public bool isDestroyed = false;
    public GameObject AstroidPrefab;
    Vector3 Astroiddirection;
    public GameObject minimapSprite;

    public int angle = 90;
    Rigidbody rigidBody;
    public float maxSpeed = 8.0f;

    Vector3 FirstDir;
    Vector3 SecondDir;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        isDestroyed = false;
        
        if (Endurance != 0)
        {
            Health = 100.0f;
        }
        else
        {
            Health = 30.0f;
        }

        transform.localScale = transform.localScale * Random.Range(0.8f, 1.2f);

        Physics.IgnoreLayerCollision(8, 8);
        //GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.up);
    }
    private void Awake()
    {
        Astroiddirection = transform.up;
    }
    // Update is called once per frame
    void Update()
    {
        
        //The direction it moves
        Debug.DrawRay(transform.position, Astroiddirection*10, Color.red);

        //Pontential direction
        Debug.DrawRay(transform.position, (Quaternion.AngleAxis(angle, Vector3.forward) * Astroiddirection) * 10.0f, Color.blue);
        Debug.DrawRay(transform.position, (Quaternion.AngleAxis(-angle, Vector3.forward) * Astroiddirection) * 10.0f, Color.green);

        //Set thoseponteial directions
        FirstDir = Quaternion.AngleAxis(angle, Vector3.forward) * Astroiddirection;
        SecondDir = Quaternion.AngleAxis(-angle, Vector3.forward) * Astroiddirection;


        //if astroid is destroyed
        if (Health <= 0.0f)
        {
            GameManager.instance.AddToScore(0, transform.localScale.x);

            //if its the parent astroid
            if (Endurance != 0)
            {
                SpawnChild();
            }

            //destroy itself;
            Destroy(gameObject);
        }
        ClampSpeed();

        minimapSprite.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }

    void ClampSpeed()
    {
        if (rigidBody.velocity.magnitude > maxSpeed)
        {
            rigidBody.velocity = rigidBody.velocity.normalized * maxSpeed;
        }
    }

    void SpawnChild()
    {
        //for the amount of children the astroid parents will spawn
        for (int i = 0; i < ChildNum; i++)
        {
            
            float randomIndex = Random.Range(0, 11);
            //float randomFloatFromNumbers = numbers[randomIndex];
            randomIndex = randomIndex / 10.0f;

            //between the first and second direction
            Vector3 interpolatedPosition = Vector3.Lerp(FirstDir, SecondDir, randomIndex);

            //create child
            GameObject childAstroid = Instantiate(AstroidPrefab);

            //set speed
            childAstroid.GetComponent<Rigidbody>().AddForce(interpolatedPosition.normalized * 300.0f, ForceMode.Impulse);

            //apply that direction onto child
            childAstroid.GetComponent<Astroid>().Astroiddirection = interpolatedPosition;
            //scale it down
            childAstroid.transform.localScale = transform.localScale*0.5f;

            //make it known it is a child in that script
            childAstroid.GetComponent<Astroid>().Endurance--;
        }
    }
    public void SetNumberofAstroids(int num)
    {
        ChildNum = num;
    }
    public void DealDamage(float _damage)
    {
        Health -= _damage;
    }
}
