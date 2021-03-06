using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Michael Jordan, Rachael Colaco
/// </summary>
public class PowerUpPickUp : MonoBehaviour
{
    public enum PickUpType { SHOT_BASIC, GUN_BASIC, GUN_SPLIT_THREE, GUN_SPLIT_TWO, SHOT_HOMING, SHOT_PIERCE, SHOT_FROST, AMMO };
    public GameObject imagePlane;
    public PickUpType myType;
    public bool isAmmoDrop = false;
    public Material gunCrate;
    public Material shotCrate;
    public Material ammoCrate;

    public MeshRenderer crate;
    public MeshRenderer miniMap;

    private Rigidbody body;
    private float maxSpeed = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        transform.up = -Vector3.forward;
        body.AddRelativeTorque(new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * 0.5f, ForceMode.Impulse);
        
        if(isAmmoDrop)
        {
            myType = PickUpType.AMMO;
        }
        else
        {
            myType = (PickUpType)Random.Range((int)PickUpType.GUN_SPLIT_THREE, (int)PickUpType.SHOT_FROST + 1);
        }
        
        bool isShot = false;
        bool isAmmo = (myType == PickUpType.AMMO);
        switch (myType)
        {
            case PickUpType.SHOT_BASIC:
                imagePlane.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/BasicShot");
                isShot = true;
                miniMap.material = Resources.Load<Material>("Materials/Powerup");
                break;
            case PickUpType.GUN_BASIC:
                imagePlane.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/BasicGun");
                miniMap.material = Resources.Load<Material>("Materials/Package");
                break;
            case PickUpType.GUN_SPLIT_THREE:
                imagePlane.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/SplitGun");
                miniMap.material = Resources.Load<Material>("Materials/Package");
                break;
            case PickUpType.SHOT_HOMING:
                imagePlane.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/HomingShot");
                isShot = true;
                miniMap.material = Resources.Load<Material>("Materials/Powerup");
                break;
            case PickUpType.GUN_SPLIT_TWO:
                imagePlane.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/DoubleGun");
                miniMap.material = Resources.Load<Material>("Materials/Package");
                break;
            case PickUpType.SHOT_PIERCE:
                imagePlane.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/PercingShot");
                isShot = true;
                miniMap.material = Resources.Load<Material>("Materials/Powerup");
                break;
            case PickUpType.SHOT_FROST:
                imagePlane.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/FreezeShot");
                isShot = true;
                miniMap.material = Resources.Load<Material>("Materials/Powerup");
                break;
            case PickUpType.AMMO:
                imagePlane.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Ammunition");
                isShot = true;
                miniMap.material = Resources.Load<Material>("Materials/Ammo");
                break; 
            default:
                Debug.LogError($"Random Power up got: {(int)myType}");
                break;
        }

        if(isAmmo)
        {
            crate.material = ammoCrate;
            return;
        }

        if(isShot)
        {
            crate.material = shotCrate;
        }
        else
        {
            
            crate.material = gunCrate;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GivePlayerPowerUp(other.gameObject.GetComponentInParent<PlayerController>());
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        ClampSpeed();
    }

    void ClampSpeed()
    {
        if (body.velocity.magnitude > maxSpeed)
        {
            body.velocity = body.velocity.normalized * maxSpeed;
        }
    }

    private void GivePlayerPowerUp(PlayerController player)
    {
        int ammoAdd = 1;

        switch (myType)
        {
            case PickUpType.SHOT_BASIC:
                player.ApplyEffect(typeof(BasicShotType));
                break;
            case PickUpType.GUN_BASIC:
                player.ApplyGun(typeof( BasicGunType));
                break;
            case PickUpType.GUN_SPLIT_THREE:
                player.ApplyGun(typeof( SplitGunType));
                break;
            case PickUpType.GUN_SPLIT_TWO:
                player.ApplyGun(typeof( SplitTwoGunType));
                break;
            case PickUpType.SHOT_HOMING:
                player.ApplyEffect(typeof( HomingShotType));
                break;
            case PickUpType.SHOT_PIERCE:
                player.ApplyEffect(typeof( PierceShotType));
                break;
            case PickUpType.SHOT_FROST:
                player.ApplyEffect(typeof( FrostShotType));
                break;
            case PickUpType.AMMO:
                ammoAdd = 3;
                break;
            default:
                break;
        }

        player.Ammo += ammoAdd;
        if (player.Ammo > player.maxAmmo && player.maxAmmo >= 0)
        {
            player.Ammo = player.maxAmmo;
        }
    }
}
