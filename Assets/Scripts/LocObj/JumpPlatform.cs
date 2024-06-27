using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    public AudioClip jumpSound;
    private AudioSource audioS;
    public float jumpForce;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out CharacterController2D player))
        {
            audioS.PlayOneShot(jumpSound, audioS.volume);
            player.rb.AddForce(player.transform.up * jumpForce, ForceMode2D.Impulse);
        }

        if (collision.TryGetComponent(out Cube cube))
        {
            audioS.PlayOneShot(jumpSound, audioS.volume);
            cube.rb.AddForce(collision.transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}

