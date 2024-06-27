using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public AudioClip contactSound;
    private AudioSource audioS;
    public bool mute;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();

        if(!mute)
        { 
        audioS.Play();
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.transform.tag)
        {
            case "Player":
                audioS.PlayOneShot(contactSound, audioS.volume * 3);
                collision.GetComponent<CharacterController2D>().Dead();
                break;

            case "Cube":
                audioS.PlayOneShot(contactSound, audioS.volume * 3);
                StartCoroutine(collision.GetComponent<Cube>().CubeDestroy());
                break;

            case "Enemy":
                audioS.PlayOneShot(contactSound, audioS.volume * 3);
                collision.GetComponent<Enemy>().Hit();
                break;

            case "Enemy2":
                audioS.PlayOneShot(contactSound, audioS.volume * 3);
                StartCoroutine(collision.GetComponent<Enemy2>().Dead());
                break;
        }
    }

    private void Update()
    {
        if(audioS.volume > 0.1f)
        {
            audioS.volume = 0.1f;
        }
    }
}
