using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Michael Jordan
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

    public enum Joysticks
    {
        LEFT,
        RIGHT

    }

    public enum KeyType
    {
        KEY_W,
        KEY_S,
        KEY_D,
        KEY_A,
        KEY_ESC,
        KEY_SPACE,
        KEY_1,
        KEY_2,
        KEY_3,
        KEY_4

    }

    private static InputManager instance = null;
    public Mouse mouse;
    public Keyboard keyboard;

    public static InputManager GetInstance()
    {
        if (instance == null)
        {
            GameObject loader = new GameObject();
            instance = loader.AddComponent<InputManager>();
            return loader.GetComponent<InputManager>();

        }

        return instance;
    }

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

    /// <summary>
    /// Checks if the key buttons has been pressed once
    /// </summary>
    /// <param name="keyName"></param>
    /// <param name="playerIndex"></param>
    /// <returns></returns>
    public bool GetKeyDown(KeyType keyName, int playerIndex)
    {
        switch (keyName)
        {
            default:
                Debug.LogWarning($"Unsupported Key type in GetKeyDown.");
                return false;
            case KeyType.KEY_W:
                return keyboard.wKey.wasPressedThisFrame;

            case KeyType.KEY_D:
                return keyboard.dKey.wasPressedThisFrame;

            case KeyType.KEY_A:
                return keyboard.aKey.wasPressedThisFrame;

            case KeyType.KEY_S:
                return keyboard.sKey.wasPressedThisFrame;

            case KeyType.KEY_ESC:
                return keyboard.escapeKey.wasPressedThisFrame;

            case KeyType.KEY_SPACE:
                return keyboard.spaceKey.wasPressedThisFrame;

            case KeyType.KEY_1:
                return keyboard.digit1Key.wasPressedThisFrame;

            case KeyType.KEY_2:
                return keyboard.digit2Key.wasPressedThisFrame;

            case KeyType.KEY_3:
                return keyboard.digit3Key.wasPressedThisFrame;

            case KeyType.KEY_4:
                return keyboard.digit4Key.wasPressedThisFrame;
        }
    }



    /// <summary>
    /// Checks if the controller buttons has been pressed once
    /// </summary>
    /// <param name="buttonName"></param>
    /// <param name="playerIndex"></param>
    /// <returns></returns>
    public bool GetKeyDown(ButtonType buttonName, int playerIndex)
    {
        if (players[playerIndex].isKeyboard)
        {
            switch (buttonName)
            {
                default:
                    Debug.LogWarning($"Unsupported key type in GetKeyDown.");
                    return false;
                case ButtonType.BUTTON_NORTH:
                    return keyboard.wKey.wasPressedThisFrame;

                case ButtonType.BUTTON_EAST:
                    return keyboard.dKey.wasPressedThisFrame;

                case ButtonType.BUTTON_WEST:
                    return keyboard.aKey.wasPressedThisFrame;

                case ButtonType.BUTTON_SOUTH:
                    return keyboard.sKey.wasPressedThisFrame;

                case ButtonType.BUTTON_START:
                    return keyboard.escapeKey.wasPressedThisFrame;

                case ButtonType.BUTTON_SELECT:
                    return keyboard.spaceKey.wasPressedThisFrame;

                case ButtonType.BUTTON_LT:
                    return keyboard.digit1Key.wasPressedThisFrame;

                case ButtonType.BUTTON_RT:
                    return keyboard.digit2Key.wasPressedThisFrame;

                case ButtonType.BUTTON_LS:
                    return keyboard.digit3Key.wasPressedThisFrame;

                case ButtonType.BUTTON_RS:
                    return keyboard.digit4Key.wasPressedThisFrame;

            }


        }
        else
        {
            Gamepad pad = Gamepad.all[players[playerIndex].controllerID];
            if (pad == null)
            {
                return false;

            }

            switch (buttonName)
            {
                default:
                    Debug.LogWarning($"Unsupported button type in GetKeyDown.");
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
    }

    public bool GetKeyPressed(KeyType keyName, int playerIndex)
    {
        switch (keyName)
        {
            default:
                Debug.LogWarning($"Unsupported Key type in GetKeyDown.");
                return false;
            case KeyType.KEY_W:
                return keyboard.wKey.isPressed;

            case KeyType.KEY_D:
                return keyboard.dKey.isPressed;

            case KeyType.KEY_A:
                return keyboard.aKey.isPressed;

            case KeyType.KEY_S:
                return keyboard.sKey.isPressed;

            case KeyType.KEY_ESC:
                return keyboard.escapeKey.isPressed;

            case KeyType.KEY_SPACE:
                return keyboard.spaceKey.isPressed;

            case KeyType.KEY_1:
                return keyboard.digit1Key.isPressed;

            case KeyType.KEY_2:
                return keyboard.digit2Key.isPressed;

            case KeyType.KEY_3:
                return keyboard.digit3Key.isPressed;

            case KeyType.KEY_4:
                return keyboard.digit4Key.isPressed;

        }


    }

    /// <summary>
    /// When the controller button are being pressed
    /// </summary>
    /// <param name="buttonName"></param>
    /// <param name="playerIndex"></param>
    /// <returns></returns>

    public bool GetKeyPressed(ButtonType buttonName, int playerIndex)
    {
        if (players[playerIndex].isKeyboard)
        {
            switch (buttonName)
            {
                default:
                    Debug.LogWarning($"Unsupported key type in GetKeyPress.");
                    return false;
                case ButtonType.BUTTON_NORTH:
                    return keyboard.wKey.isPressed;

                case ButtonType.BUTTON_EAST:
                    return keyboard.dKey.isPressed;

                case ButtonType.BUTTON_WEST:
                    return keyboard.aKey.isPressed;

                case ButtonType.BUTTON_SOUTH:
                    return keyboard.sKey.isPressed;

                case ButtonType.BUTTON_START:
                    return keyboard.escapeKey.isPressed;

                case ButtonType.BUTTON_SELECT:
                    return keyboard.spaceKey.isPressed;

                case ButtonType.BUTTON_LT:
                    return keyboard.digit1Key.isPressed;

                case ButtonType.BUTTON_RT:
                    return keyboard.digit2Key.isPressed;

                case ButtonType.BUTTON_LS:
                    return keyboard.digit3Key.isPressed;

                case ButtonType.BUTTON_RS:
                    return keyboard.digit4Key.isPressed;

            }


        }
        else
        {
            Gamepad pad = Gamepad.all[players[playerIndex].controllerID];
            if (pad == null)
            {
                return false;

            }

            switch (buttonName)
            {
                default:
                    Debug.LogWarning($"Unsupported button type in GetKeyPress.");
                    return false;
                case ButtonType.BUTTON_NORTH:
                    return pad.buttonNorth.isPressed;

                case ButtonType.BUTTON_EAST:
                    return pad.buttonWest.isPressed;

                case ButtonType.BUTTON_WEST:
                    return pad.buttonNorth.isPressed;

                case ButtonType.BUTTON_SOUTH:
                    return pad.buttonSouth.isPressed;

                case ButtonType.BUTTON_START:
                    return pad.startButton.isPressed;

                case ButtonType.BUTTON_SELECT:
                    return pad.selectButton.isPressed;

                case ButtonType.BUTTON_LT:
                    return pad.leftTrigger.isPressed;

                case ButtonType.BUTTON_RT:
                    return pad.rightTrigger.isPressed;

                case ButtonType.BUTTON_LS:
                    return pad.leftShoulder.isPressed;

                case ButtonType.BUTTON_RS:
                    return pad.rightShoulder.isPressed;

            }

        }
    }

    /// <summary>
    /// Getting the vertical axis value
    /// </summary>
    /// <param name="joystick"></param>
    /// <param name="playerIndex"></param>
    /// <returns></returns>
    public float GetVerticalAxis(Joysticks joystick, int playerIndex)
    {
        if (players[playerIndex].isKeyboard)
        {
            return 0;
        }
        else
        {
            Gamepad pad = Gamepad.all[players[playerIndex].controllerID];
            if (pad == null)
            {
                return 0;

            }

            switch (joystick)
            {
                default:
                    Debug.LogWarning($"Unsupported button type in GetKeyPress.");
                    return 0;
                case Joysticks.LEFT:
                    return pad.leftStick.y.ReadValue();

                case Joysticks.RIGHT:
                    return pad.rightStick.y.ReadValue();
            }
        }
    }

    /// <summary>
    /// Getting the Horizontall Axis value
    /// </summary>
    /// <param name="joystick"></param>
    /// <param name="playerIndex"></param>
    /// <returns></returns>
    public float GetHorizontalAxis(Joysticks joystick, int playerIndex)
    {
        if (players[playerIndex].isKeyboard)
        {
            return 0;
        }
        else
        {
            Gamepad pad = Gamepad.all[players[playerIndex].controllerID];
            if (pad == null)
            {
                return 0;

            }

            switch (joystick)
            {
                default:
                    Debug.LogWarning($"Unsupported button type in GetKeyPress.");
                    return 0;
                case Joysticks.LEFT:
                    return pad.leftStick.x.ReadValue();

                case Joysticks.RIGHT:
                    return pad.rightStick.x.ReadValue();
            }
        }
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
