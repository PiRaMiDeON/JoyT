using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WTProjectile_Phase1 : MonoBehaviour
{
    public CharacterController2D player;
    public GameObject glow;
    public float speed;
    public LayerMask playerLayer;
    public SpriteRenderer sectoringField;
    public AudioClip explosion_Sound;
    public ParticleSystem explosionPartSys;
    public ParticleSystem flyPartSys;
    private AudioSource audioS;
    private Vector2 playerPosition;
    private SpriteRenderer spriteRen;
    private PolygonCollider2D _collider;
    private float plusExplosionRadius;
    private Collider2D explosionCollider;
    private Rigidbody2D rb;
    private bool stopFly;
    private bool isExplosiong;

    private void Start()
    {
        player = GameObject.Find("Trip").GetComponent<CharacterController2D>();
        audioS = GetComponent<AudioSource>();
        spriteRen = GetComponent<SpriteRenderer>();
        _collider = GetComponent<PolygonCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        plusExplosionRadius = Random.Range(0.1f, 0.31f);
        sectoringField.transform.localScale = new Vector2(sectoringField.transform.localScale.x + plusExplosionRadius, sectoringField.transform.localScale.y + plusExplosionRadius);
        audioS.Play();
        StartCoroutine(FlyTimer());
        audioS.volume /= 2;
    }

    private void Update()
    {

        if (!stopFly)
        {
            if (playerPosition.x > gameObject.transform.position.x)
            {
                if (transform.localScale.y > 0)
                {
                    transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y * -1);
                }
            }
            else
            {
                if (transform.localScale.y < 0)
                {
                    transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y * -1);
                }
            }

            playerPosition = player.transform.position;
            Vector2 lookDir = playerPosition - rb.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            rb.rotation = angle - 180f;

            transform.position = Vector2.MoveTowards(transform.position, playerPosition, speed);
            
            if (speed > 0)
            {
                speed -= 0.000001f;
            }
        }
        else
        {
            if (isExplosiong)
            {
                return;
            }
            Explosion();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision)
        {
            if(isExplosiong)
            {
                return;
            }
            Explosion();
        }
    }

    private void Explosion()
    {
        isExplosiong = true;
        _collider.enabled = false;
        glow.SetActive(false);
        explosionPartSys.Play();
        flyPartSys.Stop();
        sectoringField.enabled = false;
        spriteRen.enabled = false;
        audioS.Stop();
        audioS.volume *= 2;
        audioS.PlayOneShot(explosion_Sound, audioS.volume);

        explosionCollider = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y), sectoringField.transform.localScale.x * 10, playerLayer);

        if (explosionCollider)
        {
            if (explosionCollider.TryGetComponent(out CharacterController2D player))
            {
                player.Dead();
            }
        }

        StartCoroutine(Dead());
    }


    private IEnumerator FlyTimer()
    {
        yield return new WaitForSeconds(10);

        stopFly = true;
    }

    private IEnumerator Dead()
    {
        yield return new WaitForSeconds(2f);

        gameObject.SetActive(false);
        Destroy(gameObject, 2);
    }
}
