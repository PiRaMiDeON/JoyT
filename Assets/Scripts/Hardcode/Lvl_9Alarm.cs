using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl_9Alarm : MonoBehaviour
{
    public GameObject flashing;
    public Animator redFlashing;
    public AudioClip alarm;
    public AudioManager audioManager;
    private AudioSource audioS;

    public ScriptEvent scriptEvent;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
        StartCoroutine(Sound());
    }

    private IEnumerator Sound()
    {
        StartCoroutine(audioManager.ExitFadeTrack(audioManager.tracks[audioManager.currentTrack]));
        audioS.PlayOneShot(alarm, audioS.volume);
        redFlashing.SetTrigger("Start");

        yield return new WaitForSeconds(12.3f);

        scriptEvent.StartEvent();
        Destroy(flashing, 3);
        Destroy(gameObject);
    }
}
