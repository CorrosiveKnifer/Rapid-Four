using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPickUp : MonoBehaviour
{
    public enum PickUpType { SHOT_BASIC, GUN_BASIC, GUN_SPLIT_THREE, GUN_SPLIT_TWO, SHOT_HOMING, GUN_PIERCE, SHOT_PIERCE };
    public GameObject imagePlane;
    public PickUpType myType;

    // Start is called before the first frame update
    void Start()
    {
        myType = (PickUpType)Random.Range((int)PickUpType.GUN_SPLIT_THREE, (int)PickUpType.SHOT_PIERCE + 1);
        
        switch (myType)
        {
            case PickUpType.SHOT_BASIC:
                imagePlane.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/BasicShot");
                break;
            case PickUpType.GUN_BASIC:
                imagePlane.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/BasicGun");
                break;
            case PickUpType.GUN_SPLIT_THREE:
                imagePlane.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/SplitGun");
                break;
            case PickUpType.SHOT_HOMING:
                imagePlane.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/HomingShot");
                break; 
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GivePlayerPowerUp(other.gameObject.GetComponentInParent<PlayerController>());
            GameManager.instance.PlayPowerUp();
            Destroy(gameObject);
        }
    }

    private void GivePlayerPowerUp(PlayerController player)
    {
        switch (myType)
        {
            case PickUpType.SHOT_BASIC:
                player.ApplyEffect(new BasicShotType());
                break;
            case PickUpType.GUN_BASIC:
                player.ApplyGun(new BasicGunType());
                break;
            case PickUpType.GUN_SPLIT_THREE:
                player.ApplyGun(new SplitGunType());
                break;
            case PickUpType.GUN_SPLIT_TWO:
                player.ApplyGun(new SplitTwoGunType());
                break;
            case PickUpType.SHOT_HOMING:
                player.ApplyEffect(new HomingShotType());
                break;
            case PickUpType.GUN_PIERCE:
                player.ApplyGun(new PierceGunType());
                break;
            case PickUpType.SHOT_PIERCE:
                player.ApplyEffect(new PierceShotType());
                break;
            default:
                break;
        }
    }
}
