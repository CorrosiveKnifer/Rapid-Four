﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///  William de Beer
/// </summary>
public class CloudSpin : MonoBehaviour
{
    public float m_fRotationSpeedMult;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0.0f, m_fRotationSpeedMult * Time.deltaTime, 0.0f));
    }
}
