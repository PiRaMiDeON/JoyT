using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePoint : MonoBehaviour
{
    [HideInInspector] public Transform _transform;

    private void Start()
    {
        _transform = transform;
    }
}
