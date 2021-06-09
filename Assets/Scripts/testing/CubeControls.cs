using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CubeControls : MonoBehaviour
{
    ControlInput controls;
    Vector2 rotate;
    Vector2 thrust;
    private Rigidbody body;
    public float speed = 350.0f;
    public float rotationSpeed = 120.0f;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Movement();
    }

    private void Awake()
    {
        controls = new ControlInput();

        //controls.Gameplay.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>();
        //controls.Gameplay.Rotate.canceled += ctx => rotate = Vector2.zero;

        controls.Gameplay.Shoot.performed += ctx => Shooting();
        controls.Gameplay.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>();
        //controls.Gameplay.Rotate.canceled += ctx => rotate = Vector2.zero;

        controls.Gameplay.Thrust.performed += ctx => thrust = ctx.ReadValue<Vector2>();
        controls.Gameplay.Thrust.canceled += ctx => thrust = Vector2.zero;
        //Debug.Log("Shoot");
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
    private void Movement()
    {
        Vector2 r = new Vector2(rotate.x, rotate.y);

        float RotHorizontalAxis = r.x;
        float RotVerticalAxis = r.y;


        Vector2 m = new Vector2(-thrust.x, thrust.y);

        float horizontalAxis = m.x;
        float verticalAxis = m.y;
        if (verticalAxis > 0.0f)
        {
            body.AddForce(transform.up * speed * verticalAxis * Time.deltaTime, ForceMode.Acceleration);
   
        }
        else if (verticalAxis < 0.0f)
        {
            body.velocity = Vector3.zero;
 
        }


        //body.rotation = Quaternion.Euler(body.rotation.eulerAngles + new Vector3(0.0f, 0.0f, Mathf.Deg2Rad * -rotationSpeed * horizontalAxis));

        Vector3 direct = new Vector3(RotHorizontalAxis, RotVerticalAxis, 0.0f).normalized;

        //body.AddForce(direct * speed * Time.deltaTime, ForceMode.Acceleration);
        body.rotation = Quaternion.Slerp(body.rotation, Quaternion.LookRotation(new Vector3(0, 0, 1), direct), 0.1f);



        //transform.Rotate(r, Space.World);
        Debug.Log(m);
        //rotate = new Vector2(0, 0);
    }


}
