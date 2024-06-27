using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor;

public class Hand : MonoBehaviour
{
    public WhoThisCntrl whoThisCntrl;
    public float shakeValue, shakeIntensity, shakeTime;

    public AudioClip[] punchSounds;
    public AudioClip attackSound;
    public AudioSource secondAudioSource;
    public Sprite punch;
    public Sprite[] sprites;
    public Animator spriteAnim;
    public SpriteRenderer empthySprite;
    private int soundIndex, lastSoundIndex = -1;

    public float punchSpeed, moveSpeed;

    public GameObject player;
    public Animator sectoringAnim;
    public GameObject mainHandPosition;
    private Vector2 punchTarget;

    [HideInInspector] public bool isAttacking;

    private AudioSource audioS;
    private CircleCollider2D circleCollider;
    private SpriteRenderer spriteRen;

    private void Start()
    {
        spriteRen = GetComponent<SpriteRenderer>();
        audioS = GetComponent<AudioSource>();
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.enabled = false;
    }

    private void Update()
    {
        if (!isAttacking)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);

            transform.position = Vector2.MoveTowards(transform.position, mainHandPosition.transform.position, moveSpeed);
        }
    }

    public IEnumerator Attack(float preparationTime)
    {
        isAttacking = true;

        Preparation();

        yield return new WaitForSeconds(preparationTime);

        punchTarget = player.transform.position;

        StartCoroutine(whoThisCntrl.StartShake());

        do
        {
            soundIndex = Random.Range(0, punchSounds.Length);
        } while (soundIndex == lastSoundIndex);

        lastSoundIndex = soundIndex;

        secondAudioSource.PlayOneShot(punchSounds[soundIndex], secondAudioSource.volume);
        circleCollider.enabled = true;
        StartCoroutine(Punch(punchSpeed));
    }

    public void Preparation()
    {
        sectoringAnim.SetTrigger("Preparation");
        spriteRen.sprite = null;
        empthySprite.sprite = punch;
    }

    public IEnumerator Punch(float punchSpeed)
    {
        spriteAnim.SetTrigger("Punch");
        audioS.PlayOneShot(attackSound, audioS.volume);

        if (whoThisCntrl.phase2)
        {
            gameObject.transform.DOMove(punchTarget, punchSpeed);

            yield return new WaitForSeconds(punchSpeed);

            gameObject.transform.DOMove(mainHandPosition.transform.position, punchSpeed / 2);

            yield return new WaitForSeconds(punchSpeed / 2);
        }
        else
        {
            gameObject.transform.DOMove(punchTarget, punchSpeed);

            yield return new WaitForSeconds(punchSpeed);

            gameObject.transform.DOMove(mainHandPosition.transform.position, punchSpeed / 2);

            yield return new WaitForSeconds(punchSpeed / 2);
        }

        circleCollider.enabled = false;
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out CharacterController2D player))
        {
            player.Dead();
        }
    }

    public void ChangeHandsSprite(int spriteIndex)
    {
        empthySprite.sprite = null;
        spriteRen.sprite = sprites[spriteIndex];
    }

    public void Disactivate()
    {
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public IEnumerator StopAttacking()
    {
        ChangeHandsSprite(0);
        circleCollider.enabled = false;
        gameObject.transform.DOMove(mainHandPosition.transform.position, punchSpeed / 2);

        yield return new WaitForSeconds(punchSpeed / 2);

        isAttacking = false;
    }
}
