using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class CollectableController : MonoBehaviour
{
    public Collectable collectable;

    public AnimationSettings idleAnimationSettings;
    public AnimationSettings onCollectedAnimationSettings;

    public SpriteRenderer spriteRenderer { get; private set; }
    public Animator animator { get; private set; }

    private bool isCollected = false;

    public Transform pickedUpBy { get; private set; }

    public AllIn1ShaderAnimator shaderAnimator { get; private set; }

    public static UnityAction<CollectableController> OnCollectedEvent;

    private void Awake()
    {
        if (collectable == null)
        {
            this.enabled = false;
            return;
        }

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
        shaderAnimator = GetComponent<AllIn1ShaderAnimator>();
    }

    private void Start()
    {
        Idle();
    }

    public void Idle()
    {
        transform.DOMoveY(transform.position.y + 0.1f, idleAnimationSettings.duration).SetLoops(-1, LoopType.Yoyo).SetEase(idleAnimationSettings.ease);
    }

    public void GetCollected()
    {
        transform.parent = pickedUpBy;

        collectable.OnPickedUp(this);       
        
        isCollected = true;
        OnCollectedEvent?.Invoke(this);

        float timer = 0;
        DOTween.To(() => timer, x => timer = x, 1, collectable.destroyAfterSeconds).OnComplete(() => collectable.OnCollectedComplete(this));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCollected && collision.CompareTag("Player"))
        {
            pickedUpBy = collision.transform;
            GetCollected();
        }
            
    }
}
