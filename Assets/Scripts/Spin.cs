using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// William de Beer
/// </summary>
public class Spin : MonoBehaviour
{
    public float maxSpinSpeed;
    public float minSpinSpeed;

    // Start is called before the first frame update
    void Start()
    {
        float lerp = Random.Range(0.0f, 1.0f);
        float speed = Mathf.Lerp(minSpinSpeed,maxSpinSpeed, lerp);

        Vector3 rotation = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        GetComponent<Rigidbody>().AddRelativeTorque(rotation * speed, ForceMode.Impulse);
    }

}
