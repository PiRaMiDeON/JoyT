using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<Transform> enemyPoints;

    public Animator preSpawnPartSysSAnim;
    public ParticleSystem preSpawnParticles;
    public ParticleSystem spawnParticles;

    public GameObject enemy;
    public GameObject spawnedEnemy;
    public GameObject finalEnemy;

    private AudioSource audioS;
    public AudioClip preSpawnSound;
    public AudioClip spawnSound;
    public AudioClip enemyHit;
    public AudioClip enemy2StunSound;
    public AudioClip enemy2PreDeadSound;
    public AudioClip enemy2DeadSound;

    public float spawnDelay;
    public float spawnTimer;
    public int spawnLimit;
    public int spawnIndex;
    public int enemy2HealthPoints;

    private bool enemyIsSpawned;
    private bool canSpawn;

    public float enemySpeed;

    private void Start()
    {
        spawnIndex = 0;
        audioS = GetComponent<AudioSource>();
    }

    public IEnumerator SpawnEnemy()
    {
        preSpawnParticles.Play();
        audioS.PlayOneShot(preSpawnSound, audioS.volume);

        yield return new WaitForSeconds(spawnDelay);

        preSpawnPartSysSAnim.SetBool("Activate", true);

        yield return new WaitForSeconds(spawnDelay);

        preSpawnParticles.Stop();
        preSpawnPartSysSAnim.SetBool("Activate", false);
        spawnParticles.Play();
        audioS.PlayOneShot(spawnSound, audioS.volume);

        if(spawnIndex == spawnLimit -1)
        {
            GameObject spawnedEnemyNow = Instantiate(finalEnemy, new Vector2(transform.position.x, transform.position.y), enemy.transform.rotation);
            spawnedEnemy = spawnedEnemyNow;
        }
        else
        {
            GameObject spawnedEnemyNow = Instantiate(enemy, new Vector2(transform.position.x, transform.position.y), enemy.transform.rotation);
            spawnedEnemy = spawnedEnemyNow;
        }

        enemyIsSpawned = true;

        if(spawnedEnemy.TryGetComponent(out SpawnEnemy enemyComponent))
        {
            enemyComponent.points = enemyPoints;
            enemyComponent.speed = enemySpeed;
            enemyComponent.hit = enemyHit;
            enemyComponent.GetComponent<AudioSource>().volume = GameObject.Find("AudioManager").GetComponent<BGmusic>().soundEffectsFloat;
        }

        if (spawnedEnemy.TryGetComponent(out SpawnEnemy2 enemy2Component))
        {
            enemy2Component.points = enemyPoints;
            enemy2Component.speed = enemySpeed;
            enemy2Component.stun = enemy2StunSound;
            enemy2Component.preDead = enemy2PreDeadSound;
            enemy2Component.dead = enemy2DeadSound;
            enemy2Component.GetComponent<AudioSource>().volume = GameObject.Find("AudioManager").GetComponent<BGmusic>().soundEffectsFloat;
            enemy2Component.healtPoints = enemy2HealthPoints;
        }

        StartCoroutine(SpawnTimer());
        spawnIndex++;
    }
    private void Update()
    { 
        if(spawnIndex < spawnLimit)
        {
            if(enemyIsSpawned)
            {
                if(spawnedEnemy == null)
                {
                    if(canSpawn)
                    {
                        StartCoroutine(SpawnEnemy());
                        enemyIsSpawned = false;
                    } 
                }
            }
        }
        else
        {
            return;
        }  
    }

    private IEnumerator SpawnTimer()
    {
        canSpawn = false;

        yield return new WaitForSeconds(spawnTimer);

        canSpawn = true;
    }

    public bool LimitExceeded(bool result)
    {
        if (spawnIndex >= spawnLimit && spawnedEnemy == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
