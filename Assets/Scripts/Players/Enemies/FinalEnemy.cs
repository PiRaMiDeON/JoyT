using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalEnemy : MonoBehaviour
{
    private bool enemyIsBoosted;

    private void Update()
    {
        if(!enemyIsBoosted)
        {
            if(gameObject.TryGetComponent(out SpawnEnemy enemy))
            {
                enemy.speed *= 1.5f;
                enemyIsBoosted = true;
            }

            if (gameObject.TryGetComponent(out SpawnEnemy2 enemy2))
            {
                enemy2.healtPoints += 2;
                enemy2.speed *= 1.5f;
                enemyIsBoosted = true;
            }
        }
    }
}
