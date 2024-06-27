using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RestartLevelButton : MonoBehaviour
{
    public ToMenuButton toMenuButton;
    public VolumeManager volumeManager;


    private void OnMouseDown()
    {
        toMenuButton.SetPocketMenu();
        volumeManager.SaveSoundSettings();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
