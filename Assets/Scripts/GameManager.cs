using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public int currentPoints;

    public static UnityAction<int> OnPlayerPointsUpdatedEvent;

    private void Awake()
    {
        CollectableController.OnCollectedEvent += OnCollectableCollected;
    }

    private void OnDestroy()
    {
        CollectableController.OnCollectedEvent -= OnCollectableCollected;
    }

    public void UpdatePlayerPoints(int amount)
    {
        currentPoints = amount;

        OnPlayerPointsUpdatedEvent?.Invoke(currentPoints);
    }

    private void OnCollectableCollected(Collectable collectable)
    {
        UpdatePlayerPoints(currentPoints + collectable.points);
    }
}
