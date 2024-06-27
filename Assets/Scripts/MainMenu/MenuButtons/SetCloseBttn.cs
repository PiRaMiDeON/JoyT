using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetCloseBttn : MonoBehaviour
{
    public SettingsBttn settings;
    public TMP_Text settingsText;

    public VolumeManager volumeManager;
    public AudioSource LvlCntrlAudioSource;
    public AudioClip sound;
    public GameObject setWin;
    public GameObject[] objs;
    
    private void OnMouseDown()
    {
        setWin.SetActive(false);

        for (int i = 0; i < objs.Length; i++)
        {
            objs[i].SetActive(true);
        }

        settingsText.enabled = true;
        settings._collider.enabled = true;

        if(settings.spriteRen != null)
        {
            settings.spriteRen.enabled = true;
        }
        else
        {
            settings.image.enabled = true;
        }

        LvlCntrlAudioSource.PlayOneShot(sound, LvlCntrlAudioSource.volume);
        volumeManager.SaveSoundSettings();
    }

}
