using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deadbody : MonoBehaviour
{

    [SerializeField] SpriteRenderer bodySprite;

    public void SetColor (Color newColor )
    {
        bodySprite.color = newColor;  // Sets body as same color as player 
    }

    private void OnEnable()
    {
        if(PlayerMovement.allBodies != null)
        {
            PlayerMovement.allBodies.Add(transform);
        }
    }

    public void Report()
    {
        Debug.Log("Body Reported"); 
        Destroy(gameObject);
    }
}   
