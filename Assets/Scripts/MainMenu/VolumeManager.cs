using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string MusicPref = "MusicPref";
    private static readonly string SoundEffectsPref = "SoundEffectPref";
    private int firstPlayInt;
    public Slider musicSlider, soundEffectsSlider;
    private float musicFloat, soundEffectsFloat;
    public AudioSource musicAudio;
    public AudioSource[] soundEffectsAudio;

    void Start()
    {
        if (musicAudio == null)
        {
            musicAudio = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        }

        if (soundEffectsAudio.Length == 0)
        {
            soundEffectsAudio = GameObject.Find("AudioManager").GetComponent<BGmusic>().soundEffectsAudio;
        }

        firstPlayInt = PlayerPrefs.GetInt(FirstPlay);

        if (firstPlayInt == 0)
        {
            musicFloat = 0.25f;
            soundEffectsFloat = 0.75f;
            musicSlider.value = musicFloat;
            soundEffectsSlider.value = soundEffectsFloat;
            PlayerPrefs.SetFloat(MusicPref, musicFloat);
            PlayerPrefs.SetFloat(SoundEffectsPref, soundEffectsFloat);
            PlayerPrefs.SetInt(FirstPlay, -1);
        }

        else
        {
            musicFloat = PlayerPrefs.GetFloat(MusicPref);
            musicSlider.value = musicFloat;
            soundEffectsFloat = PlayerPrefs.GetFloat(SoundEffectsPref);
            soundEffectsSlider.value = soundEffectsFloat;
        }
    }

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(MusicPref, musicSlider.value);
        PlayerPrefs.SetFloat(SoundEffectsPref, soundEffectsSlider.value);
    }

    private void OnApplicationFocus(bool infocus)
    {
        if (!infocus)
        {
            SaveSoundSettings();
        }
    }

    public void UpdateSound()
    {
        if (musicAudio.mute != true)
        {
            musicAudio.volume = musicSlider.value;
        }

        for (int i = 0; i < soundEffectsAudio.Length; i++)
        {
            if (soundEffectsAudio[i] != null)
            {
                soundEffectsAudio[i].volume = soundEffectsSlider.value;
            }
        }
    }
}
