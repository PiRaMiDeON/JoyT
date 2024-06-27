using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController2D : MonoBehaviour
{
    public ToMenuButton toMenuButton;
    private bool inQuasiField;
    private bool inGWL_GWR;
    [HideInInspector] public bool inBossZone;
    public Animator interFaceAnim;
    public GameObject[] healthPoint;
    [HideInInspector] public Animator animator;
    private ParticleSystem partSys;
    [HideInInspector] public SpriteRenderer spriteRen;
    public float gravityForce;
    public AudioClip boom;
    public AudioClip death;
    public AudioClip teleport;

    [HideInInspector] public AudioSource audioS;
    public GameController gameController;
    [HideInInspector] public Rigidbody2D rb;

    public float saveSpeed;
    public float saveJumpHeight;
    public float saveGravityScale;
    public float speed;
    public float jumpHeight;

    public Vector2 groundCheckOffset;
    public float groundCheckRadius = 0.30f;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    [SerializeField] private bool isDead;
    public ParticleSystem healthPartSys;

    public float X;
    public float Y;
    public float Z;

    Vector2 last;
    public float rayLenght;

    RaycastHit2D hit1;
    RaycastHit2D hit2;
    RaycastHit2D hit3;

    public float XoffsetRaycastValue;
    public float YoffsetRaycastValue;
    public Vector2 enemyHitboxCheckOffset;

    Collider2D enemyHit1;
    Collider2D enemyHit2;
    public float enemyHitboxSizeX, enemyHitboxSizeY, angle;
    public float colliderDelay;

    public float bounceForce;

    public PlayerClearWave playerClearWave;
    public bool disableClearWave;

    private void Start()
    {
        spriteRen = GetComponent<SpriteRenderer>();
        healthPartSys = GameObject.Find("HealthReturnPartSys").GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody2D>();
        audioS = GetComponent<AudioSource>();
        spriteRen.enabled = true;
        partSys = GetComponentInChildren<ParticleSystem>();
        animator = GetComponent<Animator>();

        saveSpeed = speed;
        saveJumpHeight = jumpHeight;
        saveGravityScale = rb.gravityScale;
        speed = 0f;
        jumpHeight = 0f;
    }

    private void Update()
    {
        if (isDead)
        {
            if (Input.GetKey(KeyCode.Return))
            {

                if (gameController.DeathCount >= 4)
                {
                    gameController.ReloadScene();
                    return;
                }
                else
                {
                    speed = saveSpeed;
                    jumpHeight = saveJumpHeight;
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    GetComponent<BoxCollider2D>().enabled = true;
                    GetComponent<PolygonCollider2D>().enabled = true;
                    transform.position = gameController.spawnPoint.position;

                    if (!disableClearWave)
                    {
                        playerClearWave.enabled = true;
                    }

                    spriteRen.enabled = true;
                    gameController.fader.reload.SetActive(false);
                    gameController.fader.gameObject.SetActive(false);
                    isDead = false;
                }
            }

            return;
        }

        if (Input.GetKey(KeyCode.W) && IsGrounded() || Input.GetKey(KeyCode.Space) && IsGrounded() || Input.GetKey(KeyCode.UpArrow) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
        }

        if (!inGWL_GWR && !inQuasiField)
        {
            float input = Input.GetAxis("Horizontal");

            rb.velocity = new Vector2(speed * input, rb.velocity.y);

            if (inBossZone)
            {
                float input2 = Input.GetAxis("Vertical");

                rb.velocity = new Vector2(rb.velocity.x, speed * input2);
            }
        }

        hit1 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - YoffsetRaycastValue), Vector2.down, rayLenght, groundLayer);
        hit2 = Physics2D.Raycast(new Vector2(transform.position.x + XoffsetRaycastValue, transform.position.y - YoffsetRaycastValue), Vector2.down, rayLenght, groundLayer);
        hit3 = Physics2D.Raycast(new Vector2(transform.position.x - XoffsetRaycastValue, transform.position.y - YoffsetRaycastValue), Vector2.down, rayLenght, groundLayer);

        if (hit1)
        {
            if (hit1.transform.TryGetComponent(out MovePlatform moveObj))
            {
                transform.parent = moveObj.transform;
            }
        }
        if (hit2)
        {
            if (hit2.transform.TryGetComponent(out MovePlatform moveObj))
            {
                transform.parent = moveObj.transform;
            }
        }
        if (hit3)
        {
            if (hit3.transform.TryGetComponent(out MovePlatform moveObj))
            {
                transform.parent = moveObj.transform;
            }
        }
        else if (!hit1 && !hit2 && !hit3)
        {
            transform.parent = null;
        }

        if (!disableClearWave)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (!playerClearWave.waveIsCooldowning)
                {
                    StartCoroutine(playerClearWave.ActivateWave());
                }
                else
                {
                    playerClearWave.ClearWaveError();
                }
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + (Vector3)groundCheckOffset, new Vector3(X, Y, Z));
        Gizmos.DrawWireCube(transform.position + (Vector3)enemyHitboxCheckOffset, new Vector3(enemyHitboxSizeX, enemyHitboxSizeY, Z));
        Gizmos.DrawWireCube(transform.position + (Vector3)enemyHitboxCheckOffset, new Vector3(enemyHitboxSizeX, enemyHitboxSizeY - colliderDelay, Z));

        Gizmos.DrawRay(new Vector2(transform.position.x, transform.position.y - YoffsetRaycastValue), Vector2.down * rayLenght);
        Gizmos.DrawRay(new Vector2(transform.position.x + XoffsetRaycastValue, transform.position.y - YoffsetRaycastValue), Vector2.down * rayLenght);
        Gizmos.DrawRay(new Vector2(transform.position.x - XoffsetRaycastValue, transform.position.y - YoffsetRaycastValue), Vector2.down * rayLenght);
    }
    public bool IsGrounded()
    {
        if (Physics2D.OverlapCircle(transform.position + (Vector3)groundCheckOffset, groundCheckRadius, groundLayer) || Physics2D.OverlapCircle(transform.position + (Vector3)groundCheckOffset, groundCheckRadius, enemyLayer))
        {
            return true;
        }
        return false;
    }

    public void Dead()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        if (toMenuButton.open)
        {
            toMenuButton.SetPocketMenu();
        }

        gameController.AddDeath();

        if (gameController.DeathCount >= 4)
        {
            if (!disableClearWave)
            {
                playerClearWave.enabled = false;
            }
            spriteRen.sortingOrder = 11;
            interFaceAnim.SetTrigger("Death");
            animator.SetBool("Dead", true);
            audioS.PlayOneShot(death, audioS.volume);
            gameController.LoseGame();
        }
        else
        {
            speed = 0;
            jumpHeight = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<PolygonCollider2D>().enabled = false;
            audioS.PlayOneShot(boom, audioS.volume);
            gameController.fader.gameObject.SetActive(true);
            gameController.fader.reload.SetActive(true);
            interFaceAnim.SetTrigger("Boom");
            switch (gameController.DeathCount)
            {
                case 1:
                    healthPoint[0].SetActive(false);
                    break;

                case 2:
                    healthPoint[1].SetActive(false);
                    break;

                case 3:
                    healthPoint[2].SetActive(false);
                    break;
            }

            spriteRen.enabled = false;

            if (!disableClearWave)
            {
                playerClearWave.enabled = false;
            }
            partSys.Play();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDead && collision.transform.tag == "Spike")
        {
            Dead();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead && collision.transform.tag == "Abyss")
        {
            Dead();
        }

        if (!isDead && collision.TryGetComponent(out VirusBall projectile))
        {
            projectile.Hit();
            Dead();
        }

        switch (collision.transform.tag)
        {
            case ("GravityWaveRight"):

                inGWL_GWR = true;

                break;

            case ("GravityWaveLeft"):

                inGWL_GWR = true;

                break;

            case ("SpawnCheckPoint"):

                gameController.spawnPoint = collision.transform;
                collision.GetComponent<Animator>().SetTrigger("CheckPoint");
                collision.GetComponent<AudioSource>().Play();
                collision.GetComponent<CheckPoint>().partSys.Play();
                collision.GetComponent<BoxCollider2D>().enabled = false;
                break;

            case ("QuasiField"):

                inQuasiField = true;

                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.transform.tag)
        {
            case ("GravityWaveUp"):

                rb.gravityScale = saveGravityScale;

                break;

            case ("GravityWaveDown"):

                rb.gravityScale = saveGravityScale;

                break;

            case ("GravityWaveRight"):

                inGWL_GWR = false;

                rb.gravityScale = saveGravityScale;

                break;

            case ("GravityWaveLeft"):

                inGWL_GWR = false;

                rb.gravityScale = saveGravityScale;

                break;

            case ("QuasiField"):

                inQuasiField = false;

                rb.gravityScale = saveGravityScale;

                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.transform.tag)
        {
            case ("GravityWaveUp"):
                rb.gravityScale = 0;
                GWU();
                break;

            case ("GravityWaveDown"):
                rb.gravityScale = 0;
                GWD();
                break;

            case ("GravityWaveRight"):

                rb.gravityScale = 0;

                float input = Input.GetAxis("Vertical");

                rb.velocity = new Vector2(gravityForce, speed * input);

                break;

            case ("GravityWaveLeft"):

                rb.gravityScale = 0;

                float input2 = Input.GetAxis("Vertical");

                rb.velocity = new Vector2(-gravityForce, speed * input2);

                break;

            case ("QuasiField"):

                rb.gravityScale = 0;

                float input3 = Input.GetAxis("Horizontal");

                float input4 = Input.GetAxis("Vertical");

                rb.velocity = new Vector2(speed * input3, speed * input4);

                break;
        }
    }

    public bool PlayerIsDead(bool dead)
    {
        return isDead;
    }

    private void GWU()
    {
        rb.velocity = new Vector2(rb.velocity.x, gravityForce);
    }

    private void GWD()
    {
        rb.velocity = new Vector2(rb.velocity.x, -gravityForce);
    }

    private void GWR()
    {
        rb.velocity = new Vector2(gravityForce, rb.velocity.y);
    }

    private void GWL()
    {
        rb.velocity = new Vector2(-gravityForce, rb.velocity.y);
    }
}
