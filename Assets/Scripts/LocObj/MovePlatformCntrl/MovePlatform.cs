using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{

    public Vector2 last;
    public Vector2 velocity;
    
    private void Update()
    {
        velocity = (Vector2)transform.position - last;
        last = transform.position;
    }
}
