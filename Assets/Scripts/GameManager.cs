using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Gridbased;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;
using Sirenix.OdinInspector;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Light2D globalLight;
    public Color brightLightColor;
    public float brightLightIntensity;
    public AnimationSettings brightLightAnimationSettings;

    [BoxGroup("Audio")]
    [SerializeField]
    private AudioObject mainMusic;

    [BoxGroup("Audio")]
    [SerializeField]
    private AudioObject OnGameStartedSFX;

    [BoxGroup("Audio")]
    [SerializeField]
    private AudioObject OnGameOverSFX;

    public CinemachineVirtualCamera virtualCamera;
    public int currentPoints;

    public static UnityAction<GameManager> OnStartGameRequestEvent;
    public static UnityAction<GameManager> OnGameInitializedEvent;
    public static UnityAction<int> OnPlayerPointsUpdatedEvent;
    public static UnityAction<GameManager> OnGameStartEvent;
    public static UnityAction<GameManager> OnGameOverStartEvent;
    public static UnityAction<GameManager> OnGameOverCompleteEvent;

    private PlayerController player;
    private LevelManager levelManager;
    //private PlayerHealth playerHealth;

    bool gameOver = false;

    private void Awake()
    {
        instance = this;

        levelManager = FindObjectOfType<LevelManager>();

        CollectableController.OnCollectedEvent += OnCollectableCollected;
        EnemyController.OnEnemyDeathEvent += OnEnemyDeath;
        PlayerHealth.OnPlayerDeathEvent += OnPlayerDeath;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("INTRO_HAS_BEEN_SEEN", 0);
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        CollectableController.OnCollectedEvent -= OnCollectableCollected;
        EnemyController.OnEnemyDeathEvent += OnEnemyDeath;
        PlayerHealth.OnPlayerDeathEvent -= OnPlayerDeath;
    }

    private void Start()
    {
        bool introHasBeenSeen = PlayerPrefs.GetInt("INTRO_HAS_BEEN_SEEN") == 1;

        if (introHasBeenSeen)
        {
            StartGame();
            return;
        }

        UIManager.instance.ShowMainMenu();
    }

    public void Initialize()
    {
        levelManager.Initialize();

        currentPoints = 0;
        player = FindObjectOfType<PlayerController>();

        if (player != null)
        {
            if (virtualCamera != null)
            {
                virtualCamera.Follow = player.transform;
                virtualCamera.LookAt = player.transform;
            }
        }

        Debug.Log($"{typeof(GameManager).Name}: Level initialized!");

        OnGameInitializedEvent?.Invoke(this);
    }

    public void UpdatePlayerPoints(int amount)
    {
        currentPoints = amount;

        OnPlayerPointsUpdatedEvent?.Invoke(currentPoints);
    }

    public void RequestStartGame()
    {        
        bool introHasBeenSeen = PlayerPrefs.GetInt("INTRO_HAS_BEEN_SEEN") == 1;

        if (introHasBeenSeen)
        {
            StartGame();
            return;
        }

        OnStartGameRequestEvent?.Invoke(this);

        PlayerPrefs.SetInt("INTRO_HAS_BEEN_SEEN", 1);
        PlayerPrefs.Save();       
    }

    public void StartGame()
    {
        Initialize();

        if (mainMusic != null)
            AudioManager2.Instance.Play(mainMusic);

        if (OnGameStartedSFX != null)
            AudioManager2.Instance.PlayOneShot(OnGameStartedSFX);

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

        StartCoroutine(GameOverCoroutine());       
    }

    private IEnumerator GameOverCoroutine()
    {
        OnGameOverStartEvent?.Invoke(this);

        if (OnGameOverSFX != null)
            AudioManager2.Instance.PlayOneShot(OnGameOverSFX);

        AudioManager2.Instance.Stop(AudioManager2.TrackType.MainMusic, 1.0f);

        if (globalLight != null)
        {
            float timer = 0.0f;

            while (timer < brightLightAnimationSettings.duration)
            {
                timer += Time.deltaTime;

                float percent = timer / brightLightAnimationSettings.duration;
                globalLight.color = Color.Lerp(globalLight.color, brightLightColor, percent);
                globalLight.intensity = Mathf.SmoothStep(globalLight.intensity, brightLightIntensity, percent);
                yield return null;
            }           
        }

        EnemyController[] enemies = FindObjectsOfType<EnemyController>();

        foreach (EnemyController enemy in enemies)
        {
            enemy.Die();
        }
        
        Debug.Log($"{typeof(GameManager).Name}: Game Over!");

        OnGameOverCompleteEvent?.Invoke(this);
    }

    private void OnPlayerDeath()
    {
        GameOver();
    }

    private void OnCollectableCollected(CollectableController controller)
    {
        if (gameOver) return;

        if (controller.collectable is Cookie)
        {
            UpdatePlayerPoints(currentPoints + controller.collectable.points);

            if (levelManager != null)
                levelManager.LayoutObjectAtRandom(levelManager.collectables, 1, 1);
        }
        else if (controller.collectable is Milk)
        {
            if (player != null)
                player.Shrink(2.0f);

            if (levelManager != null)
                levelManager.LayoutObjectAtRandom(levelManager.milk);
        }


    }

    private void OnEnemyDeath(EnemyController controller)
    {
        if (gameOver) return;

        if (levelManager != null)
            levelManager.LayoutObjectAtRandom(levelManager.enemies, 1, 1);
    }
}
