using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatformChildPanel : MonoBehaviour
{
    private Animator anim;
    public MovePlatformStopPoint platform;
    private bool panelPressed;
    private Collider2D checkBoxCollider;
    public LayerMask playerLayer;
    public float Xsize, Ysize;

    private AudioSource audioS;
    public AudioClip press_sound;

    private void Start()
    {
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
    }

    private void Update()
    {
        panelPressed = anim.GetBool("Press");
        checkBoxCollider = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), new Vector2(Xsize, Ysize), 0, playerLayer);

        if (checkBoxCollider && Input.GetKeyDown(KeyCode.F))
        {
            ChangePlatformVectror();
        }
    }

    private void ChangePlatformVectror()
    {

        audioS.PlayOneShot(press_sound, audioS.volume);

        if (checkBoxCollider.TryGetComponent(out CharacterController2D player))
        {
            if (!panelPressed)
            {
                anim.SetBool("Press", true);
            }
            else
            {
                anim.SetBool("Press", false);
            }

            if (platform.stopMoving)
            {
                platform.stopMoving = false;
            }

            if (platform.temporaryStopMoving)
            {
                platform.temporaryStopMoving = false;
                return;
            }

            if(platform.ignoreTemporaryStopPanels)
            {
                platform.ignoreTemporaryStopPanels = false;
            }

            platform.speed *= -1;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(Xsize, Ysize, 1));
    }

}
