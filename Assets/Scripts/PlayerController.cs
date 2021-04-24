using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerUp;

public class PlayerController : MonoBehaviour
{
    public int ID;
    public int maxAmmo = 10;
    public int Ammo;

    public Vector2 maxDist;
    public Vector2 minDist;
    public GameObject[] projectileSpawnLoc;
    public ShotType type;
    private Rigidbody body;
    
    public float speed = 350.0f;
    private float rotationSpeed = 120.0f;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponentInChildren<Rigidbody>();
        type = new BasicShotType();
        ApplyGun(new BasicGunType());
        Ammo = maxAmmo;
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
            if (Ammo > 0 || maxAmmo < 0)
            {
                foreach (var gameObject in projectileSpawnLoc)
                {
                    gameObject.GetComponent<GunType>().Fire(type);
                }
                Ammo = Mathf.Clamp(Ammo -1, 0, 100);
            }
            else Debug.Log("You are out of ammo!");
            
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

        switch (ID)
        {
            default:
            case 0:
                if (verticalAxis > 0.0f)
                {
                    body.AddForce(transform.up * speed * verticalAxis * Time.deltaTime, ForceMode.Acceleration);
                }
                body.rotation = Quaternion.Euler(body.rotation.eulerAngles + new Vector3(0.0f, 0.0f, Mathf.Deg2Rad * -rotationSpeed * horizontalAxis));
                break;
            case 1:
                if(verticalAxis != 0 || horizontalAxis != 0)
                {
                    Vector3 direct = new Vector3(horizontalAxis, verticalAxis, 0.0f).normalized;
                    body.AddForce(direct * speed * Time.deltaTime, ForceMode.Acceleration);
                    body.rotation = Quaternion.Slerp(body.rotation, Quaternion.LookRotation(new Vector3(0, 0, 1), direct), 0.15f);
                }
                break;
        }
    }
    public void ApplyGun(GunType type)
    {
        foreach (var gameObject in projectileSpawnLoc)
        {
            Destroy(gameObject.GetComponent<GunType>());
            gameObject.AddComponent(type.GetType());
        }
    }
}
