﻿using System.Collections;
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
    public Image overHeatBar;

    public Image overHeatOverlay;

    public uint playerID;

    public float healthMax;
    public float shieldMax;
    public float overheatMax = 100;

    public float healthCurrent;
    public float shieldCurrent;
    public float overheatCurrent;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.fillAmount = healthCurrent / healthMax;
        shieldBar.fillAmount = shieldCurrent / shieldMax;

        overHeatBar.fillAmount = overheatCurrent / overheatMax;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = healthCurrent / healthMax;
        shieldBar.fillAmount = shieldCurrent / shieldMax;

        overHeatBar.fillAmount = overheatCurrent / overheatMax;
        overHeatBar.color = Color.Lerp(Color.gray, new Color(1, 120.0f/255.0f, 45.0f/255.0f), overHeatBar.fillAmount);

        
        overHeatOverlay.color = new Color(1, 1, 1, (overheatCurrent / overheatMax));
    }
}
