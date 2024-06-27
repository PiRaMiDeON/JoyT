using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataStream : MonoBehaviour
{
    private AudioSource audioS;
    public AudioClip contact_Sound;
    private void Start()
    {
        audioS = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out CharacterController2D player))
        {
            player.Dead();
            audioS.PlayOneShot(contact_Sound, audioS.volume);
        }
    }
}
