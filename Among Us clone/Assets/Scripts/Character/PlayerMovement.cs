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

    //Roles
    [SerializeField] bool isImposter;
    [SerializeField] InputAction KILL;

    PlayerMovement target;
    [SerializeField] Collider myCollider;

    bool isDead;


    private void Awake()
    {
        KILL.performed += KillTarget; 
    }

    private void OnEnable()
    {
        WASD.Enable();
        KILL.Enable();
    }
    
    private void OnDisable()
    {
        WASD.Disable();
        KILL.Disable();
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
        
            color = Color.white; 
         
        if(!hasControl)
        
            return;
        mySprite.color = color;
      
    }

    private void Update()
    {
        if (!hasControl)
            return;
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

    public void SetRole(bool newRole)
    {
        isImposter = newRole;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag== "Player")
        {
            //Get Movement Script from the other player 
            PlayerMovement tempTarget = other.GetComponent<PlayerMovement>();

            if(isImposter)
            {
                if (tempTarget.isImposter)
                    //If Target player is imposter dont kill him 
                    return;
                else
                {
                    target = tempTarget;
                }
                
            }
        }
    }
     void KillTarget(InputAction.CallbackContext context)
    {
        //Checks to see if kill has been preformed 
        if(context.phase == InputActionPhase.Performed)
        {
            if (target == null)
                return;

            else
            {
                if (target.isDead)
                    return;
                transform.position = target.transform.position;
                target.Die();
                target = null;

            }
        }
    }

    public void Die()
    {
        isDead = true;
        anim.SetBool("IsDead", isDead);
        myCollider.enabled = false;
    }



}
