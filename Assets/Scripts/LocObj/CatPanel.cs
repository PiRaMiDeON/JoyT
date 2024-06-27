using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CatPanel : MonoBehaviour
{
    private Animator anim;
    private AudioSource audioS;
    private bool playerIsNear;
    private bool panelPressed;
    private bool soundChanging;
    private AudioClip saveAudioClip;

    public AudioClip activating_sound;
    public AudioClip angry_sound;
    public AudioClip[] normal_sounds;
    public ScriptEvent scriptEvent;

    public bool Change;

    private void Start()
    {
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (playerIsNear && !panelPressed && Input.GetKeyDown(KeyCode.F) || Change)
        {
            panelPressed = true;
            StartCoroutine(Activate());
        }

        if (!soundChanging && !panelPressed)
        {
            StartCoroutine(ChangeSound());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out CharacterController2D player))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out CharacterController2D player))
        {
            playerIsNear = false;
        }
    }

    private IEnumerator Activate()
    {
        anim.SetTrigger("Activate");
        audioS.PlayOneShot(activating_sound, audioS.volume);

        yield return new WaitForSeconds(2f);

        audioS.clip = angry_sound;
        audioS.Play();

        if (scriptEvent != null)
        {
            scriptEvent.StartEvent();
        }
    }

    private IEnumerator ChangeSound()
    {
        soundChanging = true;

        AudioClip clip = normal_sounds[Random.Range(0, normal_sounds.Length)];

        while (clip == saveAudioClip)
        {
            clip = normal_sounds[Random.Range(0, normal_sounds.Length)];
        }

        saveAudioClip = clip;
        audioS.PlayOneShot(clip, audioS.volume);

        yield return new WaitForSeconds(3f);

        soundChanging = false;
    }
}
