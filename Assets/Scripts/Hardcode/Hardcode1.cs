using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hardcode1 : MonoBehaviour
{
    public AudioClip boom;
    private AudioSource audioS;
    private BoxCollider2D boxColl;
    private SpriteRenderer spriteRen;
    public ParticleSystem partSys;
    public float gravityForce;
    private Rigidbody2D rb;
    public GameObject cube_prefab;
    public Transform spawnPointCubes;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
        boxColl = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        spriteRen = GetComponent<SpriteRenderer>();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Spike" || collision.transform.tag == "Abyss")
        {
            StartCoroutine(CubeDestroy());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.transform.tag)
        {
            case ("Enemy"):
                collision.GetComponent<Enemy>().Hit();
                break;

            case ("Enemy2"):
                StartCoroutine(collision.GetComponent<Enemy2>().Hit());
                StartCoroutine(CubeDestroy());
                break;
        }
    }

    private IEnumerator CubeDestroy()
    {
        Instantiate(cube_prefab, spawnPointCubes);
        audioS.PlayOneShot(boom, audioS.volume);
        boxColl.enabled = false;
        spriteRen.enabled = false;
        partSys.Play();
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
