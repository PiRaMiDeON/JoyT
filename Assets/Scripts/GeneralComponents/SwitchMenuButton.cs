using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchMenuButton : MonoBehaviour
{
    public LvlMenuCntrl lvlMenuCntrl;

    private void OnMouseDown()
    {
        SwitchMenu();
    }

    private void SwitchMenu()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlayerPrefs.SetInt("DontTransfer", 0);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            PlayerPrefs.SetInt("DontTransfer", 1);
            SceneManager.LoadScene(0);
        }
    }
}
