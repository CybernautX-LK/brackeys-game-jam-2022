using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectableController : MonoBehaviour
{
    public Collectable collectable;

    private SpriteRenderer spriteRenderer;

    public static UnityAction<Collectable> OnCollectedEvent;

    private void Awake()
    {
        if (collectable == null)
        {
            this.enabled = false;
            return;
        }

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void GetCollected()
    {        
        OnCollectedEvent?.Invoke(collectable);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            GetCollected();
    }
}
