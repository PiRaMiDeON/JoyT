using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPoint : MonoBehaviour
{
    public string position;
    public Transform saveTeleportPoint;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Boss_phase_1")
        {
            collision.GetComponent<BossCntrl_phase1>().bossPosition = position;
            collision.GetComponent<BossCntrl_phase1>().saveTeleportPoint = saveTeleportPoint;
        }

        if(collision.transform.tag == "Boss_phase_2")
        {

        }
    }
}
