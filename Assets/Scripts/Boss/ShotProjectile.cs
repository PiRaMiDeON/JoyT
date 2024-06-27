using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;

public class ShotProjectile : MonoBehaviour
{
    public Rigidbody2D rb;
    public AudioSource audioS;
    public CapsuleCollider2D _collider;
    public SpriteRenderer spriteRen;
    public ParticleSystem explosionParticles;
    public AudioClip explosionSound;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
        _collider = GetComponent<CapsuleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision)
        {
            if(collision.TryGetComponent(out CharacterController2D player))
            {
                collision.GetComponent<CharacterController2D>().Dead();
                ProjectileDestroy();
                return;
            }

            if(collision.transform.tag == "StopPanel")
            {
                Destroy(gameObject);
            }
        }
    }

    public void ProjectileDestroy()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        explosionParticles.Play();
        _collider.enabled = false;
        spriteRen.enabled = false;
        audioS.PlayOneShot(explosionSound, audioS.volume);
        Destroy(gameObject, 3);
    }

}
