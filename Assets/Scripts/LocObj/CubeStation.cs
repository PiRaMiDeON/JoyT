using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeStation : MonoBehaviour
{
    public List<Transform> points;
    public float speed;
    public AudioClip hitSound;

    public GameObject spawnObject;
    public GameObject spawnedObject;
    public Transform spawnPoint;

    private AudioSource audioS;
    public AudioClip spawnSound;

    private bool isReloading;
    private bool startSpawn;
    [HideInInspector] public bool notSpawn;
    public bool spawnInStart;
    public float reloadTime;
    public float delay;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();  

        if(spawnInStart)
        {
            StartCoroutine(SpawnDelay());
        }
    }

    public void Spawn()
    {
        startSpawn = true;
        isReloading = true;
        audioS.PlayOneShot(spawnSound, audioS.volume);
        spawnedObject = Instantiate(spawnObject, new Vector2(spawnPoint.position.x, spawnPoint.position.y), spawnObject.transform.rotation, gameObject.transform);

        if(spawnedObject.TryGetComponent(out SpawnEnemy enemy))
        {
            enemy.points = points;
            enemy.speed = speed;
            enemy.hit = hitSound;
        }

        if(spawnedObject.TryGetComponent(out AudioSource objectAudioSource))
        {
            objectAudioSource.volume = GameObject.Find("AudioManager").GetComponent<BGmusic>().soundEffectsFloat;
        }

        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(reloadTime);

        isReloading = false;
    }

    private void Update()
    {
        if(!isReloading && startSpawn && SpawnedObjectDestroy(true) && !notSpawn)
        {
            Spawn();
        }
    }

    private bool SpawnedObjectDestroy(bool result)
    {
        if(spawnedObject == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(delay);

        Spawn();
    }

}
