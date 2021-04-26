using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPickUp : MonoBehaviour
{
    public enum PickUpType { SHOT_BASIC, GUN_BASIC, GUN_SPLIT_THREE, GUN_SPLIT_TWO, SHOT_HOMING, GUN_PIERCE, SHOT_PIERCE };
    
    public PickUpType myType;

    // Start is called before the first frame update
    void Start()
    {
        
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
                player.type = new BasicShotType();
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
                player.type = new HomingShotType();
                break;
            case PickUpType.GUN_PIERCE:
                player.ApplyGun(new PierceGunType());
                break;
            case PickUpType.SHOT_PIERCE:
                player.type = new PierceShotType();
                break;
            default:
                break;
        }
    }
}
