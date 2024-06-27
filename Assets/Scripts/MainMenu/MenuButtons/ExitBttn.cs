using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitBttn : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Press()
    {
        anim.SetTrigger("Pressed");
    }

    private void ExitGame()
    {
        Application.Quit();
    }
    private void OnMouseDown()
    {
        Press();
        ExitGame();
    }
}
