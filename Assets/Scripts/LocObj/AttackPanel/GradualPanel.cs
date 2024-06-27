using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradualPanel : MonoBehaviour
{
    public GradualPanel previousPanel;

    private AudioSource audioS;
    public AudioClip press_sound;

    private Animator anim;
    private bool playerIsNear;
    [HideInInspector] public bool panelActive;

    public Animator[] gameObjectAnimators;
    public string animationBoolName;

    public DialogEdgesActivator DEA;
    public GameObject fireSound;

    public ScriptEvent scriptEvent;

    private void Start()
    {
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(playerIsNear && Input.GetKeyDown(KeyCode.F) &&!panelActive)
        {
            if(previousPanel != null)
            {
                if(previousPanel.panelActive)
                {
                    if(fireSound != null)
                    {
                        fireSound.SetActive(true);
                    }

                    audioS.PlayOneShot(press_sound, audioS.volume);
                    DEA.DialogActivate();
                    DEA.EdgesActivate();
                    panelActive = true;
                    
                    if(scriptEvent != null)
                    {
                        scriptEvent.StartEvent();
                    }

                    anim.SetBool("Press", true);

                    for (int i = 0; i < gameObjectAnimators.Length; i++)
                    {
                        gameObjectAnimators[i].SetBool(animationBoolName, true);
                    }
                }
            }
            else
            {
                if (fireSound != null)
                {
                    fireSound.SetActive(true);
                }

                audioS.PlayOneShot(press_sound, audioS.volume);
                DEA.DialogActivate();
                DEA.EdgesActivate();
                panelActive = true;
                anim.SetBool("Press", true);

                for (int i = 0; i < gameObjectAnimators.Length; i++)
                {
                    gameObjectAnimators[i].SetBool(animationBoolName, true);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out CharacterController2D player))
        {
            playerIsNear = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out CharacterController2D player))
        {
            playerIsNear = false;
        }
    }
}
