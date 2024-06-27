using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClearWave : MonoBehaviour
{
    public Animator waveBatteryAnimator;
    private Animator anim;
    private Collider2D clearCircle;
    private AudioSource audioS;

    public CharacterController2D player;
    public Transform circleColliderPoint;
    public float colliderRadius;
    public LayerMask enemyLayer;
    public AudioClip waveSound;
    public AudioClip errorWaveSound;

    [SerializeField] private float waveCooldownTime;
    public bool waveIsCooldowning;
    public BossCntrl_phase1 bossCntrl;

    private void Start()
    {
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
    }

    private void Update()
    {
        clearCircle = Physics2D.OverlapCircle(new Vector2(circleColliderPoint.position.x, circleColliderPoint.position.y), colliderRadius, enemyLayer);

        if(clearCircle)
        {
            if(clearCircle.TryGetComponent(out Enemy enemy))
            {
                if (enemy.isDead)
                {
                    return;
                }
                else
                {
                    enemy.Hit();
                    return;
                }
            }

            if(clearCircle.TryGetComponent(out SpawnEnemy spawnedEnemy))
            {
                if (spawnedEnemy.isDead)
                {
                    return;
                }
                else
                {
                    spawnedEnemy.Hit();
                    return;
                }
            }

            if(clearCircle.TryGetComponent(out Enemy2 enemy2))
            {
                if(enemy2.isDead)
                {
                    return;
                }
                else
                {
                    StartCoroutine(enemy2.Hit());
                    return;
                }
            }
            
            if(clearCircle.TryGetComponent(out SpawnEnemy2 spawnEnemy2))
            {
                if (spawnEnemy2.isDead)
                {
                    return;
                }
                else
                {
                    StartCoroutine(spawnEnemy2.Hit());
                    return;
                }
            }

            if(clearCircle.TryGetComponent(out BossHitZone boss) && !bossCntrl.isHitting)
            {
                boss.HitBoss();
            }
        }
    }

    /*private void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawWireDisc(gameObject.transform.position, Vector3.back, colliderRadius);
    }*/

    public IEnumerator ActivateWave()
    {
        waveIsCooldowning = true;
        audioS.PlayOneShot(waveSound, audioS.volume);
        anim.SetBool("Clear", true);
        waveBatteryAnimator.SetBool("Cooldown", true);

        yield return new WaitForSeconds(waveCooldownTime);

        waveBatteryAnimator.SetBool("Cooldown", false);
        anim.SetBool("Clear", false);
        waveIsCooldowning = false;
    }

    public void ClearWaveError()
    {
        audioS.PlayOneShot(errorWaveSound, audioS.volume);
    }
}
