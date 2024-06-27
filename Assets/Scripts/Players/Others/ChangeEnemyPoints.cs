using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEnemyPoints : MonoBehaviour
{
    public List<Transform> changePoints;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out SpawnEmailBot spawnedEmailBot))
        {
            spawnedEmailBot.StopAllCoroutines();
            spawnedEmailBot.points = changePoints;
            StartCoroutine(spawnedEmailBot.Stay());
        }

        if (collision.TryGetComponent(out EmailBot EmailBot))
        {
            EmailBot.StopAllCoroutines();
            EmailBot.points = changePoints;
            StartCoroutine(EmailBot.Stay());
        }

        if (collision.TryGetComponent(out Enemy enemy))
        {
            enemy.StopAllCoroutines();
            enemy.points = changePoints;
            StartCoroutine(enemy.Stay());
        }

        if (collision.TryGetComponent(out SpawnEnemy spawnedEnemy))
        {
            spawnedEnemy.StopAllCoroutines();
            spawnedEnemy.points = changePoints;
            StartCoroutine(spawnedEnemy.Stay());
        }
    }
}
