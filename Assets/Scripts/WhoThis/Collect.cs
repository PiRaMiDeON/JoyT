using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public CollectsForDamageStation collectCntrl;
    public Transform flyTarget;
    public ParticleSystem explosionPartSys;
    public SpriteRenderer glowSpriteRen;
    private bool touched;
    private CircleCollider2D _collider;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        flyTarget = GameObject.Find("Target(ForCollects)").GetComponent<Transform>();
        collectCntrl = GameObject.Find("CollectCntrl").GetComponent<CollectsForDamageStation>();
        _collider = GetComponent<CircleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        transform.DOMoveY(0.5f, 2f).SetEase(Ease.InOutCirc).SetLoops(-1, loopType:LoopType.Yoyo).SetRelative(true);
        transform.DORotate(new Vector3(0, 0, 360), 5, RotateMode.FastBeyond360).SetEase(Ease.OutCubic).SetLoops(-1, loopType: LoopType.Yoyo).SetRelative(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out CharacterController2D player) && !touched)
        {
            touched = true;
            Touch();
            collectCntrl.collectionsCount++;
            collectCntrl.PlayCollectSound();
        }
    }

    private void Touch()
    {
        _collider.enabled = false;
        _spriteRenderer.enabled = false;
        explosionPartSys.Play();
        gameObject.transform.DOMove(flyTarget.position, 2).SetEase(Ease.InBack);
        glowSpriteRen.DOColor(new Color(glowSpriteRen.color.r, glowSpriteRen.color.g, glowSpriteRen.color.b, 0), 1);

        Destroy(gameObject, 2.5f);
    }
}
