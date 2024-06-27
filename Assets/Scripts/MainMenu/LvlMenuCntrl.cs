using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;
using System;

public class LvlMenuCntrl : MonoBehaviour
{
    private bool AC_DontTransfer;
    public bool destroyMenu;
    public MenuFader fader;
    public int completeLevels;
    public int localeID;
    public MouseHighlighted[] levelButtons;
    public Transform RussianDifficlutPointsPosition;
    public Transform basePointsPosition;
    public GameObject difficultPoints;
    public GameObject switchMenuButton;

    private void Awake()
    {
        AC_DontTransfer = Convert.ToBoolean(PlayerPrefs.GetInt("DontTransfer"));
        completeLevels = PlayerPrefs.GetInt("completeLevels");
        localeID = PlayerPrefs.GetInt("localeID");

        SetLocale();

    }
    private void Start()
    {
        if(completeLevels >= 13)
        {
            switchMenuButton.SetActive(true);
        }

        if(Cursor.visible == false)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (!destroyMenu)
        {
            if (localeID == 0)
            {
                difficultPoints.transform.position = RussianDifficlutPointsPosition.position;
            }
            else
            {
                difficultPoints.transform.position = basePointsPosition.position;
            }
        }

        if (completeLevels >= 12 && !AC_DontTransfer)
        {
            if (SceneManager.GetActiveScene().buildIndex != 13)
            {
                SceneManager.LoadScene(13);
                return;
            }
        }

        if (levelButtons != null)
        {
            for (int i = 0; i < levelButtons.Length; i++)
            {
                if (completeLevels < levelButtons[i].levelPassingValue)
                {
                    levelButtons[i].buttonBlock = true;
                }
            }
        }

        StartCoroutine(StartLvlMenu());
    }

    public void LoadLvl()
    {
        StartCoroutine(Transition());
    }

    public IEnumerator Transition()
    {
        fader.gameObject.SetActive(true);

        yield return StartCoroutine(fader.Fade(true));
    }

    public IEnumerator StartLvlMenu()
    {
        yield return StartCoroutine(fader.Fade(false));

        fader.gameObject.SetActive(false);
    }

    public void SetLocale()
    {
        StartCoroutine(SetLocalizationSettings());
    }
    private IEnumerator SetLocalizationSettings()
    {
        yield return LocalizationSettings.InitializationOperation;

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
    }


}
