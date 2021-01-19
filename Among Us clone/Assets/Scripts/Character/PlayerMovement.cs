using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

    public class PlayerMovement : MonoBehaviour
{

    [SerializeField] bool hasControl;

    public static PlayerMovement localPLayer;


    //Variables 
    Rigidbody rb;
    Transform tr;
    Animator anim; 

    //Movment Values
    [SerializeField] InputAction WASD;
    Vector2 movement;
    [SerializeField] float movementSpeed;

    //Color
    static Color color;
    SpriteRenderer mySprite;

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
        
        if(hasControl == true)
        {
            localPLayer = this;
        }

        
        rb = GetComponent<Rigidbody>();
        tr = transform.GetChild(0);
        anim = GetComponent<Animator>();

        mySprite = tr.GetComponent<SpriteRenderer>();
        //If no Color Set to white
        if(color == Color.clear)
        {
            color = Color.white;
            mySprite.color = color;
        }
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

        anim.SetFloat("Speed", movement.magnitude);
    }

    private void FixedUpdate()
    {
        rb.velocity = movement * movementSpeed;
    }

    public void SetColor(Color newColor)
    {
        color = newColor;
        //If Sprite color is not null update the color 
        if(mySprite != null)
        {
            mySprite.color = color;
        }
    }
}
