using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPickUp : MonoBehaviour
{
    public enum PickUpType { SHOT_BASIC, GUN_BASIC};
    
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
            default:
                break;
        }
    }
}
