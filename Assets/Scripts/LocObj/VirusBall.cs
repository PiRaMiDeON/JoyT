using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusBall : MonoBehaviour
{
    public CapsuleCollider2D capsuleCollider;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRen;
    public AudioSource audioS;
    public ParticleSystem partSys;
    public AudioClip hit;
    public float speed;
    public string vector;
    public bool dontCollideWithGround;
    public SpriteRenderer[] childObjects;

    public void Hit()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        partSys.Play();
        capsuleCollider.enabled = false;
        spriteRen.enabled = false;

        if(childObjects != null)
        {
            for (int i = 0; i < childObjects.Length; i++)
            {
                childObjects[i].enabled = false;
            }
        }

        audioS.PlayOneShot(hit, audioS.volume);
        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "StopPanel")
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == 6)
        {
            if(dontCollideWithGround)
            {
                return;
            }

            audioS.volume /= 3;
            Hit();
        }
    }

    private void Update()
    {

        switch(vector)
        {
            case "Up":
                rb.velocity = new Vector2(0, speed);
                break;

            case "Down":
                rb.velocity = new Vector2(0, -speed);
                break;

            case "Left":
                rb.velocity = new Vector2(-speed, 0);
                break;

            case "Right":
                rb.velocity = new Vector2(speed, 0);
                break;
        }
    }
}
