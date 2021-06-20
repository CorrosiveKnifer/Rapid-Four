﻿using System.Collections;
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

    public float m_maxHealth;
    public float m_maxShields;
    public Vector2 maxDist;
    public Vector2 minDist;

    [Header("Attachments")]
    public GameObject[] projectileSpawnLoc;
    public GameObject noseProjectileSpawnLoc;
    public Shield shieldObject;
    public Animator[] engine;
    public Animator immune;
    public CameraAgent myCamera;
    public bool usingKeyboard = false;

    private GameObject proj;
    private GameObject homing;
    private GameObject blackhole;
    private GameObject energyWave;
    private GameObject beam;

    private GameObject currentBlackhole;
    private GameObject currentLaser;

    [Header("Primary Fire Overheat")]
    public float m_currentHeatLevel = 0.0f;
    public float m_heatPerShot = 5.0f;
    public float m_cooloffRate = 20.0f;
    public bool m_overheated = false;

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
    private bool isAlive = true;
    private float m_fRespawnTime = 10.0f;
    private float m_DeathTimer = 0.0f;
    private bool isInvincible = false;
    private float m_fInvincibilityTime = 2.0f;
    private float m_InvincibilityTimer = 0.0f;

    // Shoot timer
    [Header("Ability Stats")]
    public float m_fFirerate = 3.0f;
    private float m_fShootTimer = 0.0f;

    public float m_fSecondaryFireCD = 5.0f;
    private float m_fSecondaryFireTimer = 0.0f;

    public float m_fAbility1CD = 1.5f;
    private float m_fAbility1Timer = 0.0f;

    public float m_fAbility2CD = 20.0f;
    private float m_fAbility2Timer = 0.0f;

    private float m_health;
    private float m_shields;

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
        BLACKHOLE_PROJECTILE,
    }

    //ControlInput controls;

    // Start is called before the first frame update
    void Start()
    {
        HUDManager.instance.SetHealthMax(ID, m_maxHealth, m_maxShields);

        m_health = m_maxHealth;
        m_shields = m_maxShields;

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
        blackhole = Resources.Load<GameObject>("Prefabs/BlackholeProjectile");
        energyWave = Resources.Load<GameObject>("Prefabs/EnergyWave");
        beam = Resources.Load<GameObject>("Prefabs/ParticleBeam");
        effectType = typeof(BasicShotType);
        ApplyGun(typeof(BasicGunType));

        myCamera = CameraManager.instance.GetCameraAgent(ID);

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

        }

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
            HUDManager.instance.SetHealthDisplay(ID, m_health, m_shields);
        }
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

        Vector2 movement = new Vector2(InputManager.GetInstance().GetHorizontalAxis(InputManager.Joysticks.LEFT, ID), 
            InputManager.GetInstance().GetVerticalAxis(InputManager.Joysticks.LEFT, ID));

        if (!isAlive)
        {
            movement = new Vector2(0, 0);
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
        //if (usingKeyboard) // Mouse aiming
        //{
        //    Vector3 screenPoint = mouse.position.ReadValue();
        //    screenPoint.z = myCamera.GetComponent<Camera>().nearClipPlane;
        //    //Debug.LogWarning(screenPoint);
        //    Vector3 worldPoint = myCamera.GetComponent<Camera>().ScreenToWorldPoint(screenPoint);
        //    //worldPoint.z = gameObject.transform.position.z;
        //    Vector3 direct = worldPoint - gameObject.transform.position;
        //    direct.z = 0;
        //    Quaternion lookDirect = Quaternion.LookRotation(direct, transform.up);
        //    body.rotation = Quaternion.Slerp(body.rotation, lookDirect, rotationSpeed);
        //}
        //else // Joystick aiming
        {
            Vector2 aim = new Vector2(InputManager.GetInstance().GetHorizontalAxis(InputManager.Joysticks.RIGHT, ID, myCamera.GetComponent<Camera>()),
            InputManager.GetInstance().GetVerticalAxis(InputManager.Joysticks.RIGHT, ID, myCamera.GetComponent<Camera>()));

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
        if (m_fSecondaryFireTimer > 0)
            m_fSecondaryFireTimer -= Time.deltaTime;
        if (m_fAbility1Timer > 0)
            m_fAbility1Timer -= Time.deltaTime;
        if (m_fAbility2Timer > 0)
            m_fAbility2Timer -= Time.deltaTime;

        if (currentBlackhole != null && InputManager.GetInstance().GetKeyPressed(InputManager.ButtonType.BUTTON_LS, ID))
        {
           // currentBlackhole.GetComponent<BlackholeProjectile>().ActivateBlackhole();
        }

        if (m_fSecondaryFireTimer <= 0 && InputManager.GetInstance().GetKeyPressed(InputManager.ButtonType.BUTTON_LT, ID) && SecondaryFire != null)
        {
            m_fSecondaryFireTimer = m_fSecondaryFireCD;
            SecondaryFire.Invoke();
        }
        if (m_fAbility1Timer <= 0 && InputManager.GetInstance().GetKeyPressed(InputManager.ButtonType.BUTTON_RS, ID) && Ability1 != null)
        {
            m_fAbility1Timer = m_fAbility1CD;
            Ability1.Invoke();
        }
        if (m_fAbility2Timer <= 0 && InputManager.GetInstance().GetKeyPressed(InputManager.ButtonType.BUTTON_LS, ID) && Ability2 != null)
        {
            m_fAbility2Timer = m_fAbility2CD;
            Ability2.Invoke();
        }

    }

    private void Shoot()
    {
        if (m_fShootTimer > 0)
            m_fShootTimer -= Time.deltaTime;

        if (gamepad.rightTrigger.isPressed && m_fShootTimer <= 0 && !m_overheated)
        {
            m_currentHeatLevel += m_heatPerShot;
            m_fShootTimer = 1.0f / m_fFirerate;

            bool hasShot = false;
            foreach (var gameObject in projectileSpawnLoc)
            {
                hasShot = true;
                GameObject gObject = Instantiate(proj, gameObject.transform.position, Quaternion.identity);
                gObject.transform.up = transform.forward;

                //Send projectile
                gObject.GetComponent<Rigidbody>().AddForce(transform.forward * 50.0f, ForceMode.Impulse);
                //gObject.GetComponent<Rigidbody>().velocity += GetComponent<Rigidbody>().velocity;
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

        // Overheating System
        if (m_currentHeatLevel >= 100.0f)
        {
            m_currentHeatLevel = 100.0f;
            if (!m_overheated)
                audioAgent.PlaySoundEffect("Overheated");
            m_overheated = true;
        }

        if (m_currentHeatLevel > 0 && (!gamepad.rightTrigger.isPressed || m_overheated))
        {
            m_currentHeatLevel -= Time.deltaTime * m_cooloffRate;
        }

        if (m_currentHeatLevel <= 0)
        {
            m_currentHeatLevel = 0.0f;
            if (m_overheated)
                audioAgent.PlaySoundEffect("Empty");
            m_overheated = false;
        }
    }

    public void AbilityHomingMissile()
    {
        // Summon Homing Missile
        GameObject gObject = Instantiate(homing, noseProjectileSpawnLoc.transform.position, Quaternion.identity);
        gObject.transform.up = transform.forward;

        //Send projectile
        gObject.GetComponent<Rigidbody>().AddForce(transform.forward * 50.0f, ForceMode.Impulse);
        audioAgent.PlaySoundEffect("MissileLaunch");
    }

    public void AbilityDash()
    {
        activeEffect newEffect = new activeEffect();
        newEffect.effect = abilityType.DASH;
        newEffect.duration = 0.3f;
        DashDir = MoveDir;
        StoredVelocity = GetComponent<Rigidbody>().velocity;
        playerEffects.Add(newEffect);
        audioAgent.PlaySoundEffect("Dash");
    }

    public void AbilityParticleBeam()
    {
        // IMA FIORIN MAH LAHSOR
        if (currentLaser == null)
        {
            currentLaser = Instantiate(beam, noseProjectileSpawnLoc.transform.position, Quaternion.identity);
            currentLaser.transform.up = transform.forward;

            currentLaser.transform.SetParent(gameObject.transform);

            activeEffect newEffect = new activeEffect();
            newEffect.effect = abilityType.PARTICLE_BEAM;
            newEffect.duration = 1.05f;
            playerEffects.Add(newEffect);
            audioAgent.PlaySoundEffect("BeamCharge");
        }
    }

    public void AbilityEnergyWave()
    {
        // Summon Wave
        GameObject gObject = Instantiate(energyWave, noseProjectileSpawnLoc.transform.position, Quaternion.identity);
        gObject.transform.up = transform.forward;

        gObject.transform.SetParent(gameObject.transform);
        audioAgent.PlaySoundEffect("EnergyWave");
    }

    public void AbilityBlackhole()
    {
        if (currentBlackhole == null)
        {
            // Summon blackhole
            currentBlackhole = Instantiate(blackhole, noseProjectileSpawnLoc.transform.position, Quaternion.identity);
            currentBlackhole.transform.up = transform.forward;

            //Send projectile
            currentBlackhole.GetComponent<Rigidbody>().AddForce(transform.forward * 20.0f, ForceMode.Impulse);

            audioAgent.PlaySoundEffect("BlackholeProj");
        }
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
                        currentLaser.transform.SetParent(null);
                        GetComponent<Rigidbody>().velocity -= transform.forward * 50.0f;
                        audioAgent.StopAudio("BeamCharge");
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
        HUDManager.instance.GetRespawnTimer().EnableTimer(ID, !isAlive);

        immune.SetBool("IsImmune", isInvincible);

        if (m_DeathTimer <= 0.0f && !isAlive)
        {
            isAlive = true;
            m_DeathTimer = 0;
            m_InvincibilityTimer = m_fInvincibilityTime;
            isInvincible = true;
            shieldObject.gameObject.SetActive(true);
            shieldObject.timer = 0.0f;

            if (ID == 0)
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
            HUDManager.instance.GetRespawnTimer().UpdateTimer(ID, m_DeathTimer);
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

    public void DealDamage(float damage)
    {
        //Unsupported ~ Michael
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
        if (gType.IsSubclassOf(typeof(GunType)))
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

            if (ID == 0)
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
