using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushWindow : MonoBehaviour
{
    public SpriteRenderer pushRen;
    private void Start()
    {
        pushRen.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            pushRen.enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            pushRen.enabled = false;
        }
    }
}
