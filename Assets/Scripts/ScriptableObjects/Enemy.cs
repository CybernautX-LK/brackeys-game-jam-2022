using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Enemy : ScriptableObject
{
    public float minLifeTime = 10.0f;
    public float maxLifeTime = 15.0f;
    public float moveSpeed = 1.0f;

    public abstract void Initialize(EnemyController controller);

    public abstract void Think(EnemyController controller);

    public abstract void Die(EnemyController controller);
}
