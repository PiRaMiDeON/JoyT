using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton_Pocket : MonoBehaviour
{
    public int lvlMenuIndex;
    public ToMenuButton toMenuButton;
    public VolumeManager volumeManager;
    public Fader fader;

    private void OnMouseDown()
    {
        StartCoroutine(fader.Fade(false));
        volumeManager.SaveSoundSettings();
        toMenuButton.SetPocketMenu();
        SceneManager.LoadScene(lvlMenuIndex);
    }
}
