using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBossStation : MonoBehaviour
{
    public GameObject explosionCubeForBoss;
    public float cubeRotationValue, cubeRotationTime, cubeImpulseForce;
    [HideInInspector] public int collectionsValue = 3;
    public Transform spawnPosition;
    private AudioSource audioS;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            SpawnCube();
        }
    }

    public void SpawnCube()
    {
        GameObject _explosionCube = Instantiate(explosionCubeForBoss, spawnPosition.position, explosionCubeForBoss.transform.rotation);
        Rigidbody2D cubeRigidbody = _explosionCube.GetComponent<Rigidbody2D>();
        _explosionCube.GetComponent<AudioSource>().volume = audioS.volume;
        cubeRigidbody.gravityScale = 0;

        int rand = Random.Range(-1, 2);

        while(rand == 0)
        {
            rand = Random.Range(-1, 2);
        }

        cubeRigidbody.AddForce(new Vector2(0, -cubeImpulseForce), ForceMode2D.Impulse);
        _explosionCube.transform.DORotate(new Vector3(0, 0, cubeRotationValue * rand), cubeRotationTime, RotateMode.FastBeyond360);
    }
}
