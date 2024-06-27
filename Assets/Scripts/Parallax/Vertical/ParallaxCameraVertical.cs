using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxCameraVertical : MonoBehaviour
{
    public delegate void ParallaxCameraDelegate(float deltaMovement);
    public ParallaxCameraDelegate onCameraTranslate;
    private float oldPosition;
    void Start()
    {
        oldPosition = transform.position.y;
    }
    void Update()
    {
        if (transform.position.y != oldPosition)
        {
            if (onCameraTranslate != null)
            {
                float delta = oldPosition - transform.position.y;
                onCameraTranslate(delta);
            }
            oldPosition = transform.position.y;
        }
    }
}
