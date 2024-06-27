using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MouseHighlighted : MonoBehaviour
{
    public GameObject[] difficultPoints;
    public int pointsValue;
    public AudioClip sound;
    public string[] selectedLocInfo;
    public string[] information;
    public TMP_Text selectedLoc;
    public TMP_Text info;
    public PlayBttn playBttn;
    public int lvlIndex;
    [HideInInspector] public bool buttonBlock;
    public int levelPassingValue;

    private AudioSource audioS;
    private Animator anim;
    private int localeID;
    private void Start()
    {
        audioS = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        info.text = "---";
        selectedLoc.text = "---";

        localeID = PlayerPrefs.GetInt("localeID");

        for (int i = 0; i < difficultPoints.Length; i++)
        {
            difficultPoints[i].SetActive(false);
        }
    }

    private void ShowText()
    {
        localeID = PlayerPrefs.GetInt("localeID");
        info.text = information[localeID];
    }

    private void DontShowText()
    {
        info.text = "---";
    }
    private void LvlAnim()
    {
        anim.SetBool("Highlighted", true);
    }

    private void LvlDeAnim()
    {
        anim.SetBool("Highlighted", false);
    }


    private void OnMouseEnter()
    {
        if(buttonBlock)
        {
            return;
        }

        ShowText();
        LvlAnim();
    }

    private void OnMouseExit()
    {
        if (buttonBlock)
        {
            return;
        }

        DontShowText();
        LvlDeAnim();
    }

    private void OnMouseDown()
    {
        if (buttonBlock)
        {
            return;
        }

        ShowDifficultPoints();
        SelectLocationText();
        audioS.PlayOneShot(sound, audioS.volume);
        playBttn.sceneLvlIndex = lvlIndex;
    }

    private void SelectLocationText()
    {
        localeID = PlayerPrefs.GetInt("localeID");
        selectedLoc.text = selectedLocInfo[localeID];
    }

    private void ShowDifficultPoints()
    {

        if(pointsValue == 0)
        {
            for (int i = 0; i < difficultPoints.Length; i++)
            {
                if(difficultPoints[i].activeInHierarchy == true)
                {
                    difficultPoints[i].SetActive(false);
                }
            }
            return;
        }

        for (int i = 0; i < difficultPoints.Length; i++)
        {
            if(i >= pointsValue)
            {
                if(difficultPoints[i].activeInHierarchy == true)
                {
                    difficultPoints[i].SetActive(false);
                }
                else
                {
                    break;
                }
            }
        }

        for (int i = 0; i < pointsValue; i++)
        {
            difficultPoints[i].SetActive(true);
        }

        switch(pointsValue)
        {
            case ( >= 15):
                for (int i = 0; i < pointsValue; i++)
                {
                    difficultPoints[i].GetComponent<SpriteRenderer>().color = Color.red;
                }
                break;

            case ( >= 7):
                for (int i = 0; i < pointsValue; i++)
                {
                    difficultPoints[i].GetComponent<SpriteRenderer>().color = Color.yellow;
                }
                break;

            case ( < 7):
                for (int i = 0; i < pointsValue; i++)
                {
                    difficultPoints[i].GetComponent<SpriteRenderer>().color = Color.green;
                }
                break;
        }
    }
}
