using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MouseHighlighted_Destroy : MonoBehaviour
{
    public AudioClip sound;
    private AudioSource audioS;
    public PlayBttn playBttn;
    private Animator anim;
    public int lvlIndex;
    private void Start()
    {
        audioS = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    private void LvlAnim()
    {
        anim.SetBool("Highlighted", true);
    }

    private void LvlDeAnim()
    {
        anim.SetBool("Highlighted", false);
    }


    private void OnMouseEnter()
    {
        LvlAnim();
    }

    private void OnMouseExit()
    {
        LvlDeAnim();
    }

    private void OnMouseDown()
    {
        playBttn.sceneLvlIndex = lvlIndex;
        audioS.PlayOneShot(sound, audioS.volume);
    }

}
