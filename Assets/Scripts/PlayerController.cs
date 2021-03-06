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

    [Header("Player Health")]
    public float m_maxHealth;
    public float m_maxShields;
    private float m_health;
    private float m_shields;

    public float m_shieldRegenRate = 20.0f;
    public float m_shieldRegenDelay = 3.0f;
    private float m_shieldRegenTimer = 0.0f;

    private bool isPlayerMoving = false;

    public Vector2 maxDist;
    public Vector2 minDist;
    public GameObject hitVFX;

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
    private GameObject decoy;

    private GameObject currentBlackhole;
    private GameObject currentDecoy;
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


    List<activeEffect> playerEffects = new List<activeEffect>();

    [Header("Ability Thumbnails")]
    public Sprite abilityThumbnail1;
    public Sprite abilityThumbnail2;
    public Sprite abilityThumbnail3;

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


        audioAgent = GetComponent<AudioAgent>();
        shieldObject = GetComponentInChildren<Shield>();
        body = GetComponentInChildren<Rigidbody>();

        if (InputManager.GetInstance().GetPlayerControl(ID).shipID == 0)
        {
            proj = Resources.Load<GameObject>("Prefabs/BasicShot");
        }
        if (InputManager.GetInstance().GetPlayerControl(ID).shipID == 1)
        {
            proj = Resources.Load<GameObject>("Prefabs/BasicShot2");
        }

        if (ID == 0)
        {
            HUDManager.instance.player1Ability[0].sprite = abilityThumbnail1;
            HUDManager.instance.player1Ability[1].sprite = abilityThumbnail2;
            HUDManager.instance.player1Ability[2].sprite = abilityThumbnail3;
        }
        if (ID == 1)
        {
            HUDManager.instance.player2Ability[0].sprite = abilityThumbnail1;
            HUDManager.instance.player2Ability[1].sprite = abilityThumbnail2;
            HUDManager.instance.player2Ability[2].sprite = abilityThumbnail3;
        }


        homing = Resources.Load<GameObject>("Prefabs/HomingShot");
        blackhole = Resources.Load<GameObject>("Prefabs/BlackholeProjectile");
        energyWave = Resources.Load<GameObject>("Prefabs/EnergyWave");
        beam = Resources.Load<GameObject>("Prefabs/ParticleBeam");
        decoy = Resources.Load<GameObject>("Prefabs/Decoy");
        effectType = typeof(BasicShotType);
        ApplyGun(typeof(BasicGunType));

        myCamera = CameraManager.instance.GetCameraAgent(ID);

        Ammo = (maxAmmo > 0) ? maxAmmo : 0;
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody>();

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

        // Shield regeneration
        if (isAlive)
        {

            HUDManager.instance.SetCooldown(ID, 0, m_fSecondaryFireTimer, m_fSecondaryFireCD);
            HUDManager.instance.SetCooldown(ID, 1, m_fAbility1Timer, m_fAbility1CD);
            HUDManager.instance.SetCooldown(ID, 2, m_fAbility2Timer, m_fAbility2CD);

            if (m_shieldRegenTimer <= 0.0f)
            {
                if (m_shields < m_maxShields)
                {
                    m_shields += m_shieldRegenRate * Time.deltaTime;
                }
                else
                {
                    m_shields = m_maxShields;
                }
            }
            else
            {
                m_shieldRegenTimer -= Time.deltaTime;
            }
        }
        // Shield visuals
        if (m_shields > 0.0f && isAlive)
        {
            shieldObject.gameObject.SetActive(true);
        }
        else
        {
            shieldObject.gameObject.SetActive(false);
        }

        if (isAlive)
        {
            Shoot();
            Ability();
            EffectUpdate();
        }
        HUDManager.instance.SetHealthDisplay(ID, m_health, m_shields, m_currentHeatLevel);
        DeathUpdate();
    }

    private void FixedUpdate()
    {
        if (isAlive)
        {
            Bounds();
            ShipMovement();
            ShipAiming();
        }
    }

    /// <summary>
    /// Player movement
    /// </summary>
    private void ShipMovement()
    {
        // Get movement vector
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
    /// <summary>
    /// Player aiming
    /// </summary>
    private void ShipAiming()
    {
        // Get aiming axis
        Vector2 aim = new Vector2(InputManager.GetInstance().GetHorizontalAxis(InputManager.Joysticks.RIGHT, ID, myCamera.GetComponent<Camera>()),
        InputManager.GetInstance().GetVerticalAxis(InputManager.Joysticks.RIGHT, ID, myCamera.GetComponent<Camera>()));

        Vector3 cameraPos = transform.position;
        cameraPos.z = -45.0f;

        Debug.Log(isPlayerMoving);
        if ((aim.x != 0 || aim.y != 0))
        {
            Vector3 direct = new Vector3(aim.x, aim.y, 0.0f);
            if (!isPlayerMoving) // Whiling idle and aiming
                myCamera.SetTargetLoc(cameraPos + direct * 25.0f, false, 0.1f);
            else // While moving and aiming
                myCamera.SetTargetLoc(cameraPos, false, 0.1f);

            body.rotation = Quaternion.Slerp(body.rotation, Quaternion.LookRotation(direct.normalized, transform.up), rotationSpeed);
        }
        else
        {
            if (!isPlayerMoving) // While idle
                myCamera.SetTargetLoc(cameraPos, false , 0.3f);
            else // While moving
                myCamera.SetTargetLoc(cameraPos, false, 0.3f);
        }
    }

    /// <summary>
    /// Forces player back into bounds/play area
    /// </summary>
    private void Bounds()
    {
        //Vector3 force = new Vector3();
        //if (transform.position.x < minDist.x)
        //{
        //    force += new Vector3(-1f, 0f, 0f) * (speed / 350) * (transform.position.x + minDist.x); //-x
        //}
        //if (transform.position.x > maxDist.x)
        //{
        //    force += new Vector3(-1f, 0f, 0f) * (speed / 350) * (transform.position.x - maxDist.x); //x
        //}
        //if (transform.position.y < minDist.y)
        //{
        //    force += new Vector3(0f, -1f, 0f) * (speed / 350) * (transform.position.y - maxDist.y); //x
        //}
        //if (transform.position.y > maxDist.y)
        //{
        //    force += new Vector3(0f, -1f, 0f) * (speed / 350) * (transform.position.y - maxDist.y); //x
        //}
        //
        //if (force != new Vector3())
        //{
        //    body.AddForce(force, ForceMode.Acceleration);//transform.position = targetLoc;
        //}
    }

    /// <summary>
    /// Calls player abilities
    /// </summary>
    private void Ability()
    {
        if (m_fSecondaryFireTimer > 0)
            m_fSecondaryFireTimer -= Time.deltaTime;
        if (m_fAbility1Timer > 0)
            m_fAbility1Timer -= Time.deltaTime;
        if (m_fAbility2Timer > 0)
            m_fAbility2Timer -= Time.deltaTime;

        if (currentBlackhole != null && InputManager.GetInstance().GetKeyDown(InputManager.ButtonType.BUTTON_LS, ID))
        {
            if (currentBlackhole.GetComponent<BlackholeProjectile>())
                currentBlackhole.GetComponent<BlackholeProjectile>().ActivateBlackhole();
            else
                Debug.LogError("Incorrect ability saved to location or some other error");
        }
        if (currentDecoy != null && InputManager.GetInstance().GetKeyDown(InputManager.ButtonType.BUTTON_RS, ID))
        {
            if (currentDecoy.GetComponent<Decoy>())
                currentDecoy.GetComponent<Decoy>().ActivateDecoy();
            else
                Debug.LogError("Incorrect ability saved to location or some other error");
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

    /// <summary>
    /// Player primary fire that fires a projectile for each projectile spawn location it has
    /// </summary>
    private void Shoot()
    {
        if (m_fShootTimer > 0)
            m_fShootTimer -= Time.deltaTime;

        if (InputManager.GetInstance().GetKeyPressed(InputManager.ButtonType.BUTTON_RT, ID) && m_fShootTimer <= 0 && !m_overheated)
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
                gObject.GetComponent<Rigidbody>().AddForce(transform.forward * 100.0f, ForceMode.Impulse);

                if (gObject.GetComponent<BasicShotType>())
                    gObject.GetComponent<BasicShotType>().playerID = (uint)ID;
            }

            if (hasShot)
            {
                
                audioAgent.PlaySoundEffect("ShootPew", false, 255, Random.Range(0.75f, 1.25f));
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

        if (m_currentHeatLevel > 0 && (!InputManager.GetInstance().GetKeyPressed(InputManager.ButtonType.BUTTON_RT, ID) || m_overheated))
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

    /// <summary>
    /// Fire homing missile which will seek the nearest target.
    /// </summary>
    public void AbilityHomingMissile()
    {
        // Summon Homing Missile
        GameObject gObject = Instantiate(homing, noseProjectileSpawnLoc.transform.position, Quaternion.identity);
        gObject.transform.up = transform.forward;

        //Send projectile
        gObject.GetComponent<Rigidbody>().AddForce(transform.forward * 50.0f, ForceMode.Impulse);
        audioAgent.PlaySoundEffect("MissileLaunch2");

        if (gObject.GetComponent<HomingShotType>())
            gObject.GetComponent<HomingShotType>().playerID = (uint)ID;
    }

    /// <summary>
    /// Allows the player to quick dash is the direction their movement to quickly reposition,
    /// </summary>
    public void AbilityDash()
    {
        activeEffect newEffect = new activeEffect();
        newEffect.effect = abilityType.DASH;
        newEffect.duration = 0.3f;
        DashDir = MoveDir;
        StoredVelocity = GetComponent<Rigidbody>().velocity;
        playerEffects.Add(newEffect);
        audioAgent.PlaySoundEffect("Dash");

        // Particles
        GameObject gParticles = Instantiate(Resources.Load<GameObject>("VFX/DashVFXObject"), transform.position, Quaternion.identity);
        gParticles.transform.up = transform.forward;
        gParticles.transform.SetParent(gameObject.transform);

        // Trail
        //foreach (var gameObject in engine)
        {
            GameObject gTrail = Instantiate(Resources.Load<GameObject>("VFX/DashTrailVFX"), gameObject.transform.position, Quaternion.identity);
            gTrail.transform.up = transform.forward;
            gTrail.transform.SetParent(gameObject.transform);
        }
    }
        /// <summary>
        /// Create particle beam
        /// </summary>
    public void AbilityParticleBeam()
    {
        // IMA FIORIN MAH LAHSOR
        if (currentLaser == null)
        {
            currentLaser = Instantiate(beam, noseProjectileSpawnLoc.transform.position, Quaternion.identity);
            currentLaser.transform.up = transform.forward;

            // Attach laser to player
            currentLaser.transform.SetParent(gameObject.transform);

            activeEffect newEffect = new activeEffect();
            newEffect.effect = abilityType.PARTICLE_BEAM;
            newEffect.duration = 1.05f;
            playerEffects.Add(newEffect);
            audioAgent.PlaySoundEffect("BeamCharge");

            if (currentLaser.GetComponent<ParticleBeam>())
                currentLaser.GetComponent<ParticleBeam>().playerID = (uint)ID;
        }
    }

    /// <summary>
    /// Create energy wave that launches enemies backwards, dealing damage and healing allies.
    /// </summary>
    public void AbilityEnergyWave()
    {
        // Summon Wave
        GameObject gObject = Instantiate(energyWave, noseProjectileSpawnLoc.transform.position, Quaternion.identity);
        gObject.transform.up = transform.forward;

        gObject.transform.SetParent(gameObject.transform);
        audioAgent.PlaySoundEffect("EnergyWave2");

        if (gObject.GetComponent<EnergyWave>())
            gObject.GetComponent<EnergyWave>().playerID = (uint)ID;
    }
    /// <summary>
    /// Summon projectile which will summon blackhole after short period of time
    /// </summary>
    public void AbilityBlackhole()
    {
        if (currentBlackhole == null)
        {
            // Summon blackhole
            currentBlackhole = Instantiate(blackhole, noseProjectileSpawnLoc.transform.position, Quaternion.identity);
            currentBlackhole.transform.up = transform.forward;

            //Send projectile
            currentBlackhole.GetComponent<Rigidbody>().AddForce(transform.forward * 30.0f, ForceMode.Impulse);

            audioAgent.PlaySoundEffect("BlackholeProj");
        }
    }

    public void AbilityDecoy()
    {
        if (currentDecoy == null)
        {
            // Summon blackhole
            currentDecoy = Instantiate(decoy, noseProjectileSpawnLoc.transform.position, Quaternion.identity);
            currentDecoy.transform.up = transform.forward;

            //Send projectile
            currentDecoy.GetComponent<Rigidbody>().AddForce(transform.forward * 30.0f, ForceMode.Impulse);

            //audioAgent.PlaySoundEffect("BlackholeProj");
            if (currentDecoy.GetComponent<Decoy>())
                currentDecoy.GetComponent<Decoy>().playerID = (uint)ID;
        }
    }

    /// <summary>
    /// Updaet currently active effects
    /// </summary>
    private void EffectUpdate()
    {
        List<activeEffect> removeList = new List<activeEffect>();
        foreach (var item in playerEffects)
        {
            switch (item.effect) // During effect
            {
                case abilityType.DASH: // Dash
                    GetComponent<Rigidbody>().velocity = DashDir * 150.0f;
                    break;
                case abilityType.PARTICLE_BEAM: // Beam self slow
                    GetComponent<Rigidbody>().velocity -= GetComponent<Rigidbody>().velocity * Time.deltaTime * 3.0f;
                    break;
                default:
                    break;
            }
            if (item.UpdateDuration())
            {
                switch (item.effect) // Finish effect
                {
                    case abilityType.DASH: // Recover speed from dash
                        GetComponent<Rigidbody>().velocity = MoveDir * 20.0f;
                        break;
                    case abilityType.PARTICLE_BEAM: 
                        // Unattach from parents
                        currentLaser.transform.SetParent(null);
                        // Launch player backwards
                        GetComponent<Rigidbody>().velocity -= transform.forward * 50.0f;
                        audioAgent.StopAudio("BeamCharge");
                        audioAgent.PlaySoundEffect("BeamRelease");
                        break;
                    default:
                        break;
                }
                removeList.Add(item); // Add effects to be removed
            }
        }
        foreach (var item in removeList)
        {
            playerEffects.Remove(item); // Remove effects
        }
    }

    /// <summary>
    ///  Update player death
    /// </summary>
    private void DeathUpdate()
    {
        HUDManager.instance.GetRespawnTimer().EnableTimer(ID, !isAlive);

       // immune.SetBool("IsImmune", isInvincible);

        if (m_DeathTimer <= 0.0f && !isAlive)
        {
            isAlive = true;
            m_DeathTimer = 0;
            m_InvincibilityTimer = m_fInvincibilityTime;
            isInvincible = true;

            m_health = m_maxHealth;
            m_shields = m_maxShields;

            shieldObject.gameObject.SetActive(true);

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

            Vector3 spawnPos = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f);
            if (spawnPos == new Vector3(0.0f, 0.0f, 0.0f))
                spawnPos = new Vector3(0.0f, 1.0f, 0.0f);
            spawnPos = spawnPos.normalized * 75.0f;
            transform.position = spawnPos;
            //transform.rotation = Quaternion.Euler(0.0f, 0.0f, (transform.position.x < 0 ? 90.0f : -90.0f) + Mathf.Atan(spawnPos.y / spawnPos.x) * Mathf.Rad2Deg);
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
        isPlayerMoving = isMoving;
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
    /// <summary>
    ///  Deal damage to the player when the have been hit
    /// </summary>
    /// <param name="damage"></param>
    public void DealDamage(float damage, Vector3 pos = default(Vector3))
    {
        m_shieldRegenTimer = m_shieldRegenDelay;
        if (m_shields > 0.0f)
        {
            m_shields -= damage;
            if (m_shields < 0.0f)
            {
                // Move negative shields into health
                m_health += m_shields;
                m_shields = 0.0f;
                if (pos != default(Vector3))
                {
                    Vector3 direct = (pos - transform.position).normalized;
                    GameObject hit = Instantiate(hitVFX, transform.position + direct * 1.5f, Quaternion.LookRotation(direct));
                    hit.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
                }
            }
        }
        else 
        {
            m_health -= damage;
            if (pos != default(Vector3))
            {
                Vector3 direct = (pos - transform.position).normalized;
                GameObject hit = Instantiate(hitVFX, transform.position + direct * 1.5f, Quaternion.LookRotation(direct));
                hit.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
            }
            if (m_health < 0.0f)
            {
                // Kill player
                m_health = 0.0f;
                KillPlayer();
            }
        }
        
    }
    /// <summary>
    /// Heal the player for an amount
    /// </summary>
    /// <param name="heal"></param>
    public void DealHeal(float heal)
    {
        if (m_health < m_maxHealth)
        {
            m_health += heal;
        }
    }

    /// <summary>
    /// Kill player
    /// </summary>
    private void KillPlayer()
    {
        if (!isInvincible && isAlive)
        {
            isAlive = false;
            GameManager.Deaths[ID]++;
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

        }
    }
    private void OnCollisionEnter(Collision other)
    {
        //if (isAlive && !shieldObject.IsActive)
        //{
        //    if (other.gameObject.layer == LayerMask.NameToLayer("Asteroid"))
        //    {
        //        PlayerHit();
        //    }
        //}
    }

    /// <summary>
    /// No longer needed
    /// </summary>
    /// <param name="gType"></param>
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
                
            }

            if (ID == 0)
            {
                maxAmmo = ammoCount;
            }

            gunType = gType;
        }
    }
    /// <summary>
    /// No longer needed
    /// </summary>
    /// <param name="etype"></param>
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
    
}
