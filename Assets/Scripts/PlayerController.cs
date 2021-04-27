using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PowerUp;

public class PlayerController : MonoBehaviour
{
    public int ID;
    public int maxAmmo = 20;
    public int Ammo;

    public Vector2 maxDist;
    public Vector2 minDist;

    public GameObject[] projectileSpawnLoc;
    public Shield shieldObject;
    public Animator[] engine;
    public Animator immune;
    public CameraAgent myCamera;

    private System.Type effectType;
    private System.Type gunType;
    private Rigidbody body;
    
    public float speed = 350.0f;
    public float rotationSpeed = 120.0f;

    public GameObject particlePrefab;

    // Death stuff
    bool isAlive = true;
    float m_fRespawnTime = 10.0f;
    float m_DeathTimer = 0.0f;
    bool isInvincible = false;
    float m_fInvincibilityTime = 2.0f;
    float m_InvincibilityTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        shieldObject = GetComponentInChildren<Shield>();
        body = GetComponentInChildren<Rigidbody>();
        effectType = typeof(BasicShotType);
        ApplyGun(typeof(BasicGunType));
        
        Ammo = (maxAmmo > 0) ? maxAmmo : 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            if (InputManager.instance.GetPlayerShoot(ID))
            {
                foreach (var gameObject in projectileSpawnLoc)
                {
                    if (Ammo > 0 || maxAmmo < 0)
                    {
                        int cost = gameObject.GetComponent<GunType>().ammoCost;
                        if (Ammo < gameObject.GetComponent<GunType>().ammoCost)
                        {
                            cost = Ammo;
                        }
                        else Debug.Log("You are out of ammo!");

                        gameObject.GetComponent<GunType>().Fire(effectType, cost);
                        
                        if (maxAmmo > 0)
                            Ammo = Mathf.Clamp(Ammo - cost, 0, 100);

                    }

                }
            }
        }

        if(InputManager.instance.GetPlayerUnshoot(ID))
        {
            foreach (var gameObject in projectileSpawnLoc)
            {
                gameObject.GetComponent<GunType>().UnFire();
            }
        }
        DeathUpdate();
    }

    private void FixedUpdate()
    {
        float verticalAxis = InputManager.instance.GetVerticalInput(ID);
        float horizontalAxis = InputManager.instance.GetHorizontalInput(ID);

        Vector3 force = new Vector3();
        if (transform.position.x < minDist.x)
        {
            force += new Vector3(-1f, 0f, 0f) * (speed / 350) * (transform.position.x + minDist.x); //-x
        }
        if (transform.position.x > maxDist.x)
        {
            force += new Vector3(-1f, 0f, 0f) * (speed / 350) * (transform.position.x - maxDist.x); //x
        }
        if (transform.position.y < minDist.y)
        {
            force += new Vector3(0f, -1f, 0f) * (speed / 350) * (transform.position.y - maxDist.y); //x
        }
        if (transform.position.y > maxDist.y)
        {
            force += new Vector3(0f, -1f, 0f) * (speed / 350) * (transform.position.y - maxDist.y); //x
        }

        if (force != new Vector3())
        {
            body.AddForce(force, ForceMode.Acceleration);//transform.position = targetLoc;
        }

        if (!isAlive)
        {
            verticalAxis = 0;
            horizontalAxis = 0;
        }

        switch (ID)
        {
            default:
            case 0:
                if (verticalAxis > 0.0f)
                {
                    body.AddForce(transform.up * speed * verticalAxis * Time.deltaTime, ForceMode.Acceleration);
                    SetMovement(true);
                }
                else
                {
                    SetMovement(false);
                }
                body.rotation = Quaternion.Euler(body.rotation.eulerAngles + new Vector3(0.0f, 0.0f, Mathf.Deg2Rad * -rotationSpeed * horizontalAxis));
                break;
            case 1:
                if(verticalAxis != 0 || horizontalAxis != 0)
                {
                    Vector3 direct = new Vector3(horizontalAxis, verticalAxis, 0.0f).normalized;
                    body.AddForce(direct * speed * Time.deltaTime, ForceMode.Acceleration);
                    body.rotation = Quaternion.Slerp(body.rotation, Quaternion.LookRotation(new Vector3(0, 0, 1), direct), 0.05f);
                    SetMovement(true);
                }
                else
                {
                    SetMovement(false);
                }
                break;
        }
    }

    private void DeathUpdate()
    {
        GameManager.instance.GetRespawnTimer().EnableTimer(ID, !isAlive);

        immune.SetBool("IsImmune", isInvincible);

        if (m_DeathTimer <= 0.0f && !isAlive)
        {
            isAlive = true;
            m_DeathTimer = 0;
            m_InvincibilityTimer = m_fInvincibilityTime;
            isInvincible = true;

            foreach (var item in GetComponentsInChildren<MeshRenderer>())
            {
                item.enabled = true;
            }

            GetComponentInChildren<MeshCollider>().enabled = true;

            foreach (var item in GetComponentsInChildren<ParticleSystem>())
            {
                item.gameObject.SetActive(true);
            }

            myCamera.ResetCamera();
            //New Guns
            ApplyGun(typeof(BasicGunType));
            ApplyEffect(typeof(BasicShotType));

            Vector3 spawnPos = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f);
            spawnPos = spawnPos.normalized * 25.0f;
            transform.position = spawnPos;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, (transform.position.x < 0 ? 90.0f : -90.0f) + Mathf.Atan(spawnPos.y / spawnPos.x) * Mathf.Rad2Deg);
            body.velocity = Vector3.zero;
        }
        else
        {
            m_DeathTimer -= Time.deltaTime;
            GameManager.instance.GetRespawnTimer().UpdateTimer(ID, m_DeathTimer);
        }

        if (m_InvincibilityTimer <= 0)
        {
            isInvincible = false;
            m_InvincibilityTimer = 0;
        }
        else
        {
            m_InvincibilityTimer -= Time.deltaTime;

        }
        /* This unfreezes rotation when collided which looks good with the miner ship but not the gunner ship as the gunner does not restore their rotation :( */
        //if (!isAlive)
        //{
        //    body.freezeRotation = false;
        //}
        //else
        //{
        //    body.freezeRotation = true;
        //}
    }
    private void PlayerHit()
    {
        if (!isInvincible && isAlive && !shieldObject.IsActive)
        {
            isAlive = false;

            foreach (var item in GetComponentsInChildren<MeshRenderer>())
            {
                item.enabled = false;
            }

            GetComponentInChildren<MeshCollider>().enabled = false;

            foreach (var item in GetComponentsInChildren<ParticleSystem>())
            {
                item.gameObject.SetActive(false);
            }

            m_DeathTimer = m_fRespawnTime;
            myCamera.SetTargetLoc(new Vector3(0.0f, 0.0f, 0.0f));

            GameObject explode = Instantiate(particlePrefab, transform.position, Quaternion.identity);
            explode.transform.localScale = transform.localScale;

            // Force player to stop shooting
            if (InputManager.instance.GetPlayerUnshoot(ID))
            {
                foreach (var gameObject in projectileSpawnLoc)
                {
                    gameObject.GetComponent<GunType>().UnFire();
                }
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isAlive && !shieldObject.IsActive)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Asteroid"))
            {
                PlayerHit();
            }
        }
    }
    public void ApplyGun(System.Type gType)
    {
        if(gType.IsSubclassOf(typeof(GunType)))
        {
            int ammoCount = 0;
            foreach (var gameObject in projectileSpawnLoc)
            {
                Destroy(gameObject.GetComponent<GunType>());
                GunType temp = gameObject.AddComponent(gType) as GunType;
                if (InputManager.instance.GetPlayerShooting(ID) && ID == 1)
                {
                    gameObject.GetComponent<GunType>().Fire(effectType, 0);
                }
                else
                {
                    ammoCount += temp.AmmoCount();
                }
            }

            if(ID == 0)
            {
                maxAmmo = ammoCount;
            }

            gunType = gType;
        }
    }
    public void ApplyEffect(System.Type etype)
    {
        if (etype.IsSubclassOf(typeof(ShotType)))
        {
            effectType = etype;
        }
    }
    
    public void GetPowerUps(out System.Type gun, out System.Type shot)
    {
        gun = gunType;
        shot = effectType;
    }
    public bool CheckAlive()
    {
        return isAlive;
    }
    public void SetInvincibilityTimer(float _time)
    {
        m_InvincibilityTimer = _time;
        isInvincible = true;
    }

    private void SetMovement(bool isMoving)
    {
        foreach (var item in engine)
        {
            item.SetBool("IsMoving", isMoving);
        }
    }
}
