using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutTrigger : MonoBehaviour
{
    public DialogEdgesActivator DEA;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            DEA.EdgesActivate();
            DEA.DialogActivate();
        }

    }
}
