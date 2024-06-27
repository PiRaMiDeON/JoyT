using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public AudioClip boom;
    private AudioSource audioS;
    private BoxCollider2D boxColl;
    private SpriteRenderer spriteRen;
    public ParticleSystem partSys;
    public float gravityForce;
    [HideInInspector] public Rigidbody2D rb;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
        boxColl = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        spriteRen = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch(collision.transform.tag)
        {
            case ("GravityWaveUp"):
                GWU();
                break;

            case ("GravityWaveDown"):
                GWD();
                break;

            case ("GravityWaveRight"):
                GWR();
                break;

            case ("GravityWaveLeft"):
                GWL();
                break;
        }
    }

    private void GWU()
    {
        rb.velocity = new Vector2(rb.velocity.x, gravityForce);
    }

    private void GWD()
    {
        rb.velocity = new Vector2(rb.velocity.x, -gravityForce);
    }

    private void GWR()
    {
        rb.velocity = new Vector2(gravityForce, rb.velocity.y);
    }

    private void GWL()
    {
        rb.velocity = new Vector2(-gravityForce, rb.velocity.y);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Spike")
        {
            StartCoroutine(CubeDestroy());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Abyss")
        {
            StartCoroutine(CubeDestroy());
        }
    }

    public IEnumerator CubeDestroy()
    {
        audioS.PlayOneShot(boom, audioS.volume);
        boxColl.enabled = false;
        spriteRen.enabled = false;
        partSys.Play();
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
