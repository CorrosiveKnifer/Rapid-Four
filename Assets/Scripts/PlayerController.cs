using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int ID;

    private Rigidbody body;
    private float speed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalAxis = InputManager.instance.GetHorizontalInput(ID);
        float verticalAxis = InputManager.instance.GetVerticalInput(ID);
        
        transform.Rotate(new Vector3(0.0f, 0.0f, Mathf.Deg2Rad * 10.0f * horizontalAxis), Space.Self);
        
        if(verticalAxis > 1.0f)
        {
            speed += 5.0f * Time.deltaTime;
        }
        else
        {
            speed -= 8.0f * Time.deltaTime;
        }
    }
}
