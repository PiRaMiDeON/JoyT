using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackgroundCursor : MonoBehaviour
{
    public ParallaxCameraCursor parallaxCamera;
    List<ParallaxLayerCursor> parallaxLayers = new List<ParallaxLayerCursor>();

    void Start()
    {
        if (parallaxCamera == null)
            parallaxCamera = Camera.main.GetComponent<ParallaxCameraCursor>();
        if (parallaxCamera != null)
            parallaxCamera.onCameraTranslate += Move;
        SetLayers();
    }

    void SetLayers()
    {
        parallaxLayers.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            ParallaxLayerCursor layer = transform.GetChild(i).GetComponent<ParallaxLayerCursor>();

            if (layer != null)
            {
                layer.name = "Layer-" + i;
                parallaxLayers.Add(layer);
            }
        }
    }
    void Move(float Xdelta, float Ydelta)
    {
        foreach (ParallaxLayerCursor layer in parallaxLayers)
        {
            layer.Move(Xdelta, Ydelta);
        }
    }
}
