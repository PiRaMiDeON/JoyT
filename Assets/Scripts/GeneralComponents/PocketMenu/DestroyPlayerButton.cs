using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPlayerButton : MonoBehaviour
{
    public CharacterController2D player;
    public VolumeManager volumeManager;
    public bool lvl12;
    public ToMenuButton toMenuButton;

    private void Start()
    {
        if(!lvl12)
        {
            player = GameObject.Find("Trip").GetComponent<CharacterController2D>();
        }
        else
        {
            player = GameObject.Find("Trip(Lvl 12)").GetComponent<CharacterController2D>();
        }
    }

    private void OnMouseDown()
    {
        toMenuButton.SetPocketMenu();
        volumeManager.SaveSoundSettings();
        player.Dead();
    }
}
