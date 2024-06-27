using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamAim : MonoBehaviour
{
    public bool camActive = false;
    private float time;
    private int delay = 3;
    private CinemachineVirtualCamera vcam;
    public Transform[] aims;

    private void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
    }
    private void Update()
    {
        if(camActive == true)
        {
        vcam.Priority = 11;
        time += Time.deltaTime;
        vcam.Follow = aims[Mathf.Clamp(Mathf.FloorToInt(time / delay), 0, aims.Length-1)];
        }
       
    }



}
