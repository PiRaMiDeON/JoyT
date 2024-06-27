using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCntrl_phase1 : MonoBehaviour
{
    private Animator anim;
    public AnimationClip deadAnim;

    private Rigidbody2D rb;

    private AudioSource audioS;
    public AudioClip stun;
    public AudioClip jump;
    public AudioClip preJumpSound;
    public AudioClip dead;
    public AudioClip bossWakeUp;
    public AudioClip teleport;

    private PolygonCollider2D bodyCollider;

    public Transform saveTeleportPoint;
    public Transform teleportPoint;
    public Transform[] teleportPoints;

    private bool bossActive;
    public bool isHitting;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isDead;
    [SerializeField] private bool isTeleportating;
    [SerializeField] private bool isPreJumping;
    [SerializeField] private bool isStaing;

    private int jumpVector;
    public string bossPosition;

    public int healthPoints;
    public float stunTime;
    public float preJumpTime;
    public float waitTime;
    public float saveWaitTime;
    public float teleportWaitTime;
    public float jumpTime;

    public float highDistance;
    public float horizontalPower;
    public float verticalPower;

    public GameObject healthBar;
    public GameObject healthBarText;
    public GameObject[] healthBarPoints;
    private int pointIndex;

    public GameObject[] VirusBallShooters;
    public GameObject[] VirusBalls;

    public DialogEdgesActivator DEA;
    public ScriptEvent scriptEvent;

    public GameObject hitZone;

    private void Start()
    {
        pointIndex = 0;
        saveWaitTime = waitTime;
        bossPosition = "Mid";
        bossActive = false;
        isHitting = false;
        isDead = false;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioS = GetComponent<AudioSource>();
        bodyCollider = GetComponent<PolygonCollider2D>();
    }

    public void Activate()
    {
        bossActive = true;
        audioS.PlayOneShot(bossWakeUp, audioS.volume);
    }

    public IEnumerator Stun()
    {
        isStaing = false;
        audioS.PlayOneShot(stun, audioS.volume);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        hitZone.SetActive(false);
        bodyCollider.enabled = false;
        healthPoints--;
        healthBarPoints[pointIndex].SetActive(false);
        pointIndex++;

        if (healthPoints <= 0)
        {
            StartCoroutine(Dead());
            yield break;
        }


        anim.SetBool("Stun", isHitting);
        if (bossPosition == "Mid")
        {
            waitTime -= 2;
        }

        yield return new WaitForSeconds(stunTime);

        anim.SetBool("Stun", false);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        yield return new WaitForSeconds(2f);

        hitZone.SetActive(true);
        bodyCollider.enabled = true;
        StartCoroutine(Teleport());
        isHitting = false;
    }

    private IEnumerator preJump()
    {
        isPreJumping = true;
        isStaing = false;
        audioS.PlayOneShot(preJumpSound, audioS.volume);

        yield return new WaitForSeconds(preJumpTime);

        switch (bossPosition)
        {
            case "Left_Down":
                jumpVector = Random.Range(2, 4);
                break;

            case "Right_Down":
                jumpVector = Random.Range(1, 5);

                if (jumpVector == 2)
                {
                    jumpVector = 1;
                }

                if (jumpVector == 4)
                {
                    jumpVector = 3;
                }
                break;

            case "Left_Up":
                jumpVector = Random.Range(1, 5);

                if (jumpVector == 1)
                {
                    jumpVector = 2;
                }

                if (jumpVector == 3)
                {
                    jumpVector = 4;
                }
                break;

            case "Right_Up":
                jumpVector = Random.Range(1, 5);

                if (jumpVector == 2)
                {
                    jumpVector = 1;
                }

                if (jumpVector == 3)
                {
                    jumpVector = 4;
                }
                break;
        }

        switch (jumpVector)
        {
            case 1:
                StartCoroutine(JumpLeft());
                break;

            case 2:
                StartCoroutine(JumpRight());
                break;

            case 3:
                StartCoroutine(JumpUp());
                break;

            case 4:
                StartCoroutine(Teleport());
                break;
        }
    }

    private IEnumerator JumpLeft()
    {
        isJumping = true;
        isPreJumping = false;
        anim.SetBool("Jump", isJumping);

        yield return new WaitForSeconds(jumpTime);

        audioS.PlayOneShot(jump, audioS.volume);
        rb.AddForce(transform.right * -horizontalPower, ForceMode2D.Impulse);
        rb.AddForce(transform.up * verticalPower, ForceMode2D.Impulse);
        anim.SetBool("Jump", false);
        isJumping = false;
    }

    private IEnumerator JumpRight()
    {
        isJumping = true;
        isPreJumping = false;
        anim.SetBool("Jump", isJumping);

        yield return new WaitForSeconds(jumpTime);

        audioS.PlayOneShot(jump, audioS.volume);
        rb.AddForce(transform.right * horizontalPower, ForceMode2D.Impulse);
        rb.AddForce(transform.up * verticalPower, ForceMode2D.Impulse);
        anim.SetBool("Jump", false);
        isJumping = false;
    }

    private IEnumerator JumpUp()
    {
        isJumping = true;
        isPreJumping = false;
        anim.SetBool("Jump", isJumping);

        yield return new WaitForSeconds(jumpTime);

        bodyCollider.enabled = false;
        audioS.PlayOneShot(jump, audioS.volume);
        rb.AddForce(transform.up * verticalPower * highDistance, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1f);

        bodyCollider.enabled = true;
        anim.SetBool("Jump", false);
        isJumping = false;
    }

    private IEnumerator Stay()
    {
        isStaing = true;

        if (bossPosition == "Mid")
        {
            waitTime++;
            waitTime++;
        }

        yield return new WaitForSeconds(waitTime);

        if (bossPosition == "Mid")
        {
            waitTime--;
            waitTime--;
        }

        int action = Random.Range(0, 3);

        switch (action)
        {
            case 0:
                if (bossPosition == "Mid")
                {
                    StartCoroutine(Teleport());
                    break;
                }
                StartCoroutine(preJump());
                break;

            case 1:
                if (bossPosition == "Mid")
                {
                    StartCoroutine(Teleport());
                    break;
                }
                StartCoroutine(preJump());
                break;

            case 2:
                StartCoroutine(Teleport());
                break;
        }

    }

    private IEnumerator Dead()
    {
        isDead = true;

        healthBarText.SetActive(false);
        healthBar.SetActive(false);

        Clear();
        audioS.PlayOneShot(dead, audioS.volume);
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        hitZone.SetActive(false);
        bodyCollider.enabled = false;
        anim.SetTrigger("Dead_Phase_1");

        yield return new WaitForSeconds(deadAnim.length + 1);

        scriptEvent.StartEvent();

        yield return new WaitForSeconds(1f);

        DEA.EdgesActivate();
        DEA.DialogActivate();

        Destroy(gameObject);
    }

    private IEnumerator Teleport()
    {
        isTeleportating = true;
        isPreJumping = false;
        isStaing = false;
        anim.SetBool("Teleport", isTeleportating);
        audioS.PlayOneShot(teleport, audioS.volume);
        bodyCollider.enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        while (teleportPoint == saveTeleportPoint)
        {
            teleportPoint = teleportPoints[Random.Range(0, teleportPoints.Length)];
        }

        saveTeleportPoint = teleportPoint;

        yield return new WaitForSeconds(teleportWaitTime);

        gameObject.transform.position = new Vector2(teleportPoint.position.x, teleportPoint.position.y);
        bodyCollider.enabled = true;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        anim.SetBool("Teleport", false);
        isTeleportating = false;
    }

    public void Hit()
    {
        isHitting = true;
        isJumping = false;
        isPreJumping = false;

        StopAllCoroutines();

        anim.SetBool("Teleport", false);
        anim.SetBool("Jump", false);

        StartCoroutine(Stun());

    }

    private void Update()
    {
        if (bossActive)
        {
            if (bossPosition != "Mid" && waitTime != saveWaitTime)
            {
                waitTime = saveWaitTime;
            }

            if (isStaing)
            {
                return;
            }
            if (isPreJumping)
            {
                return;
            }
            if (isTeleportating)
            {
                return;
            }
            if (isHitting)
            {
                return;
            }
            if (isJumping)
            {
                return;
            }
            if (isDead)
            {
                return;
            }

            StartCoroutine(Stay());

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out CharacterController2D controller))
        {
            controller.Dead();
        }
    }

    private void Clear()
    {
        VirusBallShooters = GameObject.FindGameObjectsWithTag("VirusBallShooter");
        VirusBalls = GameObject.FindGameObjectsWithTag("VirusBall");

        for (int i = 0; i < VirusBalls.Length; i++)
        {
            VirusBalls[i].GetComponent<AudioSource>().volume = 0;
            VirusBalls[i].GetComponent<VirusBall>().Hit();
        }

        for (int i = 0; i < VirusBallShooters.Length; i++)
        {
            Destroy(VirusBallShooters[i]);
        }
    }
}

