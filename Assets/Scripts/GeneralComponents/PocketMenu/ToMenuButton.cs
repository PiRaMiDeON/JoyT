using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMenuButton : MonoBehaviour
{
    public GameObject pocketMenu;
    public CharacterController2D player;
    [HideInInspector] public bool open, levelJustStart;
    public AudioClip openSound;
    public AudioClip closeSound;
    private AudioSource audioS;
    public VolumeManager volumeManager;
    public bool lvl12;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        audioS = GetComponent<AudioSource>();
        if (!lvl12)
        {
            player = GameObject.Find("Trip").GetComponent<CharacterController2D>();
        }
        else
        {
            player = GameObject.Find("Trip(Lvl 12)").GetComponent<CharacterController2D>();
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !levelJustStart)
        {
            if (!player.PlayerIsDead(true))
            {
                SetPocketMenu();
            }
        }
    }

    public void OnMouseDown()
    {
        if (!player.PlayerIsDead(true))
        {
            SetPocketMenu();
        }
    }

    public void SetPocketMenu()
    {
        if (!open)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            open = true;
            audioS.PlayOneShot(openSound, audioS.volume);
            pocketMenu.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            open = false;
            audioS.PlayOneShot(closeSound, audioS.volume);
            pocketMenu.SetActive(false);
        }

        volumeManager.SaveSoundSettings();
    }
}
