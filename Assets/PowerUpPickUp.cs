using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPickUp : MonoBehaviour
{
    public enum PickUpType { SHOT_BASIC, GUN_BASIC, GUN_SPLIT, SHOT_HOMING};
    public GameObject imagePlane;
    public PickUpType myType;

    // Start is called before the first frame update
    void Start()
    {
        myType = (PickUpType)Random.Range((int)PickUpType.GUN_SPLIT, (int)PickUpType.SHOT_HOMING + 1);
        
        switch (myType)
        {
            case PickUpType.SHOT_BASIC:
                imagePlane.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/BasicShot");
                break;
            case PickUpType.GUN_BASIC:
                imagePlane.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/BasicGun");
                break;
            case PickUpType.GUN_SPLIT:
                imagePlane.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/SplitGun");
                break;
            case PickUpType.SHOT_HOMING:
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
            case PickUpType.GUN_SPLIT:
                player.ApplyGun(new SplitGunType());
                break;
            case PickUpType.SHOT_HOMING:
                player.ApplyEffect(new HomingShotType());
                break;
            default:
                break;
        }
    }
}
