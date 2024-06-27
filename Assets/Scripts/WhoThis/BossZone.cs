using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using DG.Tweening;

public class BossZone : MonoBehaviour
{
    public Rigidbody2D playerRb;
    public CharacterController2D player;
    public float playerRotationSpeed;
    public float gravityChangeSpeed;

    public CinemachineVirtualCamera vcam;
    public CinemachineTransposer vcam_Body;
    public float changeValueTime, speed;

    public TMP_Text bossName;
    public float bossNameChangeSpeed;

    public BGmusic musicFloat;
    public AudioSource audioManager;
    public AudioSource bossAudioS;
    public WhoThisCntrl whoThis;

    public bool playerInZone;

    private void Start()
    {
        vcam_Body = vcam.GetCinemachineComponent<CinemachineTransposer>();
    }
    private void Update()
    {
        if (playerInZone)
        {
            ChangeGravityScale(0, gravityChangeSpeed);
            ChangeCameraSettings(12, 5);
            FadeBossName(1);

            if (!player.inBossZone)
            {
                player.inBossZone = true;
            }

            if (playerRb.constraints == RigidbodyConstraints2D.FreezeRotation)
            {
                playerRb.constraints = RigidbodyConstraints2D.None;
            }

            if (!player.IsGrounded())
            {
                playerRb.transform.Rotate(Vector3.forward, playerRotationSpeed);
            }
        }
        else
        {
            FadeBossName(0);
            ChangeGravityScale(5, gravityChangeSpeed);
            ChangeCameraSettings(3, 1);

            if (player.inBossZone)
            {
                player.inBossZone = false;
            }

            if (playerRb.transform.rotation.z != 0)
            {
                if (playerRb.constraints == RigidbodyConstraints2D.None)
                {
                    if (playerRb.transform.rotation == Quaternion.Euler(0, 0, 0))
                    {
                        playerRb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    }
                }
                playerRb.transform.rotation = Quaternion.Euler(playerRb.transform.rotation.x, playerRb.transform.rotation.y, Mathf.MoveTowards(playerRb.transform.rotation.z, 0, playerRotationSpeed));
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out CharacterController2D player))
        {
            playerInZone = true;
            bossAudioS.mute = false;
            audioManager.DOFade(musicFloat.musicFloat, changeValueTime);
            whoThis.StopAttacking(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out CharacterController2D player))
        {
            playerInZone = false;
            bossAudioS.mute = true;
            audioManager.DOFade(musicFloat.musicFloat / 2, changeValueTime);
            whoThis.StopAttacking(true);
        }
    }

    private void ChangeGravityScale(float newGravityScale, float speed)
    {
        playerRb.gravityScale = Mathf.MoveTowards(playerRb.gravityScale, newGravityScale, speed);
    }

    private void ChangeCameraSettings(float lens, float yDamping)
    {
        vcam.m_Lens.OrthographicSize = Mathf.Lerp(vcam.m_Lens.OrthographicSize, lens, speed);
        vcam_Body.m_YDamping = Mathf.Lerp(vcam_Body.m_YDamping, yDamping, speed);
    }

    private void FadeBossName(float alpha)
    {
        bossName.color = new Color(bossName.color.r, bossName.color.g, bossName.color.b, Mathf.MoveTowards(bossName.color.a, alpha, bossNameChangeSpeed));
    }

}
