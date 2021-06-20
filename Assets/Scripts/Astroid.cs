using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rachael Colaco, William de Beer
/// </summary>

public class Astroid : MonoBehaviour
{
    public int Endurance = 1;
    public int ChildNum = 1;

    public float Health = 100.0f;
    public float probability = 50.0f;
    public float m_speed = 10.0f;
    public bool isDestroyed = false;
    public GameObject[] AstroidPrefab;
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

    private GameObject m_planet;

    // Start is called before the first frame update
    void Start()
    {
        GetClosestPlanet();
        rigidBody = GetComponent<Rigidbody>();
        isDestroyed = false;
        
        if (Endurance != 0)
        {
            Health = Mathf.Pow(Endurance, 0.9f) * 100.0f;
        }
        else
        {
            Health = 30.0f;
        }

        transform.localScale = transform.localScale * Random.Range(0.8f, 1.2f);
        rigidBody.mass = Mathf.Pow(2.0f * transform.localScale.x, 3);
    }

    private void GetClosestPlanet()
    {
        float currDist = (m_planet != null) ? Vector3.Distance(transform.position, m_planet.transform.position) : 1000000;
        GameObject closestPlanet = m_planet;

        //For each player in the scene
        foreach (var planet in GameObject.FindGameObjectsWithTag("Planet"))
        {
            //Calculate the distance.
            float playerDist = Vector3.Distance(transform.position, planet.transform.position);

            if (currDist > playerDist)
            {
                //Less than the current lowest, remember this player.
                currDist = playerDist;
                closestPlanet = planet;
            }
        }
        //Switch current target to the lowest
        GameObject oldTarget = m_planet;
        m_planet = closestPlanet;
    }

    private void Awake()
    {
        //Astroiddirection = transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 perp = Vector3.Cross(m_planet.transform.position - transform.position, Vector3.forward).normalized;
        rigidBody.velocity = perp.normalized * m_speed;
    }

    private void OnApplicationQuit()
    {
        //isQuitting = true;
    }

    private void OnDestroy()
    {
        //if(!isQuitting  && !LevelLoader.loadingNextArea)
        //{
        //    GameObject explode = Instantiate(particlePrefab, transform.position, Quaternion.identity);
        //    explode.transform.localScale = transform.localScale;
        //}
    }

    private void FixedUpdate()
    {
        //ClampSpeed();
    }

    void ClampSpeed()
    {
        //if (rigidBody.velocity.magnitude > maxSpeed)
        //{
        //    rigidBody.velocity = rigidBody.velocity.normalized * maxSpeed;
        //}
    }

    //void SpawnChild()
    //{
    //    //for the amount of children the astroid parents will spawn
    //    for (int i = 0; i < ChildNum; i++)
    //    {
    //        
    //        float randomIndex = Random.Range(0, 11);
    //        //float randomFloatFromNumbers = numbers[randomIndex];
    //        randomIndex = randomIndex / 10.0f;
    //
    //        //between the first and second direction
    //        Vector3 interpolatedPosition = Vector3.Lerp(FirstDir, SecondDir, randomIndex);
    //
    //        //create child
    //        GameObject childAstroid = Instantiate(AstroidPrefab[Random.Range(0, AstroidPrefab.Length)], transform.position, 
    //            Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f)));
    //
    //        //set speed
    //        childAstroid.GetComponent<Rigidbody>().AddForce(interpolatedPosition.normalized * 300.0f, ForceMode.Impulse);
    //
    //        //apply that direction onto child
    //        childAstroid.GetComponent<Astroid>().Astroiddirection = interpolatedPosition;
    //        //scale it down
    //        childAstroid.transform.localScale = transform.localScale*0.6f;
    //
    //        //make it known it is a child in that script
    //        childAstroid.GetComponent<Astroid>().Endurance = Endurance - 1;
    //    }
    //}

    //public void SetNumberofAstroids(int num)
    //{
    //    ChildNum = num;
    //}
    public void DealDamage(float _damage)
    {
    //    Health -= _damage;
    }
    //
    public void Slow(float time)
    {
    //    StartCoroutine(SlowEffect(time));
    }
    //
    //private IEnumerator SlowEffect(float time)
    //{
    //    if (slowTime > 0)
    //    {
    //        //Only if additional slowEffect is called.
    //        slowTime += time;
    //        yield return null;
    //    }
    //
    //    slowTime += time;
    //    //Slow effect
    //    float oldSpeed = maxSpeed;
    //    maxSpeed = 2.0f;
    //
    //    GetComponent<MeshRenderer>().material = slowMat;
    //    do
    //    {
    //        yield return new WaitForEndOfFrame();
    //        slowTime -= Time.deltaTime;
    //    } while (slowTime > 0.0f);
    //
    //    //Return to normal effect
    //    maxSpeed = oldSpeed;
    //    GetComponent<MeshRenderer>().material = baseMat;
    //    yield return null;
    //}
}
