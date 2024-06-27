using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocalizationSelector : MonoBehaviour
{
    public int localeID;
    public Color pressedColor;
    public Color disabledColor;
    public LocalizationSelector[] otherLocaleSetButtons;

    public Transform RussianDifficlutPointsPosition;
    public Transform basePointsPosition;
    public GameObject difficultPoints;

    public bool destroyedMenu;

    [HideInInspector] public SpriteRenderer spriteRen;
    [HideInInspector] public bool buttonPressed;
    [HideInInspector] public Animator anim;

    private void Start()
    {
        spriteRen = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (PlayerPrefs.GetInt("localeID") == localeID)
        {
            spriteRen.color = pressedColor;
        }
        else
        {
            spriteRen.color = disabledColor;
        }
    }

    private void OnMouseDown()
    {
        if (!buttonPressed)
        {
            buttonPressed = true;
            spriteRen.color = pressedColor;
            SetLocale();

            PlayerPrefs.SetInt("localeID", localeID);
            PlayerPrefs.Save();

            if (!destroyedMenu)
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

            for (int i = 0; i < otherLocaleSetButtons.Length; i++)
            {
                otherLocaleSetButtons[i].buttonPressed = false;
                otherLocaleSetButtons[i].spriteRen.color = disabledColor;
            }
        }
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
