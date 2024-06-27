using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy2 : MonoBehaviour
{
    public AudioClip preDead;
    public AudioClip dead;
    public AudioClip stun;
    private AudioSource audioS;
    private Rigidbody2D rb;
    public Collider2D[] colliders;
    public ParticleSystem[] playSys;
    public int healtPoints;
    public List<Transform> points;
    public float speed;
    private Animator animator;
    private int currentIndex;
    private Vector2 currentPoint;
    private bool walking;
    [HideInInspector] public bool isDead;
    private bool isStunned;
    public float stunTime;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentPoint = points[0].position;
        walking = true;
        ChooseDirection();
        healtPoints = 3;
    }

    private void Walk()
    {
        animator.SetBool("Walk", walking);

        if (walking)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, currentPoint, step);
            if (Vector3.Distance(transform.position, currentPoint) < 0.01f)
            {
                StartCoroutine(Stay());
            }
        }
    }

    private void Update()
    {
        if (isDead == true)
        {
            return;
        }

        if (isStunned == true)
        {
            return;
        }

        Walk();
    }

    private void ChooseNextPoint()
    {
        currentIndex = ++currentIndex < points.Count ? currentIndex : 0;

        currentPoint = points[currentIndex].position;

        ChooseDirection();
    }

    private void ChooseDirection()
    {
        GetComponent<SpriteRenderer>().flipX = currentPoint.x < transform.position.x;
    }

    private IEnumerator Stay()
    {
        walking = false;
        animator.SetTrigger("Stay");
        ChooseNextPoint();

        yield return new WaitForSeconds(1);

        walking = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out CharacterController2D controller))
        {
            controller.Dead();
        }
    }

    public IEnumerator Hit()
    {
        if (healtPoints <= 0)
        {
            audioS.PlayOneShot(preDead, audioS.volume);
            isDead = true;
            animator.SetBool("Walk", false);
            animator.SetTrigger("IsDead");
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

            for (int i = 0; i < colliders.Length; i++)
            {
                Destroy(colliders[i]);
            }

            yield return new WaitForSeconds(3f);
            StartCoroutine(Dead());
            yield break;
        }

        playSys[0].Play();
        StartCoroutine(Stun());
        yield break;
    }

    public IEnumerator Stun()
    {
        audioS.PlayOneShot(stun, audioS.volume);
        isStunned = true;
        animator.SetBool("Walk", false);
        animator.SetBool("Stun", true);

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForSeconds(stunTime);

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.bodyType = RigidbodyType2D.Dynamic;
        isStunned = false;
        animator.SetBool("Stun", false);
        animator.SetBool("Walk", true);

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = true;
        }

        healtPoints--;
    }

    public IEnumerator Dead()
    {
        audioS.PlayOneShot(dead, audioS.volume);
        playSys[1].Play();
        GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }
}
