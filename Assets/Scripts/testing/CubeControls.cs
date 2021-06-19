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

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        var allGamepads = Gamepad.all;
        player1 = allGamepads[playerID];
        
        float here = allGamepads.Count;
        Debug.Log(player1);

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
    // Update is called once per frame
    void Update()
    {
        GamepadForPlayer1();
        //Movement();
    }

   


}
