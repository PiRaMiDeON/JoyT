using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCntrl_phase2 : MonoBehaviour
{
    private Animator anim;

    private Rigidbody2D rb;

    public SpriteRenderer spriteRen;
    public AudioSource audioS;
    public AudioClip stun_Sound;
    public AudioClip preStun_Sound;
    public AudioClip dead_Sound;
    public AudioClip bossWakeUp_Sound;
    public AudioClip teleport_Sound;
    public AudioClip blast_Sound;
    public AudioClip superBlast_Sound;
    public AudioClip preparationSuperBlast_Sound;
    public AudioClip shot_Sound;
    public AudioClip superShot_Sound;
    public AudioClip preparationSuperShot_Sound;
    public AudioClip[] Cracking_Sounds;
    private int crackIndex = 0;

    private PolygonCollider2D bodyCollider;

    public bool bossActivate;
    public float equalization;
    private bool isDead;
    private bool isStaing;
    private bool isBlasting;
    private bool isShooting;
    private bool isTeleportating;
    private bool isStunning;
    private int teleportLimitIndex;
    private int blastLimitIndex;
    private bool stop;

    public int healthPoints;
    public int blastValue;
    public float shootingForce;
    public float teleportWaitTime;
    public float blastLowerSpeedLimit;
    public float blastHighestSpeedLimit;

    public GameObject healthBar;
    public GameObject healthBarText;
    public GameObject[] healthBarPoints;
    private int pointIndex = 0;

    public GameObject[] blastProjectile;
    public GameObject superBlastProjectile;
    public GameObject shotProjectile;
    public GameObject superShotProjectile;
    public Transform[] blastProjectileSpawners;
    public Rigidbody2D shotProjectileSpawner;
    public Transform[] teleportPoints;
    public Transform teleportPoint;
    public Transform saveTeleportPoint;

    private int saveBlastProjectileSpawner;

    public DialogEdgesActivator DEA;
    public ScriptEvent scriptEvent;

    private Collider2D checkCircle;
    public Transform circleColliderPoint;
    public float colliderRadius;
    public LayerMask playerLayer;

    public AnimationClip stay_Animation;
    public AnimationClip blast_Animation;
    public AnimationClip dead_Animation;
    public AnimationClip stun_Animation;
    public AnimationClip shooting_Animation;
    public AnimationClip Spawn_Animation;

    public ParticleSystem blast_PartSys;
    public ParticleSystem superBlast_PartSys;
    public ParticleSystem shot_PartSys;
    public ParticleSystem superShot_PartSys;
    public ParticleSystem stun_PartSys;
    public ParticleSystem[] dead_PartSys;
    public ParticleSystem[] postDead_PartSys;

    private GameObject[] shotProjectiles;
    private GameObject[] blastProjectiles;

    Vector2 playerPosition;
    public GameObject player;

    private bool playerNear, touched;

    private void Start()
    {
        bodyCollider = GetComponent<PolygonCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }


    private void Update()
    {
        checkCircle = Physics2D.OverlapCircle(new Vector2(circleColliderPoint.position.x, circleColliderPoint.position.y), colliderRadius, playerLayer);

        playerPosition = player.transform.position;
        Vector2 lookDir = playerPosition - shotProjectileSpawner.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        shotProjectileSpawner.rotation = angle - 90f;

        if(checkCircle)
        {
            if(checkCircle.TryGetComponent(out CharacterController2D player))
            {
                playerNear = true;
            }          
        }
        else
        {
            playerNear = false;
        }

        if(bossActivate)
        {
            if(stop || isDead || isBlasting || isStaing || isShooting || isTeleportating || isStunning)
            {
                return;
            }

            StartCoroutine(Stay());
        }

    }

    public IEnumerator Activate()
    {
        stop = true;
        bossActivate = true;
        audioS.PlayOneShot(bossWakeUp_Sound, audioS.volume);

        yield return new WaitForSeconds(Spawn_Animation.length);

        healthBar.SetActive(true);
        for (int i = 0; i < healthBarPoints.Length; i++)
        {
            healthBarPoints[i].SetActive(true);
        }
        healthBarText.SetActive(true);

        spriteRen.color = new Color(spriteRen.color.r, spriteRen.color.b, spriteRen.color.g, 1);
        stop = false;
    }
    
    private IEnumerator Teleport()
    {
        isTeleportating = true;
        isStaing = false;
        anim.SetBool("Teleport", true);
        audioS.PlayOneShot(teleport_Sound, audioS.volume);
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

    private IEnumerator Stay()
    {
        isStaing = true;

        yield return new WaitForSeconds(stay_Animation.length * 3);

        if(playerNear)
        {
            int randomIndex = Random.Range(0, 4);
            
            if(teleportLimitIndex >= 3)
            {
                StartCoroutine(StartBlast());
                yield break;
            }   
            
            if(blastLimitIndex >= 5)
            {
                StartCoroutine(StartSuperShooting());
                yield break;
            
            }
            if(randomIndex <= 1)
            {
                StartCoroutine(StartBlast());
            }
            else
            {
                StartCoroutine(StartShooting());
            }
        }
        else
        {
            StartCoroutine(Teleport());
            teleportLimitIndex++;
        }
    }

    private IEnumerator StartBlast()
    {
        isBlasting = true;
        isStaing = false;
        anim.SetBool("Blast", true);

        if(teleportLimitIndex >= 3)
        {
            audioS.PlayOneShot(preparationSuperBlast_Sound, audioS.volume * 1.5f);
        }

        yield return new WaitForSeconds(blast_Animation.length);

        if(teleportLimitIndex >= 3)
        {
            superBlast_PartSys.Play();
        }
        else
        {
            blast_PartSys.Play();
        }

        if(teleportLimitIndex >= 3)
        {
            audioS.PlayOneShot(superBlast_Sound, audioS.volume);
        }
        else
        {
            audioS.PlayOneShot(blast_Sound, audioS.volume);
        }

        Blast();

        yield return new WaitForSeconds(blast_Animation.length);

        if (teleportLimitIndex >= 3)
        {
            teleportLimitIndex = 0;
        }

        anim.SetBool("Blast", false);
        blastLimitIndex++;
        isBlasting = false;
    }

    private IEnumerator StartShooting()
    {
        isShooting = true;
        isStaing = false;
        anim.SetBool("Shoot", true);

        yield return new WaitForSeconds(shooting_Animation.length * 3);

        shot_PartSys.Play();
        Shoot();

        yield return new WaitForSeconds(0.5f);

        shot_PartSys.Play();
        Shoot();

        yield return new WaitForSeconds(0.5f);

        shot_PartSys.Play();
        Shoot();

        yield return new WaitForSeconds(0.5f);

        anim.SetBool("Shoot", false);
        isShooting = false;
    }

    private IEnumerator StartSuperShooting()
    {
        isShooting = true;
        isStaing = false;
        anim.SetBool("Shoot", true);
        audioS.PlayOneShot(preparationSuperShot_Sound, audioS.volume * 1.5f);

        yield return new WaitForSeconds(4f);

        superShot_PartSys.Play();
        Shoot();

        yield return new WaitForSeconds(0.5f);

        superShot_PartSys.Play();
        Shoot();

        yield return new WaitForSeconds(0.5f);

        superShot_PartSys.Play();
        Shoot();

        yield return new WaitForSeconds(0.5f);

        superShot_PartSys.Play();
        Shoot();

        yield return new WaitForSeconds(0.5f);

        superShot_PartSys.Play();
        Shoot();

        yield return new WaitForSeconds(1f);

        if (blastLimitIndex >= 5)
        {
            blastLimitIndex = 0;
        }

        anim.SetBool("Shoot", false);
        isShooting = false;
    }

    private void Shoot()
    {
        if(blastLimitIndex >= 5)
        {
            audioS.PlayOneShot(superShot_Sound, audioS.volume);
            GameObject projectile = Instantiate(superShotProjectile, new Vector2(shotProjectileSpawner.position.x, shotProjectileSpawner.position.y), shotProjectileSpawner.transform.rotation);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.AddForce(shotProjectileSpawner.transform.up * shootingForce * 1.5f, ForceMode2D.Impulse);
        }
        else
        {
            audioS.PlayOneShot(shot_Sound, audioS.volume);
            GameObject projectile = Instantiate(shotProjectile, new Vector2(shotProjectileSpawner.position.x, shotProjectileSpawner.position.y), shotProjectileSpawner.transform.rotation);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.AddForce(shotProjectileSpawner.transform.up * shootingForce, ForceMode2D.Impulse);
        }
    }

    private void Blast()
    {

        if(teleportLimitIndex >= 3)
        {
            for (int i = 0; i < blastProjectileSpawners.Length; i++)
            {
                GameObject blast = Instantiate(superBlastProjectile, new Vector2(blastProjectileSpawners[i].position.x, blastProjectileSpawners[i].position.y), superBlastProjectile.transform.rotation);
                blast.GetComponent<VirusBall>().speed = Random.Range(blastLowerSpeedLimit + 10, blastHighestSpeedLimit + 10);
            }

        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                int randomIndex = Random.Range(0, blastProjectile.Length);
                int randomIndex2 = Random.Range(0, blastProjectileSpawners.Length);

                while(randomIndex2 == saveBlastProjectileSpawner)
                {
                    randomIndex2 = Random.Range(0, blastProjectileSpawners.Length);
                }

                saveBlastProjectileSpawner = randomIndex2;
                GameObject blast = Instantiate(blastProjectile[randomIndex], new Vector2(blastProjectileSpawners[randomIndex2].position.x, blastProjectileSpawners[randomIndex2].position.y), blastProjectile[randomIndex].transform.rotation);
                blast.GetComponent<VirusBall>().speed = Random.Range(blastLowerSpeedLimit, blastHighestSpeedLimit + 1);
            }
        }
    }

    private bool PlayerIsNear(bool result)
    {
        if(checkCircle)
        {
            if(checkCircle.TryGetComponent(out CharacterController2D player))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out CharacterController2D controller))
        {
            controller.Dead();
        }
    }

    private void Hit()
    {
        StopAllCoroutines();
        touched = false;

        StartCoroutine(Stun());
    }

    private IEnumerator Stun()
    {
        isStunning = true;
        teleportLimitIndex = 0;

        healthPoints--;
        healthBarPoints[pointIndex].SetActive(false);
        pointIndex++;

        isStaing = false;
        isShooting = false;
        isTeleportating = false;
        isBlasting = false;

        if(healthPoints <= 0)
        {
            StartCoroutine(Dead());
            yield break;
        }

        anim.SetTrigger("Stun");

        anim.SetBool("Blast", false);
        anim.SetBool("Shoot", false);
        anim.SetBool("Teleport", false);
        audioS.PlayOneShot(preStun_Sound, audioS.volume);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        bodyCollider.enabled = false;

        yield return new WaitForSeconds(stun_Animation.length / 2);

        audioS.PlayOneShot(stun_Sound, audioS.volume);
        stun_PartSys.Play();

        yield return new WaitForSeconds(stun_Animation.length / 2);

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        bodyCollider.enabled = true;
        isStunning = false;
    }

    private IEnumerator Dead()
    {
        isDead = true;

        healthBar.SetActive(false);
        healthBarText.SetActive(false);

        Clear();

        isStaing = false;
        isShooting = false;
        isTeleportating = false;
        isBlasting = false;
        scriptEvent.StartEvent();
        bodyCollider.enabled = false;
        scriptEvent.StartEvent();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        anim.SetTrigger("Dead");
        audioS.PlayOneShot(Cracking_Sounds[crackIndex], audioS.volume);
        crackIndex++;

        yield return new WaitForSeconds(1f);

        audioS.PlayOneShot(Cracking_Sounds[crackIndex], audioS.volume);
        crackIndex++;

        yield return new WaitForSeconds(1f);

        audioS.PlayOneShot(Cracking_Sounds[crackIndex], audioS.volume);
        crackIndex++;

        yield return new WaitForSeconds(1f);

        audioS.PlayOneShot(Cracking_Sounds[crackIndex], audioS.volume);
        crackIndex++;

        yield return new WaitForSeconds(0.5f);

        audioS.PlayOneShot(Cracking_Sounds[crackIndex], audioS.volume);
        crackIndex++;

        yield return new WaitForSeconds(0.5f);

        audioS.PlayOneShot(Cracking_Sounds[crackIndex], audioS.volume);
        crackIndex++;

        yield return new WaitForSeconds(1.17f);

        for (int i = 0; i < dead_PartSys.Length; i++)
        {
            dead_PartSys[i].Play();
        }

        audioS.PlayOneShot(dead_Sound, audioS.volume);

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < postDead_PartSys.Length; i++)
        {
            postDead_PartSys[i].Play();
        }

    }
    private void Clear()
    {
        blastProjectiles = GameObject.FindGameObjectsWithTag("Blast");
        shotProjectiles = GameObject.FindGameObjectsWithTag("Shot");

        if(blastProjectiles != null)
        {
            for (int i = 0; i < blastProjectiles.Length; i++)
            {
                blastProjectiles[i].GetComponent<AudioSource>().volume = 0;
                blastProjectiles[i].GetComponent<VirusBall>().Hit();
            }
        }

        if (shotProjectiles != null)
        {
            for (int i = 0; i < shotProjectiles.Length; i++)
            {
                shotProjectiles[i].GetComponent<AudioSource>().volume = 0;
                shotProjectiles[i].GetComponent<ShotProjectile>().ProjectileDestroy();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out ExplosionCube explosionCube) && !touched)
        {
            touched = true;
            Debug.Log("fweofow");
            StartCoroutine(explosionCube.Explosion());
            Hit();
        }
    }
}
