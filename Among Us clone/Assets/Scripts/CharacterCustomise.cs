using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterCustomise : MonoBehaviour
{
    [SerializeField] Color[] allColors;

    public void SetColors(int colorIndex)

    {
        PlayerMovement.localPLayer.SetColor(allColors[colorIndex]);
    }

    public void NextScene(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }
}
