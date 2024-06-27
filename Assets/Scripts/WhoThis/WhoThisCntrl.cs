using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;
using DG.Tweening;

public class WhoThisCntrl : MonoBehaviour
{
    public CharacterController2D player;
    public Rigidbody2D playerRb;

    public Transform centrePoint;
    public ScriptEvent victory_scriptEvent;
    public ScriptEvent phase2_scriptEvent;
    public ScriptEvent rageMode_scriptEvent;

    public CinemachineVirtualCamera vcam;
    public float shakeValue, shakeIntensity, shakeTime;

    private readonly int activate_1 = Animator.StringToHash("Activate_1");
    private readonly int takeDamage_1 = Animator.StringToHash("TakeDamage_1");
    private readonly int moving_1 = Animator.StringToHash("Moving_1");
    private readonly int firstAttack_1 = Animator.StringToHash("FirstAttack_1");
    private readonly int secondAttack_1 = Animator.StringToHash("SecondAttack_1");
    private readonly int thirdAttack_1 = Animator.StringToHash("ThirdAttack_1");
    private readonly int fourthAttack_1 = Animator.StringToHash("FourthAttack_1");
    private readonly int activate_2 = Animator.StringToHash("Activate_2");
    private readonly int takeDamage_2 = Animator.StringToHash("TakeDamage_2");
    private readonly int moving_2 = Animator.StringToHash("Moving_2");
    private readonly int firstAttack_2 = Animator.StringToHash("FirstAttack_2");
    private readonly int secondAttack_2 = Animator.StringToHash("SecondAttack_2");
    private readonly int thirdAttack_2 = Animator.StringToHash("ThirdAttack_2");
    private readonly int fourthAttack_2 = Animator.StringToHash("FourthAttack_2");
    private readonly int imprevious = Animator.StringToHash("Imprevious");
    private readonly int rageModeActivating = Animator.StringToHash("RageMode");
    private readonly int dead = Animator.StringToHash("Dead");

    public AnimationClip activate_Phase1_Clip, activate_Phase2_Clip;
    public AnimationClip takeDamage_Phase1_Clip, takeDamage_Phase2_Clip;
    public AnimationClip moving_Phase1_Clip, moving_Phase2_Clip;
    public AnimationClip firstAttack_Phase1_Clip, firstAttack_Phase2_Clip;
    public AnimationClip secondAttack_Phase1_Clip, secondAttack_Phase2_Clip;
    public AnimationClip thirdAttack_Phase1_Clip, thirdAttack_Phase2_Clip;
    public AnimationClip fourthAttack_Phase1_Clip, fourthAttack_Phase2_Clip;
    public AnimationClip imprevious_Clip, preDead_Clip, dead_Clip, postDead_Clip, rageModeActivating_Clip;

    public AudioClip[] waitCommand_Sounds;
    public AudioClip preDead_Sound, dead_Sound, postDead_Sound;
    public AudioClip activate_Phase1_Sound, activate_Phase2_Sound;
    public AudioClip takeDamage_Sound;
    public AudioClip postTakeDamage_Sound;
    public AudioClip moving_Phase1_Sound;
    public AudioClip[] moving_Phase2_Sounds;
    private int moveSoundIndex, lastMoveSoundIndex;
    public AudioClip firstAttack_Phase1_Sound, firstAttack_Phase2_Sound;
    public AudioClip thirdAttack_Phase1_Sound, thirdAttack_Phase2_Sound, countDown;
    public AudioClip fourthAttack_Phase1_Sound, fourthAttack_Phase2_Sound;
    public AudioClip fourthAttack_Preparation_Phase1_Sound, fourthAttack_Preparation_Phase2_Sound;
    public AudioClip imprevious_Sound;
    public AudioClip rageModeActivating_Sound;
    public AudioSource secondAudioSource;
    private int waitCommandSoundIndex;
    private int lastWaitCommandSoundIndex = -1;

    public float waitCommandTime;
    private int attackIndex;
    private int lastAttackIndex = 0;
    private int attackCount = 0, gravityIndex = 0, gravityLimit = 3;
    public Animator gravityArrows_anim;
    public float pushPlayerPower;
    public float pushTime;
    private bool pushPlayer;

