using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int ID;
    public Vector2 maxDist;
    public Vector2 minDist;
    public GameObject[] projectileSpawnLoc;

    private Rigidbody body;
    
    private float speed = 100.0f;
    private float rotationSpeed = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        projectileSpawnLoc[0].AddComponent<BasicGunType>();
        projectileSpawnLoc[1].AddComponent<BasicGunType>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetLoc = transform.position;

        if (transform.position.x < minDist.x)
        {
            targetLoc.x = maxDist.x;
        }
        if (transform.position.x > maxDist.x)
        {
            targetLoc.x = minDist.x;
        }
        if (transform.position.y < minDist.y)
        {
            targetLoc.y = maxDist.y;
        }
        if (transform.position.y > maxDist.y)
        {
            targetLoc.y = minDist.y;
        }

        if (targetLoc != transform.position)
        {
            transform.position = targetLoc;
        }

        if (InputManager.instance.GetPlayerShoot(ID))
        {
            for (int i = 0; i < 2; i++)
            {
                projectileSpawnLoc[i].GetComponent<BasicGunType>().Fire();
            }
        }
    }

    private void FixedUpdate()
    {
        float verticalAxis = InputManager.instance.GetVerticalInput(ID);
        float horizontalAxis = InputManager.instance.GetHorizontalInput(ID);

        if (verticalAxis > 0.0f)
        {
            body.AddForce(transform.up * speed * verticalAxis * Time.deltaTime, ForceMode.Acceleration);
        }

        body.rotation = Quaternion.Euler(body.rotation.eulerAngles + new Vector3(0.0f, 0.0f, Mathf.Deg2Rad * -rotationSpeed * horizontalAxis));

    }
}
