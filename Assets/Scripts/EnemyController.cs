using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class EnemyController : MonoBehaviour, IDetectable
{
    public Enemy enemy;

    [SerializeField]
    private Color detectedColor = Color.green;

    [SerializeField]
    private Color undetectedColor = Color.grey;

    [SerializeField]
    public AudioSource walkAudioSource;

    public SpriteRenderer furnitureRenderer;

    public SpriteRenderer monsterRenderer;
    public new Rigidbody2D rigidbody { get; private set; }
    public Animator animator { get; private set; }

    public float lifeTime = 0.0f;
    public float currentLifeTime = 0.0f;

    public bool isDead = false;
    public bool isDetected;

    public bool facingRight;

    public static UnityAction<EnemyController> OnEnemyDeathEvent;

    

    private void Awake()
    {
        GameManager.OnGameOverStartEvent += OnGameOver;

        if (enemy == null)
        {
            this.enabled = false;
            return;
        }

        lifeTime = Random.Range(enemy.minLifeTime, enemy.maxLifeTime);

        enemy.Initialize(this);

        //furnitureRenderer.enabled = false;
        //monsterRenderer.enabled = true;

        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    private void OnGameOver(GameManager gameManager)
    {
        if (walkAudioSource != null)
            walkAudioSource.enabled = false;
    }

    private void OnDestroy()
    {
        GameManager.OnGameOverStartEvent -= OnGameOver;
    }

    private void Update()
    {
        if (isDead) return;

        enemy.Think(this);

        if (currentLifeTime >= lifeTime)
            Die();
    }

    [Button]
    public void Die()
    {
        isDead = true;
        enemy.Die(this);

        OnEnemyDeathEvent?.Invoke(this);
    }

    public void GetDetected(bool status)
    {
        //furnitureRenderer.enabled = status;
        //monsterRenderer.enabled = !status;
        isDetected = status;
    }
}
