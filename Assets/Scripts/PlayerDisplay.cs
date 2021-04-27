using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDisplay : MonoBehaviour
{
    public int ID;
    public Image GunIcon;
    public Image projIcon;

    private PlayerController myPlayer;
    private Dictionary<System.Type, Material> Icons;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            if(player.GetComponentInParent<PlayerController>()?.ID == ID)
            {
                myPlayer = player.GetComponentInParent<PlayerController>();
                break;
            }
        }

        Icons = new Dictionary<System.Type, Material>();
        Icons.Add(typeof(SplitGunType), Resources.Load<Material>("Materials/SplitGun"));
        Icons.Add(typeof(BasicGunType), Resources.Load<Material>("Materials/BasicGun"));
        Icons.Add(typeof(HomingShotType), Resources.Load<Material>("Materials/HomingShot"));
        Icons.Add(typeof(PierceShotType), Resources.Load<Material>("Materials/PercingShot"));
        Icons.Add(typeof(SplitTwoGunType), Resources.Load<Material>("Materials/DoubleGun"));
        Icons.Add(typeof(BasicShotType), Resources.Load<Material>("Materials/BasicShot"));
        Icons.Add(typeof(FrostShotType), Resources.Load<Material>("Materials/FreezeShot"));
    }

    // Update is called once per frame
    void Update()
    {
        System.Type gun, shot;

        myPlayer.GetPowerUps(out gun, out shot);

        Material gunMat, shotMat;
        if (Icons.TryGetValue(gun, out gunMat))
        {
            GunIcon.material = gunMat;
        }
        else
            Debug.LogError($"Gun type doesn't exist in the display dictonary. \"{gun.FullName}\"");

        if (Icons.TryGetValue(shot, out shotMat))
        {
            projIcon.material = shotMat;
        }
        else
            Debug.LogError($"Shot type doesn't exist in the display dictonary. \"{gun.FullName}\"");
    }
}
