using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDetectable
{
    public Enemy enemy;

    [SerializeField]
    private Color detectedColor = Color.green;

    [SerializeField]
    private Color undetectedColor = Color.grey;

    private SpriteRenderer spriteRenderer;
    public new Rigidbody2D rigidbody { get; private set; }

    public bool isDetected { get; private set; }

    private void Awake()
    {
        if (enemy == null)
        {
            this.enabled = false;
            return;
        }

        enemy.Initialize(this);

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();

        spriteRenderer.color = undetectedColor;
    }

    private void Update()
    {
        enemy.Think(this);
    }

    public void GetDetected(bool status)
    {
        //spriteRenderer.color = (status) ? detectedColor : undetectedColor;
        isDetected = status;
    }
}
