using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MenuLoop : MonoBehaviour
{
    private AudioSource audioS;
    public AudioClip[] boomSounds;
    private int soundIndex;
    private int saveSoundIndex;

    public float lowerLoopTimeLimit;
    public float higherLoopTimeLimit;

    private bool ignoreCode;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
        ignoreCode = true;
        StartCoroutine(Loop());
    }
    private IEnumerator Loop()
    {
        if(!ignoreCode)
        {        
            while(soundIndex == saveSoundIndex)
            {
                soundIndex = Random.Range(0, boomSounds.Length);
            }

            saveSoundIndex = soundIndex;
            audioS.PlayOneShot(boomSounds[soundIndex], audioS.volume);
        }

        if(ignoreCode)
        {
            ignoreCode = false; 
        }

        yield return new WaitForSeconds(Random.Range(lowerLoopTimeLimit, higherLoopTimeLimit + 1));

        StartCoroutine(Loop());
    }

}
