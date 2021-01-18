using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

    public class PlayerMovement : MonoBehaviour
{
    //Variables 
    Rigidbody rb;
    Transform tr;
    //Movment Values
    [SerializeField] InputAction WASD;
    Vector2 movement;
    [SerializeField] float movementSpeed;

    private void OnEnable()
    {
        WASD.Enable();

    }


    private void OnDisable()
    {
        WASD.Disable();
    }


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = transform.GetChild(0);
    }

    private void Update()
    {
        movement = WASD.ReadValue<Vector2>();

        //Are we moving left or right 
        if (movement.x != 0)
        {
            //Always returns a value of -1 or 1 So players dont get Squished 
            tr.localScale = new Vector2(Mathf.Sign(movement.x), 1);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = movement * movementSpeed;
    }


}
