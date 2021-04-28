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
    public float probability = 50.0f;

    public bool isDestroyed = false;
    public GameObject AstroidPrefab;
    Vector3 Astroiddirection;
    public GameObject minimapSprite;

    public int angle = 90;
    Rigidbody rigidBody;
    public float maxSpeed = 8.0f;

    public GameObject particlePrefab;
    public GameObject powerUpPrefab;

    public Material baseMat;
    public Material slowMat;

    Vector3 FirstDir;
    Vector3 SecondDir;

    private bool isQuitting = false;
    private float slowTime = 0.0f;
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
        rigidBody.mass = Mathf.Pow(2.0f * transform.localScale.x, 3);
        Health = Mathf.Sqrt(rigidBody.mass) * 100.0f;
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
            GameManager.instance.AddToScore(transform.localScale.x);

            //if its the parent astroid
            if (Endurance != 0)
            {
                SpawnChild();
            }

            if (Random.Range(0.0f, 100.0f) < probability * Endurance)
            {
                Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
            }

            //destroy itself;
            Destroy(gameObject);
        }

        minimapSprite.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private void OnDestroy()
    {
        if(!isQuitting  && !LevelLoader.loadingNextArea)
        {
            GameObject explode = Instantiate(particlePrefab, transform.position, Quaternion.identity);
            explode.transform.localScale = transform.localScale;
        }
    }

    private void FixedUpdate()
    {
        ClampSpeed();
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
            childAstroid.GetComponent<Astroid>().Endurance = Endurance - 1;
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

    public void Slow(float time)
    {
        StartCoroutine(SlowEffect(time));
    }

    private IEnumerator SlowEffect(float time)
    {
        if (slowTime > 0)
        {
            //Only if additional slowEffect is called.
            slowTime += time;
            yield return null;
        }

        slowTime += time;
        //Slow effect
        maxSpeed = 2.0f;

        GetComponent<MeshRenderer>().material = slowMat;
        do
        {
            yield return new WaitForEndOfFrame();
            slowTime -= Time.deltaTime;
        } while (slowTime > 0.0f);

        //Return to normal effect
        maxSpeed = 8.0f;
        GetComponent<MeshRenderer>().material = baseMat;
        yield return null;
    }
}
