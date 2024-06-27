using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DialogEdgesActivator : MonoBehaviour
{
    [HideInInspector] public CamAim camAim;
    public GameObject cutScene;
    private bool dialogActive;
    private bool edgesActive;
    [HideInInspector] public Animator edgesAnim;
    [HideInInspector] public Animator dialogAnim;
    [HideInInspector] public CharacterController2D characterController;
    [HideInInspector] private GameObject _interface;
    public bool Lvl_12;

    private void Start()
    {
        _interface = GameObject.Find("Interface");
        dialogAnim = GetComponent<Animator>();
        edgesAnim = GameObject.Find("Edges").GetComponent<Animator>();
        if(Lvl_12)
        {
            characterController = GameObject.Find("Trip(Lvl 12)").GetComponent<CharacterController2D>();
        }
        else
        {
            characterController = GameObject.Find("Trip").GetComponent<CharacterController2D>();
        }
    }
    private void Update()
    {
        if (edgesActive == true || dialogActive == true)
        {
            _interface.SetActive(false);
            PlayerFreeze();
            camAim.camActive = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                dialogAnim.SetBool("Active", false);
                edgesActive = false;
                edgesAnim.SetBool("EdgesMove", false);
                edgesActive = false;
                PlayerUnFreeze();
                camAim.camActive = false;
                _interface.SetActive(true);
                Destroy(cutScene);
            }
        }
    }
    public void EdgesActivate()
    {
        edgesAnim.SetBool("EdgesMove", true);
        edgesActive = true;
    }

    public void DialogActivate()
    {
        dialogAnim.SetBool("Active", true);
        dialogActive = true;
    }


    private void PlayerFreeze()
    {
        characterController.speed = 0f;
        characterController.jumpHeight = 0f;
        characterController.rb.bodyType = RigidbodyType2D.Kinematic;
        characterController.rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void PlayerUnFreeze()
    {
        characterController.speed = characterController.saveSpeed;
        characterController.jumpHeight = characterController.saveJumpHeight;
        characterController.rb.bodyType = RigidbodyType2D.Dynamic;
        characterController.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
