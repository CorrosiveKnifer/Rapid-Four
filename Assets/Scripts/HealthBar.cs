using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Michael Jordan
/// </summary>
public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public Image healthMaxBar;
    public Image shieldBar;
    public Image shieldMaxBar;

    public uint playerID;

    public float healthMax;
    public float shieldMax;

    public float healthCurrent;
    public float shieldCurrent;

    public float totalCombined;

    // Start is called before the first frame update
    void Start()
    {
        totalCombined = healthMax + shieldMax;
        healthMaxBar.fillAmount = healthMax / totalCombined;
        shieldMaxBar.fillAmount = shieldMax / totalCombined;

        healthBar.fillAmount = healthCurrent / totalCombined;
        shieldBar.fillAmount = shieldCurrent / totalCombined + healthCurrent / totalCombined;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = healthCurrent / (healthMax + shieldMax);
        shieldBar.fillAmount = shieldCurrent / shieldMax;

        if(shieldBar.fillAmount < healthBar.fillAmount)
        {
            shieldBar.fillAmount = healthBar.fillAmount;
        }
    }
}
