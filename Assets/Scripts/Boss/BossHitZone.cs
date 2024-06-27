using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitZone : MonoBehaviour
{
    public BossCntrl_phase1 boss;

    public void HitBoss()
    {
        if(boss.isHitting)
        {
            return;
        }
        boss.Hit();
    }
}
