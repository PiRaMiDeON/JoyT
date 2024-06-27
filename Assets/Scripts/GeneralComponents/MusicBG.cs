using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBG : MonoBehaviour
{
    
    public AudioSource musicBG;

    private void Start()
    {
        musicBG.Play();
    }

    public static MusicBG music;

    private void Awake()
    {
        
        if (music)
        {
            Destroy(gameObject);
        }
        music = this;
        DontDestroyOnLoad(gameObject);
    }
}

