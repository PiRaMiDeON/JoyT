using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxCameraCursor : MonoBehaviour
{
    public delegate void ParallaxCameraDelegate(float XdeltaMovement, float YdeltaMovement);
    public ParallaxCameraDelegate onCameraTranslate;
    private float XoldPosition;
    private float YoldPosition;
    void Start()
    {
        XoldPosition = Input.mousePosition.x;
        YoldPosition = Input.mousePosition.y;
    }
    void Update()
    {
        if (Input.mousePosition.x != XoldPosition)
        {
            if (onCameraTranslate != null)
            {
                float Xdelta = XoldPosition - Input.mousePosition.x;
                float Ydelta = YoldPosition - Input.mousePosition.y;

                onCameraTranslate(Xdelta, Ydelta);
            }
            XoldPosition = Input.mousePosition.x;
            YoldPosition = Input.mousePosition.y;
        }
    }
}
