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
    public Image overHeatBar;

    public Image boundsOverlay;
    public Image overHeatOverlay;

    public uint playerID;

    public float healthMax;
    public float shieldMax;
    public float overheatMax = 100;
    public bool showBoundsOverlay = false;

    public float healthCurrent;
    public float shieldCurrent;
    public float overheatCurrent;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.fillAmount = healthCurrent / healthMax;
        shieldBar.fillAmount = shieldCurrent / shieldMax;

        boundsOverlay.color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = healthCurrent / healthMax;
        shieldBar.fillAmount = shieldCurrent / shieldMax;

        overHeatBar.fillAmount = overheatCurrent / overheatMax;
        if (overheatCurrent < 50.0f)
            overHeatBar.color = Color.Lerp(Color.gray, new Color(1, 120.0f/255.0f, 45.0f/255.0f), overHeatBar.fillAmount * 2.0f);
        else
            overHeatBar.color = Color.Lerp(new Color(1, 120.0f / 255.0f, 45.0f / 255.0f), Color.red, overHeatBar.fillAmount * 2.0f - 1.0f);


        float alpha = (overheatCurrent > 50.0f) ? (2 * Mathf.Pow(overheatCurrent / overheatMax, 2) - 0.5f) : 0.0f;
        overHeatOverlay.color = new Color(1, 1, 1, Mathf.Lerp(overHeatOverlay.color.a, alpha, 0.05f));

        
        if (showBoundsOverlay)
            boundsOverlay.color = new Color(1, 1, 1, Mathf.Clamp(boundsOverlay.color.a + Time.deltaTime, 0.0f, 1.0f));
        else
            boundsOverlay.color = new Color(1, 1, 1, Mathf.Clamp(boundsOverlay.color.a - Time.deltaTime, 0.0f, 1.0f));
    }
}
