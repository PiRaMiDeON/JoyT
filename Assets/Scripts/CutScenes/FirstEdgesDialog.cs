using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstEdgesDialog : MonoBehaviour
{
    public GameObject dialog;
    public Animator edgeAnim;
    public Animator dialogAnim;
    public CharacterController2D charCntrl;
    public ToMenuButton toMenuButton;

    private void Start()
    {
        toMenuButton = GameObject.Find("PocketMenu_MenuActivateButton").GetComponent<ToMenuButton>();
        toMenuButton.levelJustStart = true;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            toMenuButton.levelJustStart = false;
            charCntrl.speed = charCntrl.saveSpeed;
            charCntrl.jumpHeight = charCntrl.saveJumpHeight;
            edgeAnim.SetTrigger("Stop");
            dialogAnim.SetTrigger("Stop");
            Destroy(dialog);
            Destroy(gameObject);
        }
    }
}
