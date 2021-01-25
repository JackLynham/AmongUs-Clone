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
}
