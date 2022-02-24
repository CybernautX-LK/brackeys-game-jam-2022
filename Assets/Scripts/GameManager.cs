using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Gridbased;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int currentPoints;

    public static UnityAction<int> OnPlayerPointsUpdatedEvent;
    public static UnityAction<GameManager> OnGameStartEvent;
    public static UnityAction<GameManager> OnGameOverEvent;

    private BoardManager boardManager;
    //private PlayerHealth playerHealth;

    bool gameOver = false;

    private void Awake()
    {
        boardManager = FindObjectOfType<BoardManager>();
        //playerHealth = FindObjectOfType<PlayerHealth>();

        CollectableController.OnCollectedEvent += OnCollectableCollected;
        EnemyController.OnEnemyDeathEvent += OnEnemyDeath;
        PlayerHealth.OnPlayerDeathEvent += OnPlayerDeath;
    }



    private void OnDestroy()
    {
        CollectableController.OnCollectedEvent -= OnCollectableCollected;
        EnemyController.OnEnemyDeathEvent += OnEnemyDeath;
        PlayerHealth.OnPlayerDeathEvent -= OnPlayerDeath;
    }

    private void Start()
    {
        
    }

    public void Initialize()
    {
        boardManager.SetupScene();

        Debug.Log($"{typeof(GameManager).Name}: Level initialized!");
    }

    public void UpdatePlayerPoints(int amount)
    {
        currentPoints = amount;

        OnPlayerPointsUpdatedEvent?.Invoke(currentPoints);
    }

    public void StartGame()
    {
        Initialize();

        Debug.Log($"{typeof(GameManager).Name}: Start Game!");

        OnGameStartEvent?.Invoke(this);
    }

    public void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        AsyncOperation operation = SceneManager.LoadSceneAsync(currentScene.name, LoadSceneMode.Single);
    }

    private void GameOver()
    {
        if (gameOver) return;

        gameOver = true;

        Debug.Log($"{typeof(GameManager).Name}: Game Over!");

        OnGameOverEvent?.Invoke(this);
    }

    private void OnPlayerDeath()
    {
        GameOver();
    }

    private void OnCollectableCollected(Collectable collectable)
    {
        UpdatePlayerPoints(currentPoints + collectable.points);

        if (boardManager != null)
            boardManager.LayoutObjectAtRandom(boardManager.foodTiles, 1, 1);

    }

    private void OnEnemyDeath(EnemyController controller)
    {
        if (boardManager != null)
            boardManager.LayoutObjectAtRandom(boardManager.enemyTiles, 1, 1);
    }
}
