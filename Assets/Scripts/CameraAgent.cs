using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAgent : MonoBehaviour
{
    public GameObject[] targets;
    public Vector2 size;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3();
        foreach (var target in targets)
        {
            pos += target.transform.position;
        }

        transform.position = pos / targets.Length + new Vector3(0, 0, -45);
    }
}