    public Transform[] expCubeSpawnPosition_Phase1;
    public Transform[] expCubeSpawnPosition_Phase2;
    public float cubeImpulseForce;
    public float cubeRotationTime;
    public float cubeRotationValue;
    public GameObject explosionCube;

    public float moveSpeed;
    public List<MovePoint> possibleDirections;
    private int moveIndex, lastMoveIndex = -1;

    public Hand[] hands;
    public Animator[] hands_anim;
    public float preparationTime;
    public float punchSpeed;
    public float attackSpeed;
    private int lastHandIndex = -1;
    private int spriteIndex, lastSpriteIndex;

    public ExplosionField explosionField;

    public Transform projectileSpawner;
    public GameObject projectile_Phase1;
    public GameObject projectile_Phase2;

    public GameObject healthBar;
    private Animator healthBarAnimator;
    public GameObject[] healthPoints_Phase1;
    public GameObject[] healthPoints_Phase2;

    [HideInInspector] public bool isActive, awake;
    private bool isAttacking, isMoving, isDead, isWaitingCommand, inRageMode, stopAttacking, collide, isImprevious, isStunned, isActivating, returningHealth;
    [HideInInspector] public bool phase2;
    private int health = 10;
    private int healthPointIndex = 0;

    public DamageBossStation damageBossStation;

