using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusBallShooter : MonoBehaviour
{
    public float delay;
    public bool Mute;
    public Transform spawnPosition;
    public GameObject virusBall;
    private AudioSource audioS;
    public AudioClip spawnSound;

    public float spawnTime;

    [SerializeField] private bool isShooting;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();

        audioS.volume /= 3;

        if(Mute)
        {
            audioS.volume = 0;
        }

        isShooting = false;
        StartCoroutine(StartSpawn());
    }

    public IEnumerator spawnVirusBall()
    {
        isShooting = true;
        
        GameObject spawnedVirusBall = Instantiate(virusBall, new Vector2(spawnPosition.transform.position.x, spawnPosition.transform.position.y), virusBall.transform.rotation);
        spawnedVirusBall.GetComponent<AudioSource>().volume = audioS.volume;

        if (!Mute)
        {
            audioS.PlayOneShot(spawnSound, audioS.volume);
        }

        yield return new WaitForSeconds(spawnTime);

        isShooting = false;
    }

    private void Update()
    {
        if(isShooting)
        {
            return;
        }

        StartCoroutine(spawnVirusBall());
    }

    private IEnumerator StartSpawn()
    {

        isShooting = true;

        yield return new WaitForSeconds(delay);

        StartCoroutine(spawnVirusBall());
    }
}
