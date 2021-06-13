using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using PowerUp;
using UnityEngine.InputSystem;

/// <summary>
/// Michael Jordan, William de Beer
/// </summary>
public class PlayerController : MonoBehaviour
{
    public int ID;
    public int maxAmmo = 20;
    public int Ammo;

    public Vector2 maxDist;
    public Vector2 minDist;

    [Header("Attachments")]
    public GameObject[] projectileSpawnLoc;
    public Shield shieldObject;
    public Animator[] engine;
    public Animator immune;
    public CameraAgent myCamera;
    public bool usingKeyboard = false;
    private GameObject proj;
    private GameObject homing;
    private GameObject beam;

    [Header("Abilities")]
    public UnityEvent SecondaryFire;
    public UnityEvent Ability1;
    public UnityEvent Ability2;

    private Vector3 DashDir;
    private Vector3 MoveDir;
    private Vector3 StoredVelocity;

    private System.Type effectType;
    private System.Type gunType;
    private Rigidbody body;
    private int controlsID;

    public float speed = 350.0f;
    [Range(0.0f, 1.0f)]
    public float rotationSpeed = 0.1f;

    public GameObject particlePrefab;

    AudioAgent audioAgent;

    // Death stuff
    bool isAlive = true;
    float m_fRespawnTime = 10.0f;
    float m_DeathTimer = 0.0f;
    bool isInvincible = false;
    float m_fInvincibilityTime = 2.0f;
    float m_InvincibilityTimer = 0.0f;

    List<activeEffect> playerEffects = new List<activeEffect>();

    [Header("Input Devices")]
    Gamepad gamepad;
    Keyboard keyboard;
    Mouse mouse;

    class activeEffect
    {
        public abilityType effect;
        public float duration;

        public bool UpdateDuration() 
        {
            duration -= Time.deltaTime;
            return (duration <= 0.0f); 
        }
    }
    enum abilityType
    {
        DASH,
        PARTICLE_BEAM,
    }

    //ControlInput controls;

    // Start is called before the first frame update
    void Start()
    {
        if (ID == 0)
            controlsID = GameManager.player1Controls;
        if (ID == 1)
            controlsID = GameManager.player2Controls;

        if (usingKeyboard)
        {
            keyboard = Keyboard.current;
            mouse = Mouse.current;
        }
        else if (Gamepad.all.Count != 0)
        {
            var allGamepads = Gamepad.all;
            gamepad = allGamepads[controlsID];
        }

        audioAgent = GetComponent<AudioAgent>();
        shieldObject = GetComponentInChildren<Shield>();
        body = GetComponentInChildren<Rigidbody>();
        proj = Resources.Load<GameObject>("Prefabs/BasicShot");
        homing = Resources.Load<GameObject>("Prefabs/HomingShot");
        beam = Resources.Load<GameObject>("Prefabs/ParticleBeam");
        effectType = typeof(BasicShotType);
        ApplyGun(typeof(BasicGunType));
        
        Ammo = (maxAmmo > 0) ? maxAmmo : 0;
    }
    private void Awake()
    {
        body = GetComponent<Rigidbody>();

        if (usingKeyboard)
        {
            keyboard = Keyboard.current;
            mouse = Mouse.current;
        }
        else if (Gamepad.all.Count != 0)
        {
            var allGamepads = Gamepad.all;
            gamepad = allGamepads[ID];

            //float here = allGamepads.Count;
            //Debug.Log(player1);
        }

        //controls = new ControlInput();

        ////controls.Gameplay.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>();
        ////controls.Gameplay.Rotate.canceled += ctx => rotate = Vector2.zero;

        //controls.Gameplay.Shoot.performed += ctx => Shooting();

        //Debug.Log("Shoot");
    }
    private void OnEnable()
    {
       // controls.Gameplay.Enable();
    }
    private void OnDisable()
    {
       // controls.Gameplay.Disable();
    }
    /// <summary>
    /// this activate when right trigger is pressed
    /// </summary>
    public void Shooting()
    {
        Debug.Log("Shoot");
    }
    // Update is called once per frame
    void Update()
    {
        if (Ammo > maxAmmo && maxAmmo > 0)
            Ammo = maxAmmo;

        if (isAlive)
        {
            Shoot();
            Ability();
            EffectUpdate();
        }
        //DeathUpdate();
            /*
            if(Gamepad.current.buttonSouth.wasPressedThisFrame)
            { 
            }
            */
            /*
            if (InputManager.instance.GetPlayerShoot(ID))
            {
                bool hasShot = false;
                foreach (var gameObject in projectileSpawnLoc)
                {
                    if (Ammo > 0 || maxAmmo < 0)
                    {
                        int cost = gameObject.GetComponent<GunType>().ammoCost;
                        if (Ammo < gameObject.GetComponent<GunType>().ammoCost && maxAmmo > 0)
                        {
                            cost = Ammo;
                        }
                        hasShot = true;
                        gameObject.GetComponent<GunType>().Fire(effectType, cost);
                        
                        if (maxAmmo > 0)
                            Ammo = Mathf.Clamp(Ammo - cost, 0, 100);

                    }
                }

                if(hasShot && ID == 0)
                {
                    audioAgent.PlaySoundEffect("ShootPew");
                }
                else if (ID == 0)
                {
                    audioAgent.PlaySoundEffect("Empty");
                }
                else if(ID == 1)
                {
                    audioAgent.PlaySoundEffect("Laser");
                }
                
            }
            */
        /*

        if(InputManager.instance.GetPlayerUnshoot(ID))
        {
            foreach (var gameObject in projectileSpawnLoc)
            {
                gameObject.GetComponent<GunType>().UnFire();
                audioAgent.StopAudio("Laser");
            }
        }
        */
    }

