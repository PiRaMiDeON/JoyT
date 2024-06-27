using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionField : MonoBehaviour
{
    public WhoThisCntrl whoThisCntrl;

    private Animator anim;
    private AudioSource audioS;

    public AnimationClip preparationClip_Phase1;
    public AnimationClip preparationClip_Phase2;

    [HideInInspector] public bool fieldActive;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    public IEnumerator ActivateExplosionField(bool Phase_2)
    {
        fieldActive = true;

        Preparation(Phase_2);

        if(Phase_2)
        {
            yield return new WaitForSeconds(preparationClip_Phase2.length - 1);
        }
        else
        {
            yield return new WaitForSeconds(preparationClip_Phase1.length - 1);
        }

        ActivateField(Phase_2);

        yield return new WaitForSeconds(1);

        fieldActive = false;
    }

    private void Preparation(bool Phase_2)
    {
        if(Phase_2)
        {
            anim.SetTrigger("Preparation_Phase2");
        }
        else
        {
            anim.SetTrigger("Preparation_Phase1");
        }

        audioS.PlayOneShot(whoThisCntrl.countDown, audioS.volume);
    }
    private void ActivateField(bool Phase_2)
    {
        if(Phase_2)
        {
            audioS.PlayOneShot(whoThisCntrl.thirdAttack_Phase2_Sound, audioS.volume);
            StartCoroutine(whoThisCntrl.StartShake());
        }
        else
        {
            audioS.PlayOneShot(whoThisCntrl.thirdAttack_Phase1_Sound, audioS.volume);
            StartCoroutine(whoThisCntrl.StartShake());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out CharacterController2D player))
        {
            player.Dead();
        }
    }

    public void UnActiveField()
    {
        fieldActive = false;
    }

}
