using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : ScriptableObject
{
    public float moveSpeed = 1.0f;

    public abstract void Initialize(EnemyController controller);

    public abstract void Think(EnemyController controller);
}
