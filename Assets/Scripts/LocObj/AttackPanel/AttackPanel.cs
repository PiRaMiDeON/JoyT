using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPanel : MonoBehaviour
{
    public AttackPanelController attackPanelCntrl;
    public AudioClip pressSound;

    private bool panelIsActive;
    private AudioSource audioS;
    private Animator anim;
    private bool playerNear;

    private void Start()
    {
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (playerNear && !panelIsActive && Input.GetKeyDown(KeyCode.F))
        {
            audioS.PlayOneShot(pressSound, audioS.volume);
            anim.SetBool("Press", true);
            panelIsActive = true;
            ChargeCntrl();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            playerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            playerNear = false;
        }
    }

    private void ChargeCntrl()
    {
        attackPanelCntrl.currentValue++;
    }

    public void ResetAttackPanel()
    {
        anim.SetBool("Press", false);
        panelIsActive = false;
    }

}
