using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackgroundVertical : MonoBehaviour
{
    public ParallaxCameraVertical parallaxCamera;
    List<ParallaxLayerVertical> parallaxLayers = new List<ParallaxLayerVertical>();

    void Start()
    {
        if (parallaxCamera == null)
            parallaxCamera = Camera.main.GetComponent<ParallaxCameraVertical>();
        if (parallaxCamera != null)
            parallaxCamera.onCameraTranslate += Move;
        SetLayers();
    }

    void SetLayers()
    {
        parallaxLayers.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            ParallaxLayerVertical layer = transform.GetChild(i).GetComponent<ParallaxLayerVertical>();

            if (layer != null)
            {
                layer.name = "Layer-" + i;
                parallaxLayers.Add(layer);
            }
        }
    }
    void Move(float delta)
    {
        foreach (ParallaxLayerVertical layer in parallaxLayers)
        {
            layer.Move(delta);
        }
    }
}
