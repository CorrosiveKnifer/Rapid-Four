using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///  William de Beer
/// </summary>
public class Planet : MonoBehaviour
{
    public LevelLoader levelLoader;
    public GameObject vfx;

    public CameraAgent[] playerCamera;

    float m_fMaxHealth = 10000.0f;
    public float m_fHealth;
    public float m_fRotationSpeedMult = 1.0f;
    public GameObject minimapSprite;
    float planetDeathDuration = 1.0f;
    float planetDeathTimer = 1.0f;
    Vector3 planetStartSize;

    private GameObject explode;
    private float m_alertTimer = 0;

    private void Start()
    {
        m_fHealth = m_fMaxHealth;
        planetDeathTimer = planetDeathDuration;
        planetStartSize = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_alertTimer >= 0)
        {
            m_alertTimer -= Time.deltaTime;
        }

        HUDManager.instance.SetPlanetHPAnim(m_alertTimer > 0.0f);

        // Debug kaboom
        //if (Input.GetKeyDown(KeyCode.G))
        // {
        //  m_fHealth = 0;
        // }

        transform.Rotate(new Vector3(0.0f, m_fRotationSpeedMult * Time.deltaTime, 0.0f));
        HUDManager.instance.SetPlanetHP(m_fMaxHealth, m_fHealth);
        minimapSprite.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        if (m_fHealth <= 0.0f)
        {
            foreach (var cam in playerCamera)
            {
                cam.SetTargetLoc(new Vector3(0.0f, 0.0f, -50.0f), true, 0.25f);
            }
            
            planetDeathTimer -= Time.deltaTime * 2.0f;
            float scaleMult = planetDeathTimer / planetDeathDuration;

            if (scaleMult >= 0.00f)
            {
                transform.localScale = Vector3.Lerp(new Vector3(0.1f, 0.1f, 0.1f), planetStartSize, scaleMult);
            }

            if(scaleMult <= 0.85f)
            {
                GetComponent<MeshRenderer>().enabled = scaleMult <= 0.8f;

                if (explode == null)
                {
                    explode = Instantiate(vfx, minimapSprite.transform.position, Quaternion.identity);
                    explode.transform.localScale = transform.localScale;
                }
            }

            if (scaleMult <= -3.0f)
            {
                levelLoader.LoadNextLevel();
                Destroy(this);
            }
        }
    }

    public void DealDamage(float _damage)
    {
        m_fHealth -= _damage;
        m_alertTimer = 1.0f;
        if (GetComponent<AudioAgent>().IsAudioStopped("PlanetAttacked"))
        {
            GetComponent<AudioAgent>().PlaySoundEffect("PlanetAttacked");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Asteroid"))
        {
            Astroid asteroid = other.gameObject.GetComponentInParent<Astroid>();
            float size = asteroid.Endurance;
            if (asteroid != null)
            {
                m_fHealth -= (int)(Mathf.Pow(asteroid.Endurance + 1, 2) * 20.0f);
                Destroy(other.gameObject);
            }

            foreach (var item in GameObject.FindGameObjectsWithTag("Camera"))
            {
                item.GetComponent<CameraAgent>().Shake(2.0f * ((size + 1f) / 2f));
            }
            GetComponent<AudioAgent>().PlaySoundEffect("AsteroidPlanetColl");
        }

        if (other.gameObject.tag == "PowerUp")
        {
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Projectile")
        {
            Instantiate(Resources.Load<GameObject>("VFX/RockHit"), other.gameObject.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
    }
}
