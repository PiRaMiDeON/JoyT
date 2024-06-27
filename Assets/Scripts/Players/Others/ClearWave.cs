using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClearWave : MonoBehaviour
{
    public float clearColliderRadius;
    private Collider2D clearCircle;
    public LayerMask enemyLayer;
    public Transform circleColliderPoint;
    public EmailBot emailBot;

    private void Update()
    {
        clearCircle = Physics2D.OverlapCircle(new Vector2(circleColliderPoint.position.x, circleColliderPoint.position.y), clearColliderRadius, enemyLayer);

        if (clearCircle)
        {
            if (clearCircle.TryGetComponent(out Enemy enemy))
            {
                if (enemy.isDead)
                {
                    emailBot.walking = true;
                    emailBot.Walk();
                    return;
                }
                else
                {
                    emailBot.walking = true;
                    enemy.Hit();
                    emailBot.Walk();
                    return;
                }
            }
            
            if (clearCircle.TryGetComponent(out SpawnEnemy spawnedEnemy))
            {
                if (spawnedEnemy.isDead)
                {
                    emailBot.walking = true;
                    emailBot.Walk();
                    return;
                }
                else
                {
                    emailBot.walking = true;
                    spawnedEnemy.Hit();
                    emailBot.Walk();
                    return;
                }
            }
        }
    }

    /*private void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawWireDisc(gameObject.transform.position, Vector3.back, clearColliderRadius);
    }*/
}
