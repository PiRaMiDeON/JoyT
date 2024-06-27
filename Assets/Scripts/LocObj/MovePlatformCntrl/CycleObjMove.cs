using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleObjMove : MonoBehaviour
{
    public bool easeInOutSine;
    public bool dontMoveChildobject;
    public float time = 0; 
    public float Xdist = 0;
    public float Ydist = 0;

    private Transform _object;
    private Vector3 movePosition;

    void Start()
    {
        if(!dontMoveChildobject)
        {
            _object = transform.GetChild(0);
        }
        else
        {
            _object = gameObject.transform;
        }

        movePosition = new Vector2(Xdist, Ydist);
        Move(movePosition);
    }

    private void Move(Vector2 target)
    {
        if(!easeInOutSine)
        {
            _object.DOMove(target, time).SetEase(Ease.OutCubic).SetLoops(-1, LoopType.Yoyo).SetRelative(true);
        }
        else
        {
            _object.DOMove(target, time).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetRelative(true);
        }
    }

}

