using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class GatesCntrl : MonoBehaviour
{
    private Animator anim;
    private AudioSource audioS;
    public AudioClip gatesUnlocking_Sound;
    public GameObject[] emailBots;
    public ScriptEvent scriptEvent;
    private bool botsDead;

    private void Start()
    {
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(emailBots.Length != 0 && !botsDead)
        {
            for (int i = 0; i < emailBots.Length; i++)
            {
                if (emailBots[i] != null)
                {
                    return;
                }
            }

            botsDead = true;
            Unlocked();
        }
    }

    public void Unlocked()
    {
        anim.SetTrigger("Activate");
        audioS.PlayOneShot(gatesUnlocking_Sound, audioS.volume);

        if(scriptEvent != null)
        {
            scriptEvent.StartEvent();
        }
    }
}
