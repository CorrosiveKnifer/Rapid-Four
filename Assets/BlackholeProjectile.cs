using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeProjectile : MonoBehaviour
{
    public float lifetime = 2.0f;
    GameObject blackhole;
    // Start is called before the first frame update
    void Start()
    {
        blackhole = Resources.Load<GameObject>("Prefabs/Blackhole");
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            ActivateBlackhole();
        }
    }

    public void ActivateBlackhole()
    {
        GameObject gObject = Instantiate(blackhole, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
