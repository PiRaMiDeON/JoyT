using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovePlatformChangePoint : MonoBehaviour
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
            ChangePlatformVector();        
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(Xsize, Ysize, 1));
    }

    private void ChangePlatformVector()
    {
        audioS.PlayOneShot(press_sound, audioS.volume);

        if (checkBoxCollider.gameObject.TryGetComponent(out CharacterController2D player))
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

            if (platform.transform.position.y > gameObject.transform.position.y)
            {
                platform.ignoreTemporaryStopPanels = true;

                if (platform.speed > 0)
                {
                    platform.speed *= -1;
                }
            }
            else
            {
                platform.ignoreTemporaryStopPanels = true;

                if (platform.speed < 0)
                {
                    platform.speed *= -1;
                }
            }
        }
    }
}