    private void FixedUpdate()
    {
        Bounds();
        ShipMovement();
        ShipAiming();
    }
    private void ShipMovement()
    {
        //float verticalAxis = InputManager.instance.GetVerticalInput(ID);
        //float horizontalAxis = InputManager.instance.GetHorizontalInput(ID);
        
        Vector2 movement = gamepad.leftStick.ReadValue();

        if (!isAlive)
        {
            movement = new Vector2(0,0);
        }
        if (movement.x != 0 || movement.y != 0) // Move
        {
            Vector3 direct = new Vector3(movement.x, movement.y, 0.0f).normalized;
            body.AddForce(direct * speed * Time.deltaTime, ForceMode.Acceleration);
            SetMovement(true);
            MoveDir = direct;
        }
        else
        {
            SetMovement(false);
            MoveDir = transform.forward;
        }
    }
    private void ShipAiming()
    {
        if (usingKeyboard) // Mouse aiming
        {
            
            Vector3 screenPoint = mouse.position.ReadValue();
            screenPoint.z = myCamera.GetComponent<Camera>().nearClipPlane;
            //Debug.LogWarning(screenPoint);
            Vector3 worldPoint = myCamera.GetComponent<Camera>().ScreenToWorldPoint(screenPoint);
            //worldPoint.z = gameObject.transform.position.z;
            Vector3 direct = worldPoint - gameObject.transform.position;
            direct.z = 0;
            Quaternion lookDirect = Quaternion.LookRotation(direct, transform.up);
            body.rotation = Quaternion.Slerp(body.rotation, lookDirect, rotationSpeed);
        }
        else // Joystick aiming
        {
            float verticalAxis = 0; //InputManager.instance.GetVerticalInput(1);
            float horizontalAxis = 0; //InputManager.instance.GetHorizontalInput(1);

            Vector2 aim = gamepad.rightStick.ReadValue();

            if (aim.x != 0 || aim.y != 0)
            {
                Vector3 direct = new Vector3(aim.x, aim.y, 0.0f).normalized;
                body.rotation = Quaternion.Slerp(body.rotation, Quaternion.LookRotation(direct, transform.up), rotationSpeed);
            }
        }
    }
    private void Bounds()
    {
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
    }

    private void Ability()
    {
        // false is temp
        //if (Input.GetKeyDown(KeyCode.E) && SecondaryFire != null)
        //{
        //    SecondaryFire.Invoke();
        //}
        //if (Input.GetKeyDown(KeyCode.LeftControl) && Ability1 != null)
        //{
        //    Ability1.Invoke();
        //}
        //if (Input.GetKeyDown(KeyCode.Q) && Ability2 != null)
        //{
        //    Ability2.Invoke();
        //}
    }

    private void Shoot()
    {
        if (false/*InputManager.instance.GetPlayerShoot(ID)*/)
        {
            bool hasShot = false;
            foreach (var gameObject in projectileSpawnLoc)
            {
                hasShot = true; 
                GameObject gObject = Instantiate(proj, transform.position, Quaternion.identity);
                gObject.transform.up = transform.forward;

                //Send projectile
                gObject.GetComponent<Rigidbody>().AddForce(transform.forward * 50.0f, ForceMode.Impulse);
            }

            if (hasShot)
            {
                audioAgent.PlaySoundEffect("ShootPew");
            }
            else
            {
                audioAgent.PlaySoundEffect("Empty");
            }
        }

        //if (InputManager.instance.GetPlayerUnshoot(ID))
        //{
        //    foreach (var gameObject in projectileSpawnLoc)
        //    {
        //        gameObject.GetComponent<GunType>().UnFire();
        //        audioAgent.StopAudio("Laser");
        //    }
        //}
    }

