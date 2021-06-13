using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Rachael Colaco
/// </summary>
public class InputManager : MonoBehaviour
{
    #region Singleton
    public enum ButtonType
    {
        BUTTON_NORTH,
        BUTTON_SOUTH,
        BUTTON_EAST,
        BUTTON_WEST,
        BUTTON_START,
        BUTTON_SELECT,
        BUTTON_LT,
        BUTTON_RT,
        BUTTON_LS,
        BUTTON_RS

    }

    public enum KeyType
    {
        KEY_A,
        KEY_D,
        KEY_W,
        KEY_S

    }

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
    struct Controller
    {
        public Controller(bool _isKeyboard = false, int _controllerID = 0)
        {
            isKeyboard = false;
            controllerID = 0;
        }
        public bool isKeyboard;
        public int controllerID;
    }
    private Controller[] players = new Controller[2];

    public bool GetKeyDown(ButtonType buttonName,  int playerIndex)
    {
        if (players[playerIndex].isKeyboard)
        {
            //DO LATER
            return false;
            
        }
        else
        {
            Gamepad pad = Gamepad.all[players[playerIndex].controllerID];
            if (pad != null)
            {
                switch (buttonName)
                {
                    default:
                        Debug.LogWarning($"Unsupported button type in GetKeyDown." );
                        return false;
                    case ButtonType.BUTTON_NORTH:
                        return pad.buttonNorth.wasPressedThisFrame;
                        
                    case ButtonType.BUTTON_EAST:
                        return pad.buttonWest.wasPressedThisFrame;
                        
                    case ButtonType.BUTTON_WEST:
                        return pad.buttonNorth.wasPressedThisFrame;
                        
                    case ButtonType.BUTTON_SOUTH:
                        return pad.buttonSouth.wasPressedThisFrame;
                       
                    case ButtonType.BUTTON_START:
                        return pad.startButton.wasPressedThisFrame;
                        
                    case ButtonType.BUTTON_SELECT:
                        return pad.selectButton.wasPressedThisFrame;
                        
                    case ButtonType.BUTTON_LT:
                        return pad.leftTrigger.wasPressedThisFrame;
                        
                    case ButtonType.BUTTON_RT:
                        return pad.rightTrigger.wasPressedThisFrame;
                        
                    case ButtonType.BUTTON_LS:
                        return pad.leftShoulder.wasPressedThisFrame;
                        
                    case ButtonType.BUTTON_RS:
                        return pad.rightShoulder.wasPressedThisFrame;
                        
                }
            }
            else
            {
                return false;
            }
        }
    }
    public bool GetKey(ButtonType buttonName)
    {
        return false;
    }
    /*
    [Header("Player A Controls")]
    public KeyCode playerALeft;
    public KeyCode playerARight;
    public KeyCode playerAForwards;
    public KeyCode playerABackwards;
    public KeyCode playerAShoot;
    public KeyCode playerAShootSecond;

    [Header("Player B Controls")]
    public KeyCode playerBLeft;
    public KeyCode playerBRight;
    public KeyCode playerBForwards;
    public KeyCode playerBBackwards;
    public KeyCode playerBShoot;
    public KeyCode playerBShootSecond;

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
                return Input.GetKeyDown(playerAShoot) || Input.GetKeyDown(playerAShootSecond);
            case 1:
                return Input.GetKeyDown(playerBShoot) || Input.GetKeyDown(playerBShootSecond);
        }
    }

    public bool GetPlayerShooting(int playerID)
    {
        if (playerID > 1 || playerID < 0)
            Debug.LogWarning($"Invalid player ID ({playerID}) passed, will use player 0 instead.");

        switch (playerID)
        {
            default:
            case 0:
                return Input.GetKey(playerAShoot) || Input.GetKey(playerAShootSecond);
            case 1:
                return Input.GetKey(playerBShoot) || Input.GetKey(playerBShootSecond);
        }
    }

    public bool GetPlayerUnshoot(int playerID)
    {
        if (playerID > 1 || playerID < 0)
            Debug.LogWarning($"Invalid player ID ({playerID}) passed, will use player 0 instead.");

        switch (playerID)
        {
            default:
            case 0:
                return !Input.GetKey(playerAShoot) && !Input.GetKey(playerAShootSecond);
            case 1:
                return !Input.GetKey(playerBShoot) && !Input.GetKey(playerBShootSecond);
        }
    }
    */
}
