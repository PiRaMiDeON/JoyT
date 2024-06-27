using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    public CharacterController2D charCntrl;
    public Animator interfaceAnim;
    public float posX;
    public float posY;
    public AudioManager audioManager;
    public ScriptEvent scriptEvent;
    public Animator playerAnim;
    public Fader fader;
    private BoxCollider2D boxColl;
    public static Collider2D[] overlapCircle;
    private Vector2 pos;
    public float radius;
    public Animator anim;
    [SerializeField]private bool near;
    public bool unlocked;
    public bool dontFalseActive; // включаешь, если выход появляется на сцене не сразу, а через SetActive
    public bool active;
    public int NextSceneIndex;
    private void Start()
    {
        boxColl = GetComponent<BoxCollider2D>();
        pos = new Vector2((float)posX , (float)posY);
        anim = GetComponent<Animator>();

        if(!dontFalseActive)
        {
            active = false;
        }

        unlocked = false; 
    }
    private void Update()
    {
        
       if(active == true && unlocked == true && Input.GetKeyDown(KeyCode.E) && near == true)
        {

            if(scriptEvent == null)
            {
                StartCoroutine(audioManager.ExitFadeTrack(audioManager.music.clip));
            }
            else
            {
                StartCoroutine(scriptEvent.ExitFadeEventTrack(scriptEvent.tracks[scriptEvent.currentTrack]));
            }

            StartCoroutine(ChangeScene());
            return;
        }
        
       if(Input.GetKeyDown(KeyCode.E) && near)
        {
            UnLocked();
        }

    }

   public void UnLocked()
    {
        anim.SetTrigger("Open");
        unlocked = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            near = true;
        } 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            near = false;
        }
    }

    private IEnumerator ChangeScene()
    {
        if(PlayerPrefs.GetInt("completeLevels") < SceneManager.GetActiveScene().buildIndex)
        {
            PlayerPrefs.SetInt("completeLevels", SceneManager.GetActiveScene().buildIndex);
        }

        charCntrl.audioS.PlayOneShot(charCntrl.teleport, charCntrl.audioS.volume);
        playerAnim.SetTrigger("Disappear");
        fader.gameObject.SetActive(true);
        interfaceAnim.SetTrigger("Death");

        yield return StartCoroutine(fader.Fade(true));

        PlayerPrefs.Save();

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(NextSceneIndex);
    }

}
