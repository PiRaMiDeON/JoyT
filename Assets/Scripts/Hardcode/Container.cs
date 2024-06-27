using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    public GameObject deleteCollider;
    private AudioSource audioS;
    public ScriptEvent scriptEvent;
    private BoxCollider2D _collider;

    private void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        audioS = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out CharacterController2D player))
        {
            OpenContainer();
        }
    }

    private void OpenContainer()
    {
        _collider.enabled = false;
        deleteCollider.SetActive(false);
        audioS.Play();
        scriptEvent.StartEvent();
    }
}
