using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// THIS IS FOR TESTING PURPOSES
/// The allGamePad variable is the primary control testing
/// The controls is the alternative method we will use
/// Allgamepad contains the gamepads of the controlls in the system
/// </summary>
public class CubeControls : MonoBehaviour
{
    ControlInput controls;
    Vector2 rotate;
    Vector2 thrust;
    private Rigidbody body;
    public float speed = 350.0f;
    public float rotationSpeed = 120.0f;

    public int playerID;
    Gamepad player1;

    Mouse mouse;
    Keyboard keyboard;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        keyboard = Keyboard.current;
        mouse = Mouse.current;

        if (Gamepad.all.Count != 0)
        {
            var allGamepads = Gamepad.all;
            player1 = allGamepads[playerID];

            //float here = allGamepads.Count;
            //Debug.Log(player1);
        }

        //Debug.Log(string.Join("\n", Gamepad.all));
    }

    /*
    private void Awake()
    {
        controls = new ControlInput();


        controls.Gameplay.Shoot.performed += ctx => Shooting();
        controls.Gameplay.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>();

        controls.Gameplay.Thrust.performed += ctx => thrust = ctx.ReadValue<Vector2>();
        controls.Gameplay.Thrust.canceled += ctx => thrust = Vector2.zero;
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }
    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }
    private void Shooting()
    {
        Debug.Log("Pew");
    }
    */

    /// <summary>
    /// A function that is called in update when the right joysticks move
    /// Works for controls Input system
    /// </summary>
    private void Movement()
    {
        Vector2 r = new Vector2(rotate.x, rotate.y);

        float RotHorizontalAxis = r.x;
        float RotVerticalAxis = r.y;


        Vector2 m = new Vector2(-thrust.x, thrust.y);

        float horizontalAxis = m.x;
        float verticalAxis = m.y
            ;
        if (verticalAxis > 0.0f)
        {
            //body.AddForce(transform.up * speed * verticalAxis * Time.deltaTime, ForceMode.Acceleration);

        }
        else if (verticalAxis < 0.0f)
        {
            //body.velocity = Vector3.zero;

        }

        //direction of the joysticks
        Vector3 direct = new Vector3(RotHorizontalAxis, RotVerticalAxis, 0.0f).normalized;


        if (verticalAxis != 0.0f || RotVerticalAxis != 0.0f)
        {
            body.rotation = Quaternion.Slerp(body.rotation, Quaternion.LookRotation(new Vector3(0, 0, 1), direct), 0.1f);
        }



        Debug.Log(r);

    }
    void GamepadForPlayer1()
    {
        if (player1.buttonEast.wasPressedThisFrame)
        {
            Debug.Log(player1);
            Debug.Log("This works");
        }
        if (player1.leftStick.x.ReadValue() != 0 || player1.leftStick.y.ReadValue() != 0)
        {
            Debug.Log("ROTATION");
            Vector2 r = new Vector2(player1.leftStick.x.ReadValue(), player1.leftStick.y.ReadValue());

            float RotHorizontalAxis = r.x;
            float RotVerticalAxis = r.y;

            Vector3 direct = new Vector3(RotHorizontalAxis, RotVerticalAxis, 0.0f).normalized;
            body.rotation = Quaternion.Slerp(body.rotation, Quaternion.LookRotation(new Vector3(0, 0, 1), direct), 0.1f);
        }
    }

    void GamepadControls()
    {
        //the buttons on the right
        if (player1.buttonEast.wasPressedThisFrame)
        {
            Debug.Log("player" + (playerID + 1).ToString() +  " east button pressed");
        }
        if (player1.buttonWest.wasPressedThisFrame)
        {
            Debug.Log("player" + (playerID + 1).ToString() + " west button pressed");
        }
        if (player1.buttonSouth.wasPressedThisFrame)
        {
            Debug.Log("player" + (playerID + 1).ToString() + " south button pressed");
        }
        if (player1.buttonNorth.wasPressedThisFrame)
        {
            Debug.Log("player" + (playerID + 1).ToString() + " north button pressed");
        }

        //stick clicks
        if (player1.leftStickButton.wasPressedThisFrame)
        {
            Debug.Log("player" + (playerID + 1).ToString() + " left stick button pressed");
        }
        if (player1.rightStickButton.wasPressedThisFrame)
        {
            Debug.Log("player" + (playerID + 1).ToString() + " right stick button  pressed");
        }

        //trigger button click
        if (player1.leftTrigger.wasPressedThisFrame)
        {
            Debug.Log("player" + (playerID + 1).ToString() + " left trigger button pressed");
        }
        if (player1.rightTrigger.wasPressedThisFrame)
        {
            Debug.Log("player" + (playerID + 1).ToString() + " right trigger button  pressed");
        }

        //trigger button held
        if (player1.leftTrigger.isPressed)
        {
            Debug.Log("player" + (playerID + 1).ToString() + " left trigger button held");
        }
        if (player1.rightTrigger.isPressed)
        {
            Debug.Log("player" + (playerID + 1).ToString() + " right trigger button  held");
        }

        //shoulder button
        if (player1.leftShoulder.wasPressedThisFrame)
        {
            Debug.Log("player" + (playerID + 1).ToString() + " left shoulder button pressed");
        }
        if (player1.rightShoulder.wasPressedThisFrame)
        {
            Debug.Log("player" + (playerID + 1).ToString() + " right shoulder button  pressed");
        }

        //start button pressed
        if (player1.startButton.wasPressedThisFrame)
        {
            Debug.Log("player" + (playerID + 1).ToString() + " start button pressed");
        }

        //select button pressed
        if (player1.selectButton.wasPressedThisFrame)
        {
            Debug.Log("player" + (playerID + 1).ToString() + " select button  pressed");
        }

        //Dpad
        if(player1.dpad.right.wasPressedThisFrame)
        {
            Debug.Log("player" + (playerID + 1).ToString() + " east dpad  pressed");
        }
        if (player1.dpad.left.wasPressedThisFrame)
        {
            Debug.Log("player" + (playerID + 1).ToString() + " west dpad  pressed");
        }
        if (player1.dpad.down.wasPressedThisFrame)
        {
            Debug.Log("player" + (playerID + 1).ToString() + " south dpad  pressed");
        }
        if (player1.dpad.up.wasPressedThisFrame)
        {
            Debug.Log("player" + (playerID + 1).ToString() + " north dpad  pressed");
        }


        //Stick axis
        Vector2 leftStickVector = player1.leftStick.ReadValue();
        Vector2 rightStickVector = player1.rightStick.ReadValue();
    
        //left stick moving
        if (leftStickVector.x == 0 && leftStickVector.y == 0)
        {
            //Debug.Log("no movement in left stick");
        }
        else
        {
            Debug.Log("player" + (playerID + 1).ToString() + " left stick moving");

            Vector2 r = leftStickVector;

            float RotHorizontalAxis = r.x;
            float RotVerticalAxis = r.y;

            Vector3 direct = new Vector3(RotHorizontalAxis, RotVerticalAxis, 0.0f).normalized;
            body.rotation = Quaternion.Slerp(body.rotation, Quaternion.LookRotation(new Vector3(0, 0, 1), direct), 0.1f);
        }

        //right stick moving
        if (rightStickVector.x == 0 && rightStickVector.y == 0)
        {
           //Debug.Log("no movement in right stick");
        }
        else 
        { 
            Debug.Log("player" + (playerID + 1).ToString() + " right stick moving");
            Vector2 r = rightStickVector;
            
            

            float RotHorizontalAxis = r.x;
            float RotVerticalAxis = r.y;

            Vector3 direct = new Vector3(RotHorizontalAxis, RotVerticalAxis, 0.0f).normalized;
            
        }
    
    }
    void MouseControls()
    {
        Debug.Log(string.Format("Mouse position x={0} y={1}", 
            Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue()));

        if (mouse.leftButton.wasPressedThisFrame)
        {
            Debug.Log("mouse pressed left");
        }
        if (mouse.rightButton.wasPressedThisFrame)
        {
            Debug.Log("mouse pressed right");
        }
        
    }

    void KeyboardControls()
    {
        //the WASD keys pressed
        if (keyboard.dKey.wasPressedThisFrame)
        {
            Debug.Log("player" + (playerID + 1).ToString() + " D key pressed");
        }
        if (keyboard.aKey.wasPressedThisFrame)
        {
            Debug.Log("player" + (playerID + 1).ToString() + " A Key pressed");
        }
        if (keyboard.sKey.wasPressedThisFrame)
        {
            Debug.Log("player" + (playerID + 1).ToString() + " S Key pressed");
        }
        if (keyboard.wKey.wasPressedThisFrame)
        {
            Debug.Log("player" + (playerID + 1).ToString() + " W Key pressed");
        }

        //The space bar key pressed
        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("player" + (playerID + 1).ToString() + " Space Key pressed");
        }

        //The shift bar key pressed
        if (keyboard.leftShiftKey.wasPressedThisFrame)
        {
            Debug.Log("player" + (playerID + 1).ToString() + " left shift Key pressed");
        }
        if (keyboard.rightShiftKey.wasPressedThisFrame)
        {
            Debug.Log("player" + (playerID + 1).ToString() + " right shift Key pressed");
        }

    }

    // Update is called once per frame
    void Update()
    {

        KeyboardControls();
        //MouseControls();

        //if (Gamepad.all.Count != 0 )
        if (player1 != null)
        {
            GamepadControls();
        }



        //GamepadForPlayer1();
        //Movement();
    }

   


}
