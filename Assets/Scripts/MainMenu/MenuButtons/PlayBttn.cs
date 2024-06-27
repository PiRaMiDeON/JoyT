using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayBttn : MonoBehaviour
{
    public AudioManager audioManager;
    public AudioClip sound;
    private AudioSource audioS;
    public LvlMenuCntrl fader;
    public Animator LWAnim;
    public int delay;
    public int fadeDelay;
    private Animator anim;
    public int sceneLvlIndex;

    private void Start()
    {
        anim = GetComponent<Animator>();
        sceneLvlIndex = 100;
        audioS = GetComponent<AudioSource>();
    }
    private void Press()
    {
        anim.SetTrigger("Pressed");
    }

    private void OnMouseDown()
    {
        if(sceneLvlIndex == 100)
        {
            return;
        }

        audioS.PlayOneShot(sound, audioS.volume);
        StartCoroutine(audioManager.ExitFadeTrack(audioManager.music.clip));
        Press();
        StartCoroutine(ActivateLvl());
    }

    private IEnumerator ActivateLvl()
    {
        if (LWAnim != null)
        {
            LWAnim.SetTrigger("Activate");
            yield return new WaitForSeconds(delay);
        }

        fader.LoadLvl();
        yield return new WaitForSeconds(fadeDelay);
        SceneManager.LoadScene(sceneLvlIndex);
    }

   
}
