using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private AudioSource audioS;
    public AudioClip shot_sound;

    public ParticleSystem shotParticles;
    public GameObject projectile;
    public GameObject projectileSpawner;

    private Collider2D checkCircle;
    public Transform circleColliderPoint;
    public float colliderRadius;
    public LayerMask playerLayer;
    public Rigidbody2D turret;
    public Rigidbody2D textureRigidbody;

    Vector2 playerPosition;
    public GameObject player;

    public float shootingForce;

    private bool playerInAttackZone;
    private bool isShooting;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
    }
    private void Update()
    {
        checkCircle = Physics2D.OverlapCircle(new Vector2(circleColliderPoint.position.x, circleColliderPoint.position.y), colliderRadius, playerLayer);
        
        playerPosition = player.transform.position;
        Vector2 lookDir = playerPosition - turret.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        textureRigidbody.rotation = angle - 90f;
        turret.rotation = angle - 90f;

        if(PlayerIsNear(true))
        {
            playerInAttackZone = true;
        }
        else
        {
            playerInAttackZone = false;
            StopAllCoroutines();
            isShooting = false;
        }


        if (playerInAttackZone && !isShooting)
        {
            StartCoroutine(startShooting());
        }
    }

    private IEnumerator startShooting()
    {
        isShooting = true;

        yield return new WaitForSeconds(1f);

        shotParticles.Play();
        Shooting();

        yield return new WaitForSeconds(0.5f);

        shotParticles.Play();
        Shooting();

        yield return new WaitForSeconds(0.5f);

        isShooting = false;
    }
    private void Shooting()
    {
        audioS.PlayOneShot(shot_sound, audioS.volume);
        GameObject _projectile = Instantiate(projectile, new Vector2(projectileSpawner.transform.position.x, projectileSpawner.transform.position.y), projectileSpawner.transform.rotation);
        Rigidbody2D rb = _projectile.GetComponent<Rigidbody2D>();
        rb.AddForce(projectileSpawner.transform.up * shootingForce, ForceMode2D.Impulse);
    }

    /*private void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawWireDisc(gameObject.transform.position, Vector3.back, colliderRadius);
    }*/

    private bool PlayerIsNear(bool result)
    {
        if (checkCircle)
        {
            if (checkCircle.TryGetComponent(out CharacterController2D player))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

}
