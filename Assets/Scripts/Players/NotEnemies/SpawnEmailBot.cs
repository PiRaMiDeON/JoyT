using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEmailBot : MonoBehaviour
{
    public AudioClip dead_Sound;
    public List<Transform> points;
    public ParticleSystem deathPartSys;
    public GameObject deathMessage;
    public float speed;

    private AudioSource audioS;
    private CapsuleCollider2D bodyCollider;
    private SpriteRenderer spriteRen;
    private Animator animator;

    private int currentIndex;
    private Vector2 currentPoint;
    [HideInInspector] public bool walking;
    private bool isDead;
    public bool clearingEnemy;

    private Collider2D checkCircle;
    public Animator clearWaveAnim;
    public Transform circleColliderPoint;
    public LayerMask enemyLayer;
    public float checkColliderRadius;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        currentPoint = points[0].position;
        walking = true;
        ChooseDirection();
    }

    public void Walk()
    {
        clearWaveAnim.SetBool("Clear", false);
        animator.SetBool("Walk", walking);
        clearingEnemy = false;

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

        checkCircle = Physics2D.OverlapCircle(new Vector2(circleColliderPoint.position.x, circleColliderPoint.position.y), checkColliderRadius, enemyLayer);

        if (checkCircle)
        {

            if (checkCircle.TryGetComponent(out Enemy enemyBot) && !clearingEnemy)
            {
                clearingEnemy = true;
                clearWaveAnim.SetBool("Clear", true);
            }
            
            if (checkCircle.TryGetComponent(out SpawnEnemy spawnedEnemyBot) && !clearingEnemy)
            {
                clearingEnemy = true;
                clearWaveAnim.SetBool("Clear", true);
            }
        }
        else
        {
            Walk();           
        }     
    }

    /*private void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawWireDisc(gameObject.transform.position, Vector3.back, checkColliderRadius);
    }*/

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
        ChooseNextPoint();

        yield return new WaitForSeconds(1);

        walking = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Spike")
        {
            Dead();
        }
    }

    public void Dead()
    {
        isDead = true;
        animator.SetBool("Walk", false);
        deathPartSys.Play();
        spriteRen.enabled = false;
        bodyCollider.enabled = false;
        audioS.PlayOneShot(dead_Sound, audioS.volume);
        deathMessage.SetActive(true);

        Destroy(gameObject, 3);
    }
}
