using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallGates : MonoBehaviour
{
    private Animator anim;
    private AudioSource audioS;
    public AudioClip unlock_sound;
    public AudioClip lock_sound;
    public bool lvl_12;
    private bool volumeFixed;
    public bool dontEnableBoxColliderAfterScriptEvent;

    private void Start()
    {
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.TryGetComponent(out CharacterController2D player))
        {
            UnlockGates();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out CharacterController2D player))
        {
            LockGates();
        }
    }

    public void UnlockGates()
    {
        if(lvl_12 && !volumeFixed)
        {
            volumeFixed = true;
            audioS.volume *= 0.5f;
        }

        anim.SetBool("Unlocked", true);
        audioS.PlayOneShot(unlock_sound, audioS.volume);
    }
    
    public void LockGates()
    {
        if (lvl_12 && !volumeFixed)
        {
            volumeFixed = true;
            audioS.volume *= 0.5f;
        }

        anim.SetBool("Unlocked", false);
        audioS.PlayOneShot(lock_sound, audioS.volume);
    }
}
