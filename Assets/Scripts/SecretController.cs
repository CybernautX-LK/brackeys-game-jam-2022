using UnityEngine;
using DG.Tweening;
using System;
using System.Collections.Generic;

public class SecretController : MonoBehaviour
{
    [SerializeField]
    private Transform secretRoomSpawnPoint;

    [SerializeField]
    private Transform levelSpawnPoint;

    [SerializeField]
    private AnimationSettings travelAnimationSettings;

    [SerializeField]
    private List<CollectableController> collectables = new List<CollectableController>();

    private PlayerController player;
    private int collectableCount;

    private bool isActive = false;
    private bool isComplete = false;

    private void Subscribe()
    {
        CollectableController.OnCollectedEvent += OnCollectableCollected;
    }

    private void Unsubscribe()
    {
        CollectableController.OnCollectedEvent -= OnCollectableCollected;
    }

    public void StartSecret()
    {
        if (isActive) return;

        isActive = true;

        Subscribe();

        collectableCount = collectables.Count;


        Travel(secretRoomSpawnPoint.position);


        

        Debug.Log("Start Secret");
    }

    private void CompleteSecret()
    {
        if (!isActive) return;

        isActive = false;

        Unsubscribe();

        Travel(levelSpawnPoint.position);

        Debug.Log("Complete Secret");
    }

    private void Travel(Vector3 destination)
    {
        Sequence sequence = DOTween.Sequence();

        Tween shrinkTween = player.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
        Tween moveTween = player.transform.DOMove(destination, travelAnimationSettings.duration).SetEase(travelAnimationSettings.ease);
        Tween growTween = player.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InBack);

        sequence.Append(shrinkTween);
        sequence.Append(moveTween);
        sequence.Append(growTween);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (collision.transform.TryGetComponent(out PlayerController player))
            {
                this.player = player;

                if (!isComplete && !isActive && player.isShrinked)
                    StartSecret();
                else if(isComplete && isActive)
                    CompleteSecret();
            }
        }


    }

    private void OnCollectableCollected(CollectableController controller)
    {
        collectableCount--;

        if (collectableCount <= 0)
            isComplete = true;
    }


}
