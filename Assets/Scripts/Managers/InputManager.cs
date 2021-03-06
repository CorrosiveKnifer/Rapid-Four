using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Rachael Colaco, Michael Jordan
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
        KEY_TAB,
        KEY_Q,
        KEY_E,
        KEY_ENTER

    }

    private static InputManager instance = null;
    public Mouse mouse = Mouse.current;
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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Second Instance of InputManager was created, this instance was destroyed.");
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
        public Controller(bool _isKeyboard = false, int _controllerID = 0, Gamepad _gamepad = null, int _shipID =-1)
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

    public void setUpLog()
    
    {
        players[0] = new Controller(false, 0, null, -1);
        players[1] = new Controller(false, 0, null, -1);

    }

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
        if(IsPlayerAssigned(_playerID) && players[_playerID].shipID !=-1)
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
    public bool CheckGampadConnected(int playerid)
    {
        foreach(Gamepad padAvail in Gamepad.all)
        {
            if (padAvail == players[playerid].gamepad)
            {
                return true;
            }
        }
        players[playerid] = new Controller(false, 0, null, -1);

        return false;
    }
    /// <summary>
    /// Check if the ship has been assigned by a player
    /// </summary>
    /// <param name="shipID"></param>
    /// <returns></returns>
    public bool IsShipIdTaken(int shipID)
    {
        foreach (Controller player in players)
        {
            if (player.shipID == shipID)
            {
                return true;
            }
           
        }
  

        return false;
    }


    /// <summary>
    /// check on any detection on the controller for the player(s)
    /// </summary>
    public void LobbyDetection()
    {
        if(!PlayerChoseGamepad() && !PlayerChoseKeyBoard() && players[0].shipID == -1)
        {
            confirmController(0);
            return;
        }

        for(int i=0; i< players.Length; i++)
        {
            //check if there is no controls assigned to this player
            if (!players[i].isKeyboard && players[i].gamepad == null)
            {
                if (players[i].shipID == -1)
                    confirmController(i);
                
            }
            else if(players[i].isKeyboard || players[i].gamepad != null)
            {
                if (players[i].shipID == -1)
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
            if (Keyboard.current.tabKey.wasPressedThisFrame)
            {
                players[_index] = new Controller(false, 0, null, -1);
                Debug.Log("deselect player " + _index.ToString() + " with keyboard");
            }
            
        }
        else
        {
            if(players[_index].gamepad.buttonEast.wasPressedThisFrame)
            {
                players[_index] = new Controller(false, 0, null, -1);
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
            players[index].shipID = index;
            index++;
        }
        if (index != 2)
        {
            players[index].isKeyboard = true;
            players[index].controllerID = index;
            players[index].shipID = index;
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

            case KeyType.KEY_TAB:
                return keyboard.tabKey.wasPressedThisFrame;

            case KeyType.KEY_Q:
                return keyboard.qKey.wasPressedThisFrame;

            case KeyType.KEY_E:
                return keyboard.eKey.wasPressedThisFrame;

            case KeyType.KEY_ENTER:
                return keyboard.enterKey.wasPressedThisFrame;
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
                    return keyboard.spaceKey.wasPressedThisFrame;

                case ButtonType.BUTTON_SELECT:
                    return keyboard.tabKey.wasPressedThisFrame;

                case ButtonType.BUTTON_LT:
                    return mouse.rightButton.wasPressedThisFrame;

                case ButtonType.BUTTON_RT:
                    return mouse.leftButton.wasPressedThisFrame;

                case ButtonType.BUTTON_LS:
                    return keyboard.qKey.wasPressedThisFrame;

                case ButtonType.BUTTON_RS:
                    return keyboard.eKey.wasPressedThisFrame;

            }


        }
        else
        {
            Gamepad pad = players[playerIndex].gamepad; //Gamepad.all[players[playerIndex].controllerID];
            if (pad == null)
            {
                pad = Gamepad.current;
                //return false;

            }
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

            case KeyType.KEY_TAB:
                return keyboard.tabKey.isPressed;

            case KeyType.KEY_Q:
                return keyboard.qKey.isPressed;

            case KeyType.KEY_E:
                return keyboard.eKey.isPressed;

            case KeyType.KEY_ENTER:
                return keyboard.enterKey.isPressed;
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
                    Debug.LogWarning($"Unsupported key type in GetKeyDown.");
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
                    return keyboard.spaceKey.isPressed;

                case ButtonType.BUTTON_SELECT:
                    return keyboard.tabKey.isPressed;

                case ButtonType.BUTTON_LT:
                    return mouse.rightButton.isPressed;

                case ButtonType.BUTTON_RT:
                    return mouse.leftButton.isPressed;

                case ButtonType.BUTTON_LS:
                    return keyboard.qKey.isPressed;

                case ButtonType.BUTTON_RS:
                    return keyboard.eKey.isPressed;

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
                pad = Gamepad.current;
                //return false;

            }
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
                    return pad.leftStick.up.wasPressedThisFrame || pad.dpad.up.wasPressedThisFrame;

                case StickDirection.DOWN:
                    return pad.leftStick.down.wasPressedThisFrame || pad.dpad.down.wasPressedThisFrame;

                case StickDirection.LEFT:
                    return pad.leftStick.left.wasPressedThisFrame || pad.dpad.left.wasPressedThisFrame;

                case StickDirection.RIGHT:
                    return pad.leftStick.right.wasPressedThisFrame || pad.dpad.right.wasPressedThisFrame;

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
                return mouse.rightButton.wasPressedThisFrame;

            case MouseButton.RIGHT:
                return mouse.leftButton.wasPressedThisFrame;
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
                return mouse.rightButton.isPressed;

            case MouseButton.RIGHT:
                return mouse.leftButton.isPressed;
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
            mousePos.z = playerCam.farClipPlane;
            Vector3 worldPoint = playerCam.ScreenToWorldPoint(mousePos);
            Vector3 direct = worldPoint - new Vector3(playerCam.transform.position.x, playerCam.transform.position.y, worldPoint.z);
            
            direct = direct / 800.0f;
            if (direct.magnitude > 1.0f)
                direct = direct.normalized * 1.0f;

            direct.z = 0;
            return direct.y;
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
            mousePos.z = playerCam.farClipPlane;
            Vector3 worldPoint = playerCam.ScreenToWorldPoint(mousePos);
            Vector3 direct = worldPoint - new Vector3(playerCam.transform.position.x, playerCam.transform.position.y, worldPoint.z);

            direct = direct / 800.0f;
            if (direct.magnitude > 1.0f)
                direct = direct.normalized * 1.0f;

            direct.z = 0;
            return direct.x;
        }
        return 0;

    }
}
