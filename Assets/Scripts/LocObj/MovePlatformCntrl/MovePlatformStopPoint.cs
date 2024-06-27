using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatformStopPoint : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
    public float speed;
    public bool stopMoving;
    public bool temporaryStopMoving;
    public bool ignoreTemporaryStopPanels;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    private void Update()
    {
        if(stopMoving)
        {
            rb.velocity = new Vector2(0, 0);
            return;
        }
        
        if(!ignoreTemporaryStopPanels)
        {
            if(temporaryStopMoving)
            {
                rb.velocity = new Vector2(0, 0);
                return;
            }
        }

        rb.velocity = new Vector2(0, speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "StopPanel")
        {
            ignoreTemporaryStopPanels = false;
            stopMoving = true;
        }
        
        if(!ignoreTemporaryStopPanels)
        {
            if(collision.transform.tag == "TemporaryStopPanel")
            {   
                temporaryStopMoving = true;
            }
        }
    }
}
