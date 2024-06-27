using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [HideInInspector] public int currentTrack;
    public AudioClip[] tracks;
    private float volumeValue;
    public static AudioManager instance;
    public AudioSource music;
    public float timeFade;
    [HideInInspector] public bool lvlsEnd;

    private void Awake()
    {
        lvlsEnd = false;

        if(instance == null)
        {
            instance = this;
        }

        currentTrack = 0;
    }
    public IEnumerator ExitFadeTrack(AudioClip newClip)
    {
        lvlsEnd = true;

        float timeToFade = timeFade;
        float timeElapsed= 0;

        music.clip = newClip;

        while(timeElapsed < timeToFade)
        {
            music.volume = Mathf.Lerp(volumeValue, 0, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        music.Stop();
        music.mute = true;
    }

    private void Update()
    {
        volumeValue = music.volume;

        if(!music.isPlaying && music.enabled && !lvlsEnd)
        {
            currentTrack++;

            if(currentTrack >= tracks.Length)
            {
                currentTrack = 0;
                music.clip = tracks[currentTrack];
                music.Play();
                return;
            }
            
            music.clip = tracks[currentTrack];
            music.Play();
        }
    }

}
