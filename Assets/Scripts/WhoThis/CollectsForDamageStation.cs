using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectsForDamageStation : MonoBehaviour
{
    public GameObject[] collect;
    public AudioClip attackSignal_Sound;
    public AudioClip spawnCollect_Sound;
    public AudioClip[] collectSounds;
    public Transform[] possibleSpawnPositions;
    public List<Transform> spawnPositions;
    public DamageBossStation bossStation;
    public int collectionsCount;
    private int collectSoundIndex, collectIndex = 0;
    private AudioSource audioS;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if(collectionsCount >= bossStation.collectionsValue)
        {
            StartCoroutine(ActivateStation());
        }
    }
    public IEnumerator SpawnCollections()
    {
        for (int i = 0; i < bossStation.collectionsValue; i++)
        {
            int rand = Random.Range(0, spawnPositions.Count);

            Instantiate(collect[collectIndex], spawnPositions[rand].position, collect[collectIndex].transform.rotation);
            collectIndex++;

            spawnPositions.RemoveAt(rand);

            audioS.PlayOneShot(spawnCollect_Sound, audioS.volume);

            yield return new WaitForSeconds(0.3f);
        }

        collectIndex = 0;
        spawnPositions = possibleSpawnPositions.ToList();
    }

    private IEnumerator ActivateStation()
    {
        collectionsCount = 0;
        audioS.PlayOneShot(attackSignal_Sound, audioS.volume);

        yield return new WaitForSeconds(2);

        bossStation.SpawnCube();

        yield return new WaitForSeconds(5f);

        StartCoroutine(SpawnCollections());
    }

    public void PlayCollectSound()
    {
        audioS.PlayOneShot(collectSounds[collectSoundIndex], audioS.volume);
        collectSoundIndex++;

        if(collectSoundIndex == collectSounds.Length)
        {
            collectSoundIndex = 0;
        }
    }
}
