using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandBehaviour : MonoBehaviour
{
    [HideInInspector] public SpriteRenderer spriteRen;
    public HandBehaviour secondHand;
    public Sprite[] hands_Phase_1;
    public Sprite[] hands_Phase_2;
    public Sprite prePunch, punch_Left, punch_Right;
    private GameObject basePosition;
    public float time, speed;
    private bool isPunching;
    public bool phase2;

    private void Start()
    {
        spriteRen = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if(!isPunching)
        {
            transform.position = Vector2.MoveTowards(transform.position, basePosition.transform.position, speed);
        }
    }

    public IEnumerator Punch(float preparationTime, bool playerIsLeft)
    {
        isPunching = true;
        gameObject.transform.DOScale(new Vector2(4, 4), time / 2);
        gameObject.transform.DOMove(basePosition.transform.position, time / 2);
        spriteRen.sprite = prePunch;

        yield return new WaitForSeconds(preparationTime);

        if (playerIsLeft)
        {
            spriteRen.sprite = punch_Left;
        }
        else
        {
            spriteRen.sprite = punch_Right;
        }

        yield return new WaitForSeconds(2f);

        isPunching = false;
    }
}
