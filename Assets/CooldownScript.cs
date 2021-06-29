using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownScript : MonoBehaviour
{
    public void SetCooldown(float current, float max)
    {
        GetComponent<Image>().fillAmount = current / max;
    }
}
