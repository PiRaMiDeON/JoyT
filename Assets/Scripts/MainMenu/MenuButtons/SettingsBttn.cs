using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SettingsBttn : MonoBehaviour
{
    public GameObject setWin;
    public GameObject[] objs;
    public TMP_Text settingsText;

    public AudioClip sound;
    private AudioSource audioS;
    private Animator anim;
    [HideInInspector] public BoxCollider2D _collider;
    [HideInInspector] public SpriteRenderer spriteRen;
    [HideInInspector] public UnityEngine.UI.Image image;
    public bool settingsOpen;

    private void Start()
    {
        if(TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            spriteRen = GetComponent<SpriteRenderer>();
        }
        else
        {
            image = GetComponent<UnityEngine.UI.Image>();
        }
        
        _collider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        settingsOpen = false;
        audioS = GetComponent<AudioSource>();
    }
    private IEnumerator Press()
    {
        anim.SetTrigger("Pressed");
        audioS.PlayOneShot(sound, audioS.volume);
        settingsOpen = true;
        
        yield return new WaitForSeconds(0.2f);

        settingsText.enabled = false;
        _collider.enabled = false;

        if(spriteRen != null)
        {
            spriteRen.enabled = false;
        }
        else
        {
            image.enabled = false;
        }

        for (int i = 0; i < objs.Length; i++)
        {
            objs[i].SetActive(false);
        }

        setWin.SetActive(true);
    }

    private void OnMouseDown()
    {
        StartCoroutine(Press());
    }
}