    public void AbilityHomingMissile()
    {
        // Summon Homing Missile
        GameObject gObject = Instantiate(homing, transform.position, Quaternion.identity);
        gObject.transform.up = transform.forward;

        //Send projectile
        gObject.GetComponent<Rigidbody>().AddForce(transform.forward * 50.0f, ForceMode.Impulse);
    }

    public void AbilityDash()
    {
        activeEffect newEffect = new activeEffect();
        newEffect.effect = abilityType.DASH;
        newEffect.duration = 0.2f;
        DashDir = MoveDir;
        StoredVelocity = GetComponent<Rigidbody>().velocity;
        playerEffects.Add(newEffect);
        audioAgent.PlaySoundEffect("Dash");
    }

    public void AbilityParticleBeam()
    {
        // IMA FIORIN MAH LAHSOR
        activeEffect newEffect = new activeEffect();
        newEffect.effect = abilityType.PARTICLE_BEAM;
        newEffect.duration = 2.0f;
        playerEffects.Add(newEffect);
        audioAgent.PlaySoundEffect("BeamCharge");
    }

    private void EffectUpdate()
    {
        List<activeEffect> removeList = new List<activeEffect>();
        foreach (var item in playerEffects)
        {
            switch (item.effect) // During effect
            {
                case abilityType.DASH:
                    GetComponent<Rigidbody>().velocity = DashDir * 150.0f;
                    break;
                case abilityType.PARTICLE_BEAM:
                    GetComponent<Rigidbody>().velocity -= GetComponent<Rigidbody>().velocity * Time.deltaTime * 3.0f;
                    break;
                default:
                    break;
            }
            if (item.UpdateDuration()) 
            {
                switch (item.effect) // Finish effect
                {
                    case abilityType.DASH:
                        GetComponent<Rigidbody>().velocity = MoveDir * 20.0f;
                        break;
                    case abilityType.PARTICLE_BEAM:
                        GameObject gObject = Instantiate(beam, transform.position, Quaternion.identity);
                        gObject.transform.up = transform.forward;
                        GetComponent<Rigidbody>().velocity -= transform.forward * 100.0f;
                        audioAgent.PlaySoundEffect("BeamRelease");
                        //gObject.transform.rotation = transform.rotation;
                        break;
                    default:
                        break;
                }
                removeList.Add(item);
            }
        }
        foreach (var item in removeList)
        {
            playerEffects.Remove(item);
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
            shieldObject.gameObject.SetActive(true);
            shieldObject.timer = 0.0f;

            if(ID == 0)
                Ammo = maxAmmo;

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
    }
    private void PlayerHit()
    {
        if (!isInvincible && isAlive && !shieldObject.IsActive)
        {
            isAlive = false;
            shieldObject.gameObject.SetActive(false);
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
            myCamera.SetTargetLoc(new Vector3(0.0f, 0.0f, -45.0f));

            GameObject explode = Instantiate(particlePrefab, transform.position, Quaternion.identity);
            explode.transform.localScale = transform.localScale;

            /*
            // Force player to stop shooting
            if (InputManager.instance.GetPlayerUnshoot(ID))
            {
                foreach (var gameObject in projectileSpawnLoc)
                {
                    gameObject.GetComponent<GunType>().UnFire();
                }
            }
            */
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
            audioAgent.PlaySoundEffect("Pickup" + Random.Range(1, 5));

            int ammoCount = 0;
            foreach (var gameObject in projectileSpawnLoc)
            {
                Destroy(gameObject.GetComponent<GunType>());
                GunType temp = gameObject.AddComponent(gType) as GunType;
                /*
                if (InputManager.instance.GetPlayerShooting(ID) && ID == 1)
                {
                    gameObject.GetComponent<GunType>().Fire(effectType, 0);
                }
                else
                {
                    ammoCount += temp.AmmoCount();
                }
                */
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
            audioAgent.PlaySoundEffect("Pickup" + Random.Range(1, 5));
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
    private void SetMovement(bool isMoving, float speed = 1.0f)
    {
        foreach (var item in engine)
        {
            item.SetBool("IsMoving", isMoving);
            item.SetFloat("Speed", speed);
        }
        if (isMoving && audioAgent.IsAudioStopped("Thruster1"))
        {
            audioAgent.PlaySoundEffect("Thruster1", true);
        }
        else if (!isMoving && !audioAgent.IsAudioStopped("Thruster1"))
        {
            audioAgent.StopAudio("Thruster1");
        }
    }
}