    private AudioSource audioS;
    private PolygonCollider2D mainCollider;
    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
        mainCollider = GetComponent<PolygonCollider2D>();
        healthBarAnimator = healthBar.GetComponent<Animator>();
    }

    private void Update()
    {
        if (pushPlayer)
        {
            PlayerPush(phase2);
        }

        for (int i = 0; i < hands.Length; i++)
        {
            if (hands[i].isAttacking)
            {
                return;
            }
        }

        if (stopAttacking || explosionField.fieldActive)
        {
            return;
        }

        if (awake)
        {
            if (isActive)
            {
                isActive = false;
                StartCoroutine(Activate(phase2));
                return;
            }

            if (isActivating || isStunned || isMoving || isImprevious || isDead || isAttacking || isWaitingCommand || returningHealth)
            {
                return;
            }

            if (phase2)
            {
                if (attackCount > 0)
                {
                    StartCoroutine(Moving(phase2));
                    return;
                }
            }
            else
            {
                if (attackCount > 1)
                {
                    StartCoroutine(Moving(phase2));
                    return;
                }
            }

            StartCoroutine(WaitCommand(phase2));
        }
    }

    private IEnumerator WaitCommand(bool Phase_2)
    {
        isWaitingCommand = true;

        if (!mainCollider.enabled)
        {
            mainCollider.enabled = true;
        }

        if (collide)
        {
            collide = false;
        }

        if (gravityIndex >= gravityLimit)
        {
            StartCoroutine(PushTime());
            if (phase2)
            {
                gravityArrows_anim.SetTrigger("Phase2");
            }
            else
            {
                gravityArrows_anim.SetTrigger("Phase1");
            }

            gravityIndex = 0;
        }

        if (phase2)
        {
            do
            {
                spriteIndex = Random.Range(0, hands[2].sprites.Length);
            } while (spriteIndex == lastSpriteIndex);
            lastSpriteIndex = spriteIndex;

            hands[2].ChangeHandsSprite(spriteIndex);
            hands[3].ChangeHandsSprite(spriteIndex);
        }
        else
        {
            do
            {
                spriteIndex = Random.Range(0, hands[0].sprites.Length);
            } while (spriteIndex == lastSpriteIndex);
            lastSpriteIndex = spriteIndex;

            hands[0].ChangeHandsSprite(spriteIndex);
            hands[1].ChangeHandsSprite(spriteIndex);
        }

        do
        {
            waitCommandSoundIndex = Random.Range(0, waitCommand_Sounds.Length);
        }
        while (waitCommandSoundIndex == lastWaitCommandSoundIndex);

        lastWaitCommandSoundIndex = waitCommandSoundIndex;
        audioS.PlayOneShot(waitCommand_Sounds[waitCommandSoundIndex]);

        if (Phase_2)
        {
            yield return new WaitForSeconds(waitCommandTime / 2);

            do
            {
                attackIndex = Random.Range(0, 5);
            }
            while (attackIndex == lastAttackIndex);

            lastAttackIndex = attackIndex;

            switch (attackIndex)
            {
                case 0:
                    StartCoroutine(FirstAttack(phase2));
                    break;

                case 1:
                    StartCoroutine(SecondAttack(phase2));
                    break;

                case 2:
                    StartCoroutine(ThirdAttack(phase2));
                    break;

                case 3:
                    StartCoroutine(FourthAttack(phase2));
                    break;

                case 4:
                    StartCoroutine(Imprevios());
                    break;
            }
        }
        else
        {
            yield return new WaitForSeconds(waitCommandTime);

            do
            {
                attackIndex = Random.Range(0, 4);
            }
            while (attackIndex == lastAttackIndex);
            lastAttackIndex = attackIndex;

            switch (attackIndex)
            {
                case 0:
                    StartCoroutine(FirstAttack(phase2));
                    break;

                case 1:
                    StartCoroutine(SecondAttack(phase2));
                    break;

                case 2:
                    StartCoroutine(ThirdAttack(phase2));
                    break;

                case 3:
                    StartCoroutine(FourthAttack(phase2));
                    break;
            }
        }

        gravityIndex++;
        isWaitingCommand = false;
    }

    private IEnumerator Moving(bool Phase_2)
    {
        isMoving = true;

        if (phase2)
        {
            do
            {
                spriteIndex = Random.Range(0, hands[2].sprites.Length);
            } while (spriteIndex == lastSpriteIndex);
            lastSpriteIndex = spriteIndex;

            hands[2].ChangeHandsSprite(spriteIndex);
            hands[3].ChangeHandsSprite(spriteIndex);
        }
        else
        {
            do
            {
                spriteIndex = Random.Range(0, hands[0].sprites.Length);
            } while (spriteIndex == lastSpriteIndex);
            lastSpriteIndex = spriteIndex;

            hands[0].ChangeHandsSprite(spriteIndex);
            hands[1].ChangeHandsSprite(spriteIndex);
        }

        if (Phase_2)
        {
            anim.SetBool(moving_2, true);
        }
        else
        {
            anim.SetBool(moving_1, true);
        }

        if (Phase_2)
        {
            int rand = Random.Range(1, 4);

            while (rand > 0)
            {
                bool _continue = false;

                if (explosionField.fieldActive)
                {
                    _continue = true;

                    yield return new WaitForSeconds(0.1f);
                }

                if (_continue)
                {
                    continue;
                }

                do
                {
                    moveIndex = Random.Range(0, possibleDirections.Count);
                } while (moveIndex == lastMoveIndex);
                lastMoveIndex = moveIndex;

                transform.DOMove(possibleDirections[moveIndex]._transform.position, moveSpeed).SetEase(Ease.InOutBounce);

                rand--;

                do
                {
                    moveSoundIndex = Random.Range(0, moving_Phase2_Sounds.Length);
                } while (moveSoundIndex == lastMoveSoundIndex);

                lastMoveSoundIndex = moveSoundIndex;
                secondAudioSource.PlayOneShot(moving_Phase2_Sounds[moveSoundIndex], audioS.volume);

                yield return new WaitForSeconds(moveSpeed + 0.1f);
            }

        }
        else
        {
            int rand = Random.Range(2, 5);

            while (rand > 0)
            {
                do
                {
                    moveIndex = Random.Range(0, possibleDirections.Count);
                } while (moveIndex == lastMoveIndex);
                lastMoveIndex = moveIndex;

                transform.DOMove(possibleDirections[moveIndex]._transform.position, moveSpeed).SetEase(Ease.InOutQuint);

                rand--;
                secondAudioSource.PlayOneShot(moving_Phase1_Sound, audioS.volume);

                yield return new WaitForSeconds(moveSpeed + 0.1f);
            }
        }

        attackCount = 0;
        anim.SetBool(moving_1, false);
        anim.SetBool(moving_2, false);
        isMoving = false;
    }

    private IEnumerator ReturnHealthPoints(bool rageMode)
    {
        returningHealth = true;
        mainCollider.enabled = false;

        healthPointIndex = 0;

        if (rageMode)
        {
            healthPointIndex = healthPoints_Phase2.Length - 2;

            healthPoints_Phase2[healthPoints_Phase2.Length - 1].SetActive(true);

            yield return new WaitForSeconds(0.2f);

            healthPoints_Phase2[healthPoints_Phase2.Length - 2].SetActive(true);
        }
        else
        {
            for (int i = healthPoints_Phase2.Length - 1; i >= 0; i--)
            {
                healthPoints_Phase2[i].SetActive(true);

                yield return new WaitForSeconds(0.2f);
            }
        }

        mainCollider.enabled = true;
        returningHealth = false;
    }

    private IEnumerator Imprevios()
    {
        isImprevious = true;

        anim.SetBool(imprevious, true);

        audioS.PlayOneShot(imprevious_Sound, audioS.volume);

        yield return new WaitForSeconds(imprevious_Clip.length);

        anim.SetBool(imprevious, false);
        isImprevious = false;
    }

    public IEnumerator TakeDamage()
    {
        isStunned = true;

        isAttacking = false;
        isMoving = false;
        isWaitingCommand = false;

        pushPlayer = false;

        anim.SetBool(moving_1, false);
        anim.SetBool(moving_2, false);
        anim.SetBool(firstAttack_2, false);
        anim.SetBool(firstAttack_1, false);
        anim.SetBool(secondAttack_1, false);
        anim.SetBool(secondAttack_2, false);
        anim.SetBool(thirdAttack_1, false);
        anim.SetBool(thirdAttack_2, false);
        anim.SetBool(fourthAttack_1, false);
        anim.SetBool(fourthAttack_2, false);

        for (int i = 0; i < hands.Length; i++)
        {
            if (hands[i].isAttacking)
            {
                hands[i].StopAllCoroutines();
                StartCoroutine(hands[i].StopAttacking());
            }
        }

        explosionField.UnActiveField();

        attackCount = 0;

        health--;
        mainCollider.enabled = false;

        audioS.PlayOneShot(takeDamage_Sound, audioS.volume);
        secondAudioSource.PlayOneShot(postTakeDamage_Sound, audioS.volume);

        if (health <= 0)
        {
            if (phase2)
            {
                if (inRageMode)
                {
                    StartCoroutine(Dead());
                    healthPoints_Phase2[healthPointIndex].SetActive(false);
                    healthBarAnimator.SetTrigger("Dead");
                    isStunned = false;
                    yield break;
                }
                else
                {
                    inRageMode = true;

                    healthPoints_Phase2[healthPointIndex].SetActive(false);
                    healthBarAnimator.SetTrigger("RageMode");
                    StartCoroutine(ReturnHealthPoints(inRageMode));
                    health = 2;
                    isActive = true;
                    isStunned = false;
                    yield break;
                }
            }
            else
            {
                phase2 = true;

                healthPoints_Phase1[healthPointIndex].SetActive(false);
                healthBarAnimator.SetTrigger("Phase2_Transition");
                StartCoroutine(ReturnHealthPoints(inRageMode));
                health = 10;
                isActive = true;
                isStunned = false;
                yield break;
            }
        }

        if (phase2)
        {
            anim.SetBool(takeDamage_2, true);
        }
        else
        {
            anim.SetBool(takeDamage_1, true);
        }

        if (phase2 || inRageMode)
        {
            healthPoints_Phase2[healthPointIndex].SetActive(false);

            yield return new WaitForSeconds(takeDamage_Phase2_Clip.length * 5);
        }
        else
        {
            healthPoints_Phase1[healthPointIndex].SetActive(false);

            yield return new WaitForSeconds(takeDamage_Phase1_Clip.length * 5);
        }

        collide = false;
        mainCollider.enabled = true;
        healthPointIndex++;

        anim.SetBool(takeDamage_1, false);
        anim.SetBool(takeDamage_2, false);
        isStunned = false;
    }

    private IEnumerator Activate(bool Phase_2)
    {
        isActivating = true;

        if (collide)
        {
            collide = false;
        }

        if (inRageMode)
        {
            anim.SetTrigger(rageModeActivating);

            rageMode_scriptEvent.StartEvent();

            for (int i = 0; i < hands.Length; i++)
            {
                hands[i].punchSpeed /= 2;
            }

            waitCommandTime /= 4;
            preparationTime /= 2;
            cubeImpulseForce *= 2;
            moveSpeed /= 2;
            attackSpeed /= 2;
            gravityLimit++;

            for (int i = 0; i < hands.Length; i++)
            {
                hands[i].punchSpeed = punchSpeed / 2;
            }

            audioS.PlayOneShot(rageModeActivating_Sound, audioS.volume);

            yield return new WaitForSeconds(rageModeActivating_Clip.length);

            isActivating = false;

            yield break;
        }

        if (Phase_2)
        {
            anim.SetTrigger(activate_2);
            phase2_scriptEvent.StartEvent();
            cubeImpulseForce *= 3;
            damageBossStation.cubeImpulseForce *= 1.5f;

            StartCoroutine(HandsTransitionAnim());

            StartCoroutine(PushTime());
            if (phase2)
            {
                gravityArrows_anim.SetTrigger("Phase2");
            }
            else
            {
                gravityArrows_anim.SetTrigger("Phase1");
            }

            for (int i = 0; i < hands.Length; i++)
            {
                hands[i].punchSpeed = punchSpeed / 2;
            }

            audioS.PlayOneShot(activate_Phase2_Sound, audioS.volume);

            yield return new WaitForSeconds(activate_Phase2_Clip.length * 2);
        }
        else
        {
            anim.SetTrigger(activate_1);
            healthBarAnimator.SetTrigger("Spawn");
            StartCoroutine(PushTime());
            mainCollider.enabled = true;

            StartCoroutine(HandsTransitionAnim());

            if (phase2)
            {
                gravityArrows_anim.SetTrigger("Phase2");
            }
            else
            {
                gravityArrows_anim.SetTrigger("Phase1");
            }

            for (int i = 0; i < hands.Length; i++)
            {
                hands[i].punchSpeed = punchSpeed;
            }

            audioS.PlayOneShot(activate_Phase1_Sound, audioS.volume);

            yield return new WaitForSeconds(activate_Phase1_Clip.length);

        }

        isActivating = false;
    }

    private IEnumerator FirstAttack(bool Phase_2)
    {
        isAttacking = true;

        if (Phase_2)
        {
            anim.SetBool(firstAttack_2, true);
        }
        else
        {
            anim.SetBool(firstAttack_1, true);
        }

        if (Phase_2)
        {
            audioS.PlayOneShot(firstAttack_Phase2_Sound, audioS.volume);

            yield return new WaitForSeconds(firstAttack_Phase2_Clip.length / 2);

            for (int i = 0; i < expCubeSpawnPosition_Phase1.Length; i++)
            {
                int rand = Random.Range(0, 6);
                GameObject _explosionCube = Instantiate(explosionCube, expCubeSpawnPosition_Phase2[i].position, explosionCube.transform.rotation);
                Rigidbody2D cubeRigidbody = _explosionCube.GetComponent<Rigidbody2D>();
                _explosionCube.GetComponent<AudioSource>().volume = audioS.volume;
                cubeRigidbody.gravityScale = 0;

                if (i == 0 || i % 2 == 0)
                {
                    cubeRigidbody.AddForce(new Vector2(cubeImpulseForce + rand, 0), ForceMode2D.Impulse);
                    _explosionCube.transform.DORotate(new Vector3(0, 0, -cubeRotationValue), cubeRotationTime, RotateMode.FastBeyond360);
                }
                else
                {
                    cubeRigidbody.AddForce(new Vector2(-cubeImpulseForce - rand, 0), ForceMode2D.Impulse);
                    _explosionCube.transform.DORotate(new Vector3(0, 0, cubeRotationValue), cubeRotationTime, RotateMode.FastBeyond360);
                }

                if (i == 1 || i == 5 || i == 9)
                {
                    yield return new WaitForSeconds(0.5f);
                }
            }

            yield return new WaitForSeconds(firstAttack_Phase2_Clip.length / 2);
        }
        else
        {
            audioS.PlayOneShot(firstAttack_Phase1_Sound, audioS.volume);

            yield return new WaitForSeconds(firstAttack_Phase1_Clip.length / 2);

            for (int i = 0; i < expCubeSpawnPosition_Phase1.Length; i++)
            {
                GameObject _explosionCube = Instantiate(explosionCube, expCubeSpawnPosition_Phase1[i].position, explosionCube.transform.rotation);
                Rigidbody2D cubeRigidbody = _explosionCube.GetComponent<Rigidbody2D>();
                cubeRigidbody.gravityScale = 0;

                int rand = Random.Range(-3, 4);

                cubeRigidbody.AddForce(new Vector2(0, -cubeImpulseForce + rand), ForceMode2D.Impulse);
                _explosionCube.transform.DORotate(new Vector3(0, 0, cubeRotationValue), cubeRotationTime, RotateMode.FastBeyond360);

                yield return new WaitForSeconds(0.2f);

            }

            yield return new WaitForSeconds(firstAttack_Phase1_Clip.length / 2);
        }

        attackCount++;
        anim.SetBool(firstAttack_2, false);
        anim.SetBool(firstAttack_1, false);
        isAttacking = false;
    }

    private IEnumerator SecondAttack(bool Phase_2)
    {
        isAttacking = true;

        shakeTime /= 2;

        if (Phase_2)
        {
            anim.SetBool(secondAttack_2, true);
        }
        else
        {
            anim.SetBool(secondAttack_1, true);
        }

        if (Phase_2)
        {
            int punchCount = Random.Range(1, 6);

            while (punchCount > 0)
            {
                bool _continue = false;

                for (int i = 0; i < hands.Length; i++)
                {
                    if (hands[i].isAttacking)
                    {
                        _continue = true;
                        yield return new WaitForSeconds(0.5f);
                    }
                }

                if (_continue)
                {
                    continue;
                }

                int handIndex = Random.Range(2, 4);

                while (handIndex == lastHandIndex)
                {
                    handIndex = Random.Range(2, 4);
                }
                lastHandIndex = handIndex;

                StartCoroutine(hands[handIndex].Attack(preparationTime / 2));

                yield return new WaitForSeconds(attackSpeed / 2);

                punchCount--;
            }
        }
        else
        {
            int punchCount = Random.Range(1, 4);

            while (punchCount > 0)
            {
                bool _continue = false;

                for (int i = 0; i < hands.Length; i++)
                {
                    if (hands[i].isAttacking)
                    {
                        _continue = true;
                        yield return new WaitForSeconds(0.5f);
                    }
                }

                if (_continue)
                {
                    continue;
                }

                int handIndex = Random.Range(0, 2);

                while (handIndex == lastHandIndex)
                {
                    handIndex = Random.Range(0, 2);
                }
                lastHandIndex = handIndex;

                StartCoroutine(hands[handIndex].Attack(preparationTime));

                yield return new WaitForSeconds(attackSpeed);

                punchCount--;
            }
        }

        shakeTime *= 2;
        attackCount++;
        anim.SetBool(secondAttack_1, false);
        anim.SetBool(secondAttack_2, false);
        isAttacking = false;
    }

    private IEnumerator ThirdAttack(bool Phase_2)
    {
        isAttacking = true;

        if (Phase_2)
        {
            anim.SetBool(thirdAttack_2, true);
        }
        else
        {
            anim.SetBool(thirdAttack_1, true);
        }

        StartCoroutine(explosionField.ActivateExplosionField(Phase_2));

        yield return new WaitForSeconds(4f);

        attackCount++;
        anim.SetBool(thirdAttack_1, false);
        anim.SetBool(thirdAttack_2, false);
        isAttacking = false;
    }

    private IEnumerator FourthAttack(bool Phase_2)
    {
        isAttacking = true;

        if (Phase_2)
        {
            anim.SetBool(fourthAttack_2, true);
        }
        else
        {
            anim.SetBool(fourthAttack_1, true);
        }

        if (Phase_2)
        {
            audioS.PlayOneShot(fourthAttack_Preparation_Phase2_Sound, audioS.volume);

            yield return new WaitForSeconds(fourthAttack_Phase2_Clip.length * 0.8f);

            secondAudioSource.PlayOneShot(fourthAttack_Phase2_Sound, secondAudioSource.volume);
            GameObject projectile = Instantiate(projectile_Phase2, projectileSpawner.position, Quaternion.identity);
            projectile.GetComponent<AudioSource>().volume = audioS.volume;

            yield return new WaitForSeconds(1f);
        }
        else
        {
            audioS.PlayOneShot(fourthAttack_Preparation_Phase1_Sound, audioS.volume);

            yield return new WaitForSeconds(fourthAttack_Phase1_Clip.length * 0.85f);

            secondAudioSource.PlayOneShot(fourthAttack_Phase1_Sound, secondAudioSource.volume);
            GameObject projectile = Instantiate(projectile_Phase1, projectileSpawner.position, Quaternion.identity);
            projectile.GetComponent<AudioSource>().volume = audioS.volume;

            yield return new WaitForSeconds(1f);
        }

        attackCount++;
        anim.SetBool(fourthAttack_1, false);
        anim.SetBool(fourthAttack_2, false);
        isAttacking = false;
    }

    private IEnumerator Dead()
    {
        isDead = true;

        anim.SetTrigger(dead);

        mainCollider.enabled = false;

        for (int i = 0; i < hands.Length; i++)
        {
            hands[i].Disactivate();
        }

        StartCoroutine(StartShake());
        moveToCentre();

        audioS.PlayOneShot(preDead_Sound, audioS.volume);

        yield return new WaitForSeconds(preDead_Clip.length);

        secondAudioSource.PlayOneShot(dead_Sound, secondAudioSource.volume);

        yield return new WaitForSeconds(dead_Clip.length);

        audioS.clip = postDead_Sound;
        audioS.loop = true;
        audioS.volume *= 0.7f;
        audioS.Play();

        victory_scriptEvent.StartEvent();
    }

    private void moveToCentre()
    {
        gameObject.transform.DOMove(centrePoint.position, dead_Clip.length);
        gameObject.transform.DORotate(new Vector3(0, 0, -30), dead_Clip.length).SetEase(Ease.OutBounce);
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

    public void StopAttacking(bool stop)
    {
        if (stop)
        {
            stopAttacking = true;
        }
        else
        {
            stopAttacking = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out CharacterController2D player))
        {
            player.Dead();
        }

        if (collision.TryGetComponent(out ExplosionCube explosionCube) && explosionCube.detonationWithBoss && !isActivating && !returningHealth && !collide)
        {
            collide = true;
            StartCoroutine(explosionCube.Explosion());

            if (!isImprevious)
            {
                StopAllCoroutines();
                StartCoroutine(TakeDamage());
            }
        }
    }

    private IEnumerator HandsTransitionAnim()
    {
        if (phase2)
        {
            hands_anim[0].SetTrigger("Disactivate");
            hands_anim[1].SetTrigger("Disactivate");

            yield return new WaitForSeconds(1);

            hands[0].Disactivate();
            hands[1].Disactivate();

            hands[2].transform.position = hands[2].mainHandPosition.transform.position;
            hands[3].transform.position = hands[3].mainHandPosition.transform.position;

            hands[2].Activate();
            hands[3].Activate();

            hands_anim[2].SetTrigger("Activate");
            hands_anim[3].SetTrigger("Activate");
        }
        else
        {
            hands[0].Activate();
            hands[1].Activate();
        }
    }

    private void PlayerPush(bool phase2)
    {
        int rand = Random.Range(0, 6);

        if (phase2)
        {
            playerRb.AddForce(new Vector2(0, -pushPlayerPower * 1.5f - rand), ForceMode2D.Force);
        }
        else
        {
            playerRb.AddForce(new Vector2(0, pushPlayerPower + rand), ForceMode2D.Force);
        }
    }

    private IEnumerator PushTime()
    {
        pushPlayer = true;

        yield return new WaitForSeconds(pushTime);

        pushPlayer = false;
    }
}
