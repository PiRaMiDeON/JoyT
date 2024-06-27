using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform secondPortalExit;
    public ParticleSystem portalBurst;
    public ParticleSystem virusDetected;
    private AudioSource audioS;
    public AudioClip teleport;
    public AudioClip enemyDead;
    private void Start()
    {
        audioS = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        switch(collision.transform.tag)
        {
            case ("Player"):
                collision.GetComponent<Transform>().position = secondPortalExit.position;
                portalBurst.Play();
                audioS.PlayOneShot(teleport, audioS.volume);
                break;

            case ("Cube"):
                collision.GetComponent<Transform>().position = secondPortalExit.position;
                portalBurst.Play();
                audioS.PlayOneShot(teleport, audioS.volume);
                break;

            case ("Enemy"):
                collision.GetComponent<Enemy>().Hit();
                audioS.PlayOneShot(enemyDead, audioS.volume);
                virusDetected.Play();
                break;

            case ("Enemy2"):
                collision.GetComponent<Enemy2>().healtPoints = 0;
                StartCoroutine(collision.GetComponent<Enemy2>().Hit());
                audioS.PlayOneShot(enemyDead, audioS.volume);
                virusDetected.Play();
                break;
        }
        
    }
}
