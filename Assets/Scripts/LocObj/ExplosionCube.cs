using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExplosionCube : MonoBehaviour
{
    private AudioSource audioS;
    private Animator anim;
    private BoxCollider2D boxColl;
    private PolygonCollider2D triggerCollider;
    private SpriteRenderer spriteRen;

    public CinemachineVirtualCamera vcam;
    public float shakeIntensity, shakeValue, shakeTime;

    public bool detonationIfCollision;
    public bool detonationWithBoss;

    public AudioClip explosionSound;
    public AudioClip countdownSound;
    public ParticleSystem explosionParticles;

    private int lifeTime = 5;
    public float explosionRadius;

    private Collider2D radiusCircle;
    public LayerMask playerLayer;

    private void Start()
    {
        vcam = GameObject.Find("CM vcam2").GetComponent<CinemachineVirtualCamera>();
        spriteRen = GetComponent<SpriteRenderer>();
        boxColl = GetComponent<BoxCollider2D>();
        triggerCollider = GetComponent<PolygonCollider2D>();
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
        StartCoroutine(LifeTimer());
    }

    public IEnumerator LifeTimer()
    {
        yield return new WaitForSeconds(lifeTime);

        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        anim.SetTrigger("Countdown");
        audioS.PlayOneShot(countdownSound, audioS.volume);

        yield return new WaitForSeconds(5f);

        if (spriteRen.enabled)
        {
            StartCoroutine(Explosion());
        }
    }

    public IEnumerator Explosion()
    {
        triggerCollider.enabled = false;
        boxColl.enabled = false;
        spriteRen.enabled = false;
        StartCoroutine(StartShake());
        explosionParticles.Play();
        audioS.PlayOneShot(explosionSound, audioS.volume);

        radiusCircle = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y), explosionRadius, playerLayer);

        if (radiusCircle)
        {
            if (radiusCircle.TryGetComponent(out CharacterController2D player))
            {
                player.Dead();
            }

            if (radiusCircle.TryGetComponent(out Enemy enemy))
            {
                enemy.Hit();
            }

            if (radiusCircle.TryGetComponent(out Enemy2 enemy2))
            {
                enemy2.healtPoints = 0;
                StartCoroutine(enemy2.Hit());
            }
        }

        yield return new WaitForSeconds(shakeTime);

        Destroy(gameObject, 2);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Abyss")
        {
            Destroy(gameObject, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (detonationIfCollision)
        {
            if (collision.TryGetComponent(out CharacterController2D player) && !detonationWithBoss)
            {
                StartCoroutine(Explosion());
            }
        }
    }

    public void ShakeCamera(float amplitudeGain, float frequencyGain)
    {
        vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitudeGain;

        vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequencyGain;
    }

    public IEnumerator StartShake()
    {
        ShakeCamera(shakeValue, shakeIntensity);

        yield return new WaitForSeconds(shakeTime);

        ShakeCamera(0, 0);
    }
}
