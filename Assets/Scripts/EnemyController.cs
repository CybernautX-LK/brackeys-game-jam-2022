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

    private SpriteRenderer spriteRenderer;
    public new Rigidbody2D rigidbody { get; private set; }

    public float lifeTime = 0.0f;
    public float currentLifeTime = 0.0f;

    public bool isDead = false;
    public bool isDetected { get; private set; }

    public static UnityAction<EnemyController> OnEnemyDeathEvent;

    private void Awake()
    {
        if (enemy == null)
        {
            this.enabled = false;
            return;
        }

        lifeTime = Random.Range(enemy.minLifeTime, enemy.maxLifeTime);

        enemy.Initialize(this);

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();

        spriteRenderer.color = undetectedColor;
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
        //spriteRenderer.color = (status) ? detectedColor : undetectedColor;
        isDetected = status;
    }
}
