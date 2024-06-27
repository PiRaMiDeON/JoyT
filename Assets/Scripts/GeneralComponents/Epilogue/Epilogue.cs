using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.Localization.Settings;

public class Epilogue : MonoBehaviour
{
    public AudioManager audioManager;
    public TMP_Text epilogueText;
    public Animator epilogueFaderAnimator, finalImage, applicationQuiteAnim;
    public ArrayLayout showingText;
    public ArrayLayout titlesText;
    private string[] selectedShowingText;
    public string[] selectedTitlesText;
    public float textShowingSpeed = 0, moveTime;
    public Transform teleportYTargetPosition, yTargetPosition;
    public AudioClip[] textSounds;
    private int textSoundIndex, lastTextSoundIndex = -1, textIndex = 0, localeID;
    private bool first = true;
    private AudioSource audioS;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
        StartCoroutine(ShowingText());
    }

    public void epilogueMoveX(float x, float time)
    {
        transform.DOMoveX(transform.position.x + x, time);
    }

    private IEnumerator ShowingText()
    {
        yield return LocalizationSettings.InitializationOperation;

        localeID = PlayerPrefs.GetInt("localeID");
        epilogueText.text = null;
        selectedShowingText = showingText.data[localeID].showingTexts;

        for (; textIndex < selectedShowingText.Length; textIndex++)
        {
            for (int j = 0; j < selectedShowingText[textIndex].Length; j++)
            {
                epilogueText.text += selectedShowingText[textIndex][j];

                do
                {
                    textSoundIndex = Random.Range(0, textSounds.Length);
                } while (textSoundIndex == lastTextSoundIndex);
                lastTextSoundIndex = textSoundIndex;

                audioS.PlayOneShot(textSounds[textSoundIndex], audioS.volume);

                yield return new WaitForSeconds(textShowingSpeed);
            }

            epilogueText.text += ' ';

            if (textIndex >= 2)
            {
                textIndex++;

                if (textIndex >= selectedShowingText.Length)
                {
                    StartCoroutine(StartTitles());
                }
                else
                {
                    yield return new WaitForSeconds(textShowingSpeed * 10);

                    yield return HideText(false);

                    yield break;
                }
            }
            else
            {
                yield return new WaitForSeconds(textShowingSpeed * 10);
            }
        }
    }

    private IEnumerator HideText(bool titles)
    {
        epilogueText.DOFade(0, 1);

        yield return new WaitForSeconds(1);

        epilogueText.text = null;

        if (!titles)
        {
            epilogueText.DOFade(1, 0);
        }
        else
        {
            if (first)
            {
                first = false;
                transform.position =teleportYTargetPosition.position;
                epilogueText.alignment = TextAlignmentOptions.Center;
                epilogueText.alignment = TextAlignmentOptions.Midline;
                selectedTitlesText = titlesText.data[localeID].showingTexts;
            }
        }

        yield return new WaitForSeconds(0.5f);

        if (titles)
        {
            epilogueText.text = selectedTitlesText[0];
            epilogueText.DOFade(1, 1);
        }
        else
        {
            StartCoroutine(ShowingText());
        }
    }

    private IEnumerator StartTitles()
    {
        StartCoroutine(HideText(true));

        yield return new WaitForSeconds(1f);

        epilogueFaderAnimator.SetTrigger("Activate");

        gameObject.transform.DOMoveY(yTargetPosition.position.y, moveTime).SetEase(Ease.Linear);

        yield return new WaitForSeconds(moveTime);

        finalImage.SetTrigger("Show");
        StartCoroutine(audioManager.ExitFadeTrack(audioManager.tracks[audioManager.currentTrack]));

        yield return new WaitForSeconds(5);

        applicationQuiteAnim.SetTrigger("Activate");

        yield return new WaitForSeconds(3.3f);

        Application.Quit();
    }
}
