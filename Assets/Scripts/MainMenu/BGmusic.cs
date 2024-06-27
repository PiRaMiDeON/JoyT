using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGmusic : MonoBehaviour
{
    private static readonly string MusicPref = "MusicPref";
    private static readonly string SoundEffectsPref = "SoundEffectPref";
    [HideInInspector] public float musicFloat, soundEffectsFloat;
    public AudioSource musicAudio;
    public AudioSource[] soundEffectsAudio;

    public static BGmusic instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        musicAudio.volume = 0;  
    }

    private void Start()
    {
        SoundsSettingsLoad();
        StartCoroutine(StartFadeTrack(musicAudio.clip));

        for (int i = 0; i < soundEffectsAudio.Length; i++)
        {
            soundEffectsAudio[i].volume = soundEffectsFloat;
        }
    }

    private IEnumerator StartFadeTrack(AudioClip newClip)
    {
        float timeToFade = 2f;
        float timeElapsed = 0;

        musicAudio.clip = newClip;
        musicAudio.Play();

        while (timeElapsed < timeToFade)
        {
            musicAudio.volume = Mathf.Lerp(0, musicFloat, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
 
    }

    private void SoundsSettingsLoad()
    {
        musicFloat = PlayerPrefs.GetFloat(MusicPref);
        soundEffectsFloat = PlayerPrefs.GetFloat(SoundEffectsPref);
    }
}
