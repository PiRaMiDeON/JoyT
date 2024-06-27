using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObj : MonoBehaviour
{
    public Rigidbody2D cubeRB;
    public GameObject obj;
    Vector2 platformPosition;

    private void Awake()
    {
        platformPosition = obj.transform.position;
        obj.transform.position = new Vector2(20, 20);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            cubeRB.constraints = RigidbodyConstraints2D.None;
            obj.transform.position = platformPosition;
            Destroy(gameObject);
        }
    }
}
