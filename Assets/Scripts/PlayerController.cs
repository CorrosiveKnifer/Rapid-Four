using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerUp;

public class PlayerController : MonoBehaviour
{
    public int ID;
    public Vector2 maxDist;
    public Vector2 minDist;
    public GameObject[] projectileSpawnLoc;
    public ShotType type;
    private Rigidbody body;
    
    private float speed = 100.0f;
    private float rotationSpeed = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        type = new BasicShotType();

        if (ID == 1)
        {
            ApplyGun(new BasicLaserType());
        }
        else
        {
            ApplyGun(new BasicGunType());
        }        
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
            foreach (var gameObject in projectileSpawnLoc)
            {
                gameObject.GetComponent<GunType>().Fire(type);
            }
        }
        else if(InputManager.instance.GetPlayerUnshoot(ID))
        {
            foreach (var gameObject in projectileSpawnLoc)
            {
                gameObject.GetComponent<GunType>().UnFire();
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
    private void ApplyGun(GunType type)
    {
        foreach (var gameObject in projectileSpawnLoc)
        {
            Destroy(gameObject.GetComponent<GunType>());
            gameObject.AddComponent(type.GetType());
        }
        
    }
}
