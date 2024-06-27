using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundAnimCntrl : MonoBehaviour
{
    public float period;
    public int index;
    private Animator anim;

    private void Start()
    {
        index = 0;
        anim = GetComponent<Animator>();
        StartCoroutine(RandomIndex());
    }

   
    private IEnumerator RandomIndex()
    {
        while(true)
        {
            index = Random.Range(3, 8);

            yield return new WaitForSeconds(period);
        }
        
    }

    private void Update()
    { 
        switch (index)
        {
            case 1:
                anim.SetInteger("Index", index);
                break;

            case 2:
                anim.SetInteger("Index", index);
                break;

            case 3:
                anim.SetInteger("Index", index);
                break;

            case 4:
                anim.SetInteger("Index", index);
                break;

            case 5:
                anim.SetInteger("Index", index);
                break;

            case 6:
                anim.SetInteger("Index", index);
                break;

            case 7:
                anim.SetInteger("Index", index);
                break;

            case 8:
                anim.SetInteger("Index", index);
                break;

        }
    }
}   

