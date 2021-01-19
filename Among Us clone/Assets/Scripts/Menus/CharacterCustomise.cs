using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterCustomise : MonoBehaviour
{
    [SerializeField] Color[] allColors;
   public Canvas canvas;

    public void SetColors(int colorIndex)

    {
        PlayerMovement.localPLayer.SetColor(allColors[colorIndex]);
    }
    public void CloseMenu()
    {
        canvas.GetComponent<Canvas>();
        canvas.enabled = false;
    }

    public void OpenMenu()
    {
        canvas.GetComponent<Canvas>();
        canvas.enabled = true;
    }

    public void NextScene(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }
}
