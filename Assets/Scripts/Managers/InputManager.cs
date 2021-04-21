using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Singleton

    public static InputManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Debug.LogError("Second Instance of InputManager was created, this instance was destroyed.");
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    #endregion

    [Header("Player A Controls")]
    public KeyCode playerALeft;
    public KeyCode playerARight;
    public KeyCode playerAForwards;
    public KeyCode playerABackwards;
    public KeyCode playerAShoot;

    [Header("Player B Controls")]
    public KeyCode playerBLeft;
    public KeyCode playerBRight;
    public KeyCode playerBForwards;
    public KeyCode playerBBackwards;
    public KeyCode playerBShoot;

    public float GetHorizontalInput(int playerID)
    {
        if (playerID > 1 || playerID < 0)
            Debug.LogWarning($"Invalid player ID ({playerID}) passed, will use player 0 instead.");

        float playerHoriz = 0.0f;
        KeyCode left;
        KeyCode right;
        

        switch (playerID)
        {
            default:
            case 0:
                left = playerALeft;
                right = playerARight;
                break;
            case 1:
                left = playerBLeft;
                right = playerBRight;
                break;
        }

        if (Input.GetKey(left))
        {
            playerHoriz -= 1.0f;
        }
        if (Input.GetKey(right))
        {
            playerHoriz += 1.0f;
        }
        return playerHoriz;
    }

    public float GetVerticalInput(int playerID)
    {
        if (playerID > 1 || playerID < 0)
            Debug.LogWarning($"Invalid player ID ({playerID}) passed, will use player 0 instead.");
        
        float playerVerti = 0.0f;
        KeyCode forward;
        KeyCode backward;


        switch (playerID)
        {
            default:
            case 0:
                forward = playerAForwards;
                backward = playerABackwards;
                break;
            case 1:
                forward = playerBForwards;
                backward = playerBBackwards;
                break;
        }

        if (Input.GetKey(backward))
        {
            playerVerti -= 1.0f;
        }
        if (Input.GetKey(forward))
        {
            playerVerti += 1.0f;
        }
        return playerVerti;
    }

    public bool GetPlayerShoot(int playerID)
    {
        if (playerID > 1 || playerID < 0)
            Debug.LogWarning($"Invalid player ID ({playerID}) passed, will use player 0 instead.");
        
        switch (playerID)
        {
            default:
            case 0:
                return Input.GetKeyDown(playerAShoot);
            case 1:
                return Input.GetKeyDown(playerBShoot);
        }
    }
}
