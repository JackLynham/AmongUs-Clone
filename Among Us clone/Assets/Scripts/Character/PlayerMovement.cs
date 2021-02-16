using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
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

    List<PlayerMovement> targets;
    [SerializeField] Collider myCollider;

    bool isDead;

    [SerializeField] GameObject bodyPrefab;

    public static List<Transform> allBodies;

    List<Transform> bodiesFound;  //incase of multiple bodies 

    [SerializeField] InputAction REPORT;
    [SerializeField] LayerMask ignoreForBody;

    private void Awake()
    {
        KILL.performed += KillTarget;
        REPORT.performed += ReportBody;
    }

    private void OnEnable()
    {
        WASD.Enable();
        KILL.Enable();
        REPORT.Enable();
    }

    private void OnDisable()
    {
        WASD.Disable();
        KILL.Disable();
        REPORT.Disable();
    }


    private void Start()
    {

        if (hasControl == true)
        {
            localPLayer = this;
        }
        //init Our targets list
        targets = new List<PlayerMovement>();

        rb = GetComponent<Rigidbody>();
        tr = transform.GetChild(0);
        anim = GetComponent<Animator>();

        mySprite = tr.GetComponent<SpriteRenderer>();

        //If no Color Set to white
        if (color == Color.clear)

            color = Color.white;

        if (!hasControl)

            return;
        mySprite.color = color;

        //Init list
        allBodies = new List<Transform>();

        bodiesFound = new List<Transform>();
    }

    private void Update()
    {
        if (!hasControl)
            return;
        movement = WASD.ReadValue<Vector2>();
        anim.SetFloat("Speed", movement.magnitude);
        //Are we moving left or right 
        if (movement.x != 0)
        {
            //Always returns a value of -1 or 1 So players dont get Squished 
            tr.localScale = new Vector2(Mathf.Sign(movement.x), 1);
        }
        //At least One body in the Scene
        if (allBodies.Count > 0)
        {
            BodySearch();
        }

    }

    private void FixedUpdate()
    {
        rb.velocity = movement * movementSpeed;
    }

    public void SetColor(Color newColor)
    {
        color = newColor;
        //If Sprite color is not null update the color 
        if (mySprite != null)
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
        if (other.tag == "Player")
        {
            //Get Movement Script from the other player 
            PlayerMovement tempTarget = other.GetComponent<PlayerMovement>();

            if (isImposter)
            {
                if (tempTarget.isImposter)
                    //If Target player is imposter dont kill him 
                    return;
                else
                {
                    //Adds players to list
                    targets.Add(tempTarget);
                }

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //removes A player from the Kill list if they move out of range 
        if (other.tag == "Player")
        {
            PlayerMovement tempTarget = other.GetComponent<PlayerMovement>();
            if (targets.Contains(tempTarget))
            {
                targets.Remove(tempTarget);
            }
        }
    }

    void KillTarget(InputAction.CallbackContext context)
    {
        //Checks to see if kill has been preformed 
        if (context.phase == InputActionPhase.Performed)
        {
            if (targets.Count == 0)
                return;

            else
            {//Always one more than the index by default 
                if (targets[targets.Count - 1].isDead)
                    return;
                transform.position = targets[targets.Count - 1].transform.position;
                targets[targets.Count - 1].Die();
                targets.RemoveAt(targets.Count - 1);

            }
        }
    }

    public void Die()
    {
        isDead = true;
        anim.SetBool("IsDead", isDead);
        gameObject.layer = 10;
        myCollider.enabled = false;
        //Gets object and makes sure it has the script attached 
        Deadbody tempBody = Instantiate(bodyPrefab, transform.position,
            transform.rotation).GetComponent<Deadbody>();

        tempBody.SetColor(mySprite.color);

    }

    void BodySearch()
    {
        foreach (Transform body in allBodies)
        {
            RaycastHit hit;
            //The direction between the Player and a body 
            Ray ray = new Ray(transform.position, body.position - transform.position);
            Debug.DrawRay(transform.position, body.position - transform.position, Color.cyan);

            //If it Finds a body it returns true 
            if (Physics.Raycast(ray, out hit, 1000f, ~ignoreForBody))
            {
                //Double checking the the list contains the body
                if (hit.transform == body)
                {
                    //If it does contain carry on
                    if (bodiesFound.Contains(body.transform))

                        return;
                    //If not Add the body 
                    bodiesFound.Add(body.transform);
                }
                //if not in line of sight remove from list
                else
                {
                    bodiesFound.Remove(body.transform);
                }



            }

        }
    }


    private void ReportBody(InputAction.CallbackContext obj)
    {
        if (bodiesFound == null)
            return;

        if(bodiesFound.Count == 0)
        
            return;
        //If Body List has more than one Body it will carry on
        //If the Body is found it needs to be removed from the list 
        Transform tempBody = bodiesFound[bodiesFound.Count - 1];
        allBodies.Remove(tempBody);
        bodiesFound.Remove(tempBody);
        tempBody.GetComponent<Deadbody>().Report();
           


    }

}
