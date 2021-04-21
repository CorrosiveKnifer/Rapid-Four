using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rachael
/// </summary>
public class Astroid : MonoBehaviour
{
    public int Endurance = 1;
    public int ChildNum = 1;
    public bool isDestroyed = false;
    public GameObject AstroidPrefab;
    Vector3 Astroiddirection;

    public int angle = 90;
    public Transform target;

    Vector3 FirstDir;
    Vector3 SecondDir;
    // Start is called before the first frame update
    void Start()
    {
        isDestroyed = false;
        
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
        FirstDir = Quaternion.AngleAxis(angle, Vector3.forward) * Astroiddirection;
        SecondDir = Quaternion.AngleAxis(-angle, Vector3.forward) * Astroiddirection;



        if (isDestroyed)
        {
            if (Endurance != 0)
            {
                SpawnChild();
            }

            Destroy(gameObject);
        }
    }
    void SpawnChild()
    {
        
        for (int i = 0; i < ChildNum; i++)
        {
            
            float randomIndex = Random.Range(0, 11);
            //float randomFloatFromNumbers = numbers[randomIndex];
            randomIndex = randomIndex / 10.0f;

            Vector3 interpolatedPosition = Vector3.Lerp(FirstDir, SecondDir, randomIndex);
            GameObject childAstroid = Instantiate(AstroidPrefab);
            childAstroid.GetComponent<Rigidbody>().velocity = transform.TransformDirection(interpolatedPosition * 10);
            childAstroid.GetComponent<Astroid>().Astroiddirection = interpolatedPosition;
            childAstroid.GetComponent<Astroid>().Endurance--;
            
        }
    }
    void SetNumberofAstroids(int num)
    {
        ChildNum = num;
    }
}
