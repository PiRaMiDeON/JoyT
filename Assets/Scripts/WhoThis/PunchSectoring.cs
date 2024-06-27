using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchSectoring : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D rb;
    [HideInInspector] public Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Update()
    {
        Vector2 lookDir = (Vector2)player.transform.position - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle - 180f;
    }
}
