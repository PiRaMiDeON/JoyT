using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public AudioClip hit;
    private AudioSource audioS;
    public List<Transform> points;
    public float speed;
    private Animator animator;
    private int currentIndex;
    private Vector2 currentPoint;
    private bool walking;
    [HideInInspector] public bool isDead;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        currentPoint = points[0].position;
        walking = true;
        ChooseDirection();
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

    public IEnumerator Stay()
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

    public void Hit()
    {
        isDead = true;
        animator.SetBool("Walk", false);
        animator.SetTrigger("IsDead");
        audioS.PlayOneShot(hit, audioS.volume);
        Destroy(GetComponent<Collider2D>());
        Destroy(GetComponent<Rigidbody2D>(), 1);
        Destroy(gameObject, 2);
    }
}
