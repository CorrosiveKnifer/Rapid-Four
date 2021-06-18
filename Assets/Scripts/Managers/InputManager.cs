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

    public enum StickDirection
    {
        LEFT,
        RIGHT,
        UP,
        DOWN

    }
    public enum MouseButton
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
    public Keyboard keyboard = Keyboard.current;

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
    public struct Controller
    {
        public Controller(bool _isKeyboard = false, int _controllerID = 0, Gamepad _gamepad = null, int _shipID =0)
        {
            isKeyboard = _isKeyboard;
            controllerID = _controllerID;
            gamepad = _gamepad;
            shipID = _shipID;
        }
        public bool isKeyboard;
        public int controllerID;
        public Gamepad gamepad;
        public int shipID;

    }
    private Controller[] players = new Controller[2];
    /// <summary>
    /// Check if the player is assigned a controller
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool IsPlayerAssigned(int id)
    {
        if (players[id].isKeyboard || players[id].gamepad != null)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Set ship ID
    /// </summary>
    /// <param name="_shipID"></param>
    /// <param name="_playerID"></param>
    public void SetShipToPlayer(int _playerID, int _shipID)
    {
        players[_playerID].shipID = _shipID;
    }

    /// <summary>
    /// Set the controller to Keyboard
    /// </summary>
    /// <param name="_isKeyboard"></param>
    /// <param name="_playerID"></param>
    public void SetKeyToPlayer( int _playerID, bool _isKeyboard)
    {
        players[_playerID].isKeyboard = _isKeyboard;
    }
    /// <summary>
    /// check if the certain player is ready as it has both have a control assigned and a ship assigned
    /// </summary>
    /// <param name="_playerID"></param>
    /// <returns></returns>

    public bool IsPlayerReady(int _playerID)
    {
        if(IsPlayerAssigned(_playerID) && players[_playerID].shipID !=0)
        {
            return true;
        }
        return false;
    }
    //-----------------------------------------------------------------------
    //this section is for lobby

    /// <summary>
    /// check if the gamepad is connected otherwise it assigned as null
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool CheckGampadConnected(int id)
    {
        foreach(Gamepad padAvail in Gamepad.all)
        {
            if(padAvail == players[id].gamepad)
            {
                return true;
            }
        }
        players[id] = new Controller();

        return false;
    }

    /// <summary>
    /// check on any detection on the controller for the player(s)
    /// </summary>
    public void LobbyDetection()
    {
        if(!PlayerChoseGamepad() && !PlayerChoseKeyBoard() && players[0].shipID == 0)
        {
            confirmController(0);
            return;
        }

        for(int i=0; i< players.Length; i++)
        {
            //check if there is no controls assigned to this player
            if (!players[i].isKeyboard && players[i].gamepad == null)
            {
                if (players[i].shipID == 0)
                    confirmController(i);
                
            }
            else if(players[i].isKeyboard || players[i].gamepad != null)
            {
                if (players[i].shipID == 0)
                    cancelController(i);
             
            }
            
        }
    }

    /// <summary>
    /// check if player chose gamepad
    /// </summary>
    /// <returns></returns>
    public bool PlayerChoseGamepad()
    {
        foreach (Controller player in players)
        {
            if(player.gamepad !=null)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// checks if player chose keyboard
    /// </summary>
    /// <returns></returns>
    public bool PlayerChoseKeyBoard()
    {
        foreach (Controller player in players)
        {
            if (player.isKeyboard)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// waits for a confirmation to unassign controller
    /// </summary>
    /// <param name="_index"></param>
    public void cancelController(int _index)
    {
        //if its assigned to keys
        if (players[_index].isKeyboard)
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                players[_index] = new Controller();
                Debug.Log("deselect player " + _index.ToString() + " with keyboard");
            }
            
        }
        else
        {
            if(players[_index].gamepad.buttonEast.wasPressedThisFrame)
            {
                players[_index] = new Controller();
                Debug.Log("deselect player " + _index.ToString() + " with controller");
            }
            

        }
    }

    /// <summary>
    /// waits for a confirmation to assign controller
    /// </summary>
    /// <param name="_index"></param>
    public void confirmController(int _index)
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && !PlayerChoseKeyBoard())
        {
            players[_index] = new Controller(true, _index);            
            Debug.Log("confirm player " + _index.ToString() + " with keyboard");


        }
        else
        {
            confirmGamepad(_index);
            
        }
    }

    /// <summary>
    /// comfirming for a gamepad
    /// </summary>
    /// <param name="_index"></param>
    public void confirmGamepad(int _index)
    {
        Gamepad currentGamepad = Gamepad.current;
        if (currentGamepad != null)
        {
            if (currentGamepad.buttonSouth.wasPressedThisFrame && UnasignedController(currentGamepad))
            {

                players[_index] = new Controller(false, _index, currentGamepad);

                Debug.Log("confirm player " + _index.ToString() + " with Controller");
                
            }
        }
    }

    /// <summary>
    /// check if this the player controls are assigned both from the key and the gamepad
    /// </summary>
    /// <param name="_gamepad"></param>
    /// <returns></returns>
    public bool UnasignedController(Gamepad _gamepad)
    {
        foreach(Controller player in players)
        {
            if(player.isKeyboard )
            {
                return true;
            }
            if(player.gamepad == _gamepad)
            {
                return false;
            }
        }
        return true;
        
    }
    //end of lobby selection
    //-----------------------------------------------------------------------------


    /// <summary>
    /// get the player controller by player id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Controller GetPlayerControl(int id)
    {
        return players[id];
    }

    /// <summary>
    /// Automatically assigned controllers (assigns the controllers first before keyboard
    /// </summary>
    public void DefaultAssignControllers()
    {
        int index = 0;
        foreach (Gamepad padAvail in Gamepad.all)
        {
            players[index].gamepad = padAvail;
            players[index].controllerID = index;
            index++;
        }
        if (index != 2)
        {
            players[index].isKeyboard = true;
        }

    }


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
            Gamepad pad = players[playerIndex].gamepad; //Gamepad.all[players[playerIndex].controllerID];
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
                    return pad.buttonEast.wasPressedThisFrame;

                case ButtonType.BUTTON_WEST:
                    return pad.buttonWest.wasPressedThisFrame;

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
            Gamepad pad = players[playerIndex].gamepad; //Gamepad.all[players[playerIndex].controllerID];
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
                    return pad.buttonEast.isPressed;

                case ButtonType.BUTTON_WEST:
                    return pad.buttonWest.isPressed;

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

    public bool GetStickDirection(StickDirection direction, int playerIndex)
    {
        if (players[playerIndex].isKeyboard)
        {
            switch (direction)
            {
                default:
                    Debug.LogWarning($"Unsupported key type in GetStickDirection.");
                    return false;
                case StickDirection.UP:
                    return keyboard.wKey.wasPressedThisFrame;

                case StickDirection.DOWN:
                    return keyboard.sKey.wasPressedThisFrame;

                case StickDirection.LEFT:
                    return keyboard.aKey.wasPressedThisFrame;

                case StickDirection.RIGHT:
                    return keyboard.dKey.wasPressedThisFrame;


            }


        }
        else
        {
            Gamepad pad = players[playerIndex].gamepad; //Gamepad.all[players[playerIndex].controllerID];
            if (pad == null)
            {
                return false;

            }

            switch (direction)
            {
                default:
                    Debug.LogWarning($"Unsupported button type in GetStickDirection.");
                    return false;
                case StickDirection.UP:
                    return pad.leftStick.up.wasPressedThisFrame;

                case StickDirection.DOWN:
                    return pad.leftStick.down.wasPressedThisFrame;

                case StickDirection.LEFT:
                    return pad.leftStick.left.wasPressedThisFrame;

                case StickDirection.RIGHT:
                    return pad.leftStick.right.wasPressedThisFrame;

            }

        }
    }

    /// <summary>
    /// Getting the vertical axis value from controller
    /// </summary>
    /// <param name="joystick"></param>
    /// <param name="playerIndex"></param>
    /// <returns></returns>
    public float GetVerticalAxis(Joysticks joystick, int playerIndex, Camera playercamera = null)
    {
        if (players[playerIndex].isKeyboard)
        {
            switch (joystick)
            {
                default:
                    Debug.LogWarning($"Unsupported key type in GetVerticalAxis.");
                    return 0;
                case Joysticks.LEFT:
                    return GetVerticalAxis();

                case Joysticks.RIGHT:
                    return GetMouseVertAxis(playercamera);
            }
            
        }
        else
        {
            Gamepad pad = players[playerIndex].gamepad; //Gamepad.all[players[playerIndex].controllerID];
            if (pad == null)
            {
                return 0;

            }

            switch (joystick)
            {
                default:
                    Debug.LogWarning($"Unsupported button type in GetVerticalAxis.");
                    return 0;
                case Joysticks.LEFT:
                    return pad.leftStick.y.ReadValue();

                case Joysticks.RIGHT:
                    return pad.rightStick.y.ReadValue();
            }
        }
    }

    /// <summary>
    /// Getting the Horizontall Axis value from controller
    /// </summary>
    /// <param name="joystick"></param>
    /// <param name="playerIndex"></param>
    /// <returns></returns>
    public float GetHorizontalAxis(Joysticks joystick, int playerIndex, Camera playercamera = null)
    {
        if (players[playerIndex].isKeyboard)
        {
            switch (joystick)
            {
                default:
                    Debug.LogWarning($"Unsupported key type in GetHorizontalAxis.");
                    return 0;
                case Joysticks.LEFT:
                    return GetHorizontalAxis();

                case Joysticks.RIGHT:
                    return GetMouseHortAxis(playercamera);
            }

        }
        else
        {
            Gamepad pad = players[playerIndex].gamepad; //Gamepad.all[players[playerIndex].controllerID];
            if (pad == null)
            {
                return 0;

            }

            switch (joystick)
            {
                default:
                    Debug.LogWarning($"Unsupported button type in GetHorizontalAxis.");
                    return 0;
                case Joysticks.LEFT:
                    return pad.leftStick.x.ReadValue();

                case Joysticks.RIGHT:
                    return pad.rightStick.x.ReadValue();
            }
        }
    }

    /// <summary>
    /// Getting the vertical axis value from keyboard
    /// </summary>
    /// <param name="joystick"></param>
    /// <param name="playerIndex"></param>
    /// <returns></returns>
    public float GetVerticalAxis()
    {
        if(keyboard.wKey.isPressed)
        {
            return 1.0f;
        }
        else if (keyboard.sKey.isPressed)
        {
            return -1.0f;
        }
        else
        {
            return 0.0f;
        }


        
    }

    /// <summary>
    /// Getting the Horizontall Axis value from keyboard
    /// </summary>
    /// <param name="joystick"></param>
    /// <param name="playerIndex"></param>
    /// <returns></returns>
    public float GetHorizontalAxis()
    {
        if (keyboard.dKey.isPressed)
        {
            return 1.0f;
        }
        else if (keyboard.aKey.isPressed)
        {
            return -1.0f;
        }
        else
        {
            return 0.0f;
        }
    }
    /// <summary>
    /// Checking if the mouse button is down
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    public bool GetMouseDown(MouseButton button)
    {

        switch (button)
        {
            default:
                Debug.LogWarning($"Unsupported mouse button type in GetMouseDown.");
                return false;
            case MouseButton.LEFT:
                return mouse.leftButton.wasPressedThisFrame;

            case MouseButton.RIGHT:
                return mouse.rightButton.wasPressedThisFrame;
        }

    }
    /// <summary>
    /// Checking if the mouse button is pressed
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    public bool GetMousePress(MouseButton button)
    {

        switch (button)
        {
            default:
                Debug.LogWarning($"Unsupported mouse button type in GetMouseDown.");
                return false;
            case MouseButton.LEFT:
                return mouse.leftButton.isPressed;

            case MouseButton.RIGHT:
                return mouse.rightButton.isPressed;
        }

    }

    /// <summary>
    /// Get the mouse Vertical axis
    /// </summary>
    /// <returns></returns>
    public float GetMouseVertAxis(Camera playerCam)
    {
        if (playerCam != null)
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos.z = playerCam.farClipPlane * .5f;
            Vector3 worldPoint = playerCam.ScreenToWorldPoint(mousePos);

            return worldPoint.y;
        }
        return 0;

    }
    /// <summary>
    /// Get the mouse Horizontal axis
    /// </summary>
    /// <returns></returns>
    public float GetMouseHortAxis(Camera playerCam)
    {
        if (playerCam != null)
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos.z = playerCam.farClipPlane * .5f;
            Vector3 worldPoint = playerCam.ScreenToWorldPoint(mousePos);

            return worldPoint.x;
        }
        return 0;

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
