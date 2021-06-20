using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Michael Jordan
/// </summary>
public class CameraManager : MonoBehaviour
{
    #region Singleton

    public static CameraManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Second Instance of CameraManager was created, this instance was destroyed.");
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    #endregion
    // Start is called before the first frame update
    private CameraAgent[] agents;

    void Start()
    {
        agents = GetComponentsInChildren<CameraAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Updates the focus point of a camera to a new game object.
    /// </summary>
    /// <param name="index"> player index </param>
    /// <param name="focusObject"> new focus </param>
    public void SetCameraFocus(int index, GameObject focusObject)
    {
        agents[index].AddTarget(focusObject);
    }

    public CameraAgent GetCameraAgent(int index)
    {
        return agents[index];
    }
}
