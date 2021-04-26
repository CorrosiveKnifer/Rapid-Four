using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPickUp : MonoBehaviour
{
    public enum PickUpType { SHOT_BASIC, GUN_BASIC, GUN_SPLIT_THREE, GUN_SPLIT_TWO, SHOT_HOMING, SHOT_PIERCE, SHOT_FROST };
    public GameObject imagePlane;
    public PickUpType myType;

    // Start is called before the first frame update
    void Start()
    {
        transform.up = -Vector3.forward;
        myType = (PickUpType)Random.Range((int)PickUpType.GUN_SPLIT_THREE, (int)PickUpType.SHOT_FROST + 1);

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
            case PickUpType.GUN_SPLIT_TWO:
                imagePlane.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/DoubleGun");
                break;
            case PickUpType.SHOT_PIERCE:
                imagePlane.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/PercingShot");
                break;
            default:
                break;
        }
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
            case PickUpType.SHOT_PIERCE:
                player.ApplyEffect(new PierceShotType());
                break;
            case PickUpType.SHOT_FROST:
                player.ApplyEffect(new FrostShotType());
                break;
            default:
                break;
        }
    }
}
