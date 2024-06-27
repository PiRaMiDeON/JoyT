using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    public ToMenuButton toMenuButton;
    public VolumeManager volumeManager;

    private void OnMouseDown()
    {
        toMenuButton.SetPocketMenu();
        volumeManager.SaveSoundSettings();
    }
}
