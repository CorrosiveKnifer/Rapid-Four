using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldSlider : MonoBehaviour
{
    public Image fillImage;

    public int ID = 0;

    private PlayerController myPlayer;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            if (player.GetComponentInParent<PlayerController>()?.ID == ID)
            {
                myPlayer = player.GetComponentInParent<PlayerController>();
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float max = myPlayer.shieldObject.reachargeTime;
        float current = myPlayer.shieldObject.timer;

        fillImage.fillAmount = 1.0f - current / max;
    }
}
