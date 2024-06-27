using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayerCursor : MonoBehaviour
{
    public float XparallaxFactor;
    public float YparallaxFactor;
    public void Move(float Xdelta, float Ydelta)
    {
        Vector3 newPos = transform.localPosition;
        newPos.x -= Xdelta * -XparallaxFactor;
        newPos.y -= Ydelta * -YparallaxFactor;
        transform.localPosition = newPos;
    }
}
