using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Brackeys/Enemy/Chaser")]
public class Chaser : Enemy
{
    public string targetTag = "Player";
    public GameObject target;

    public override void Initialize(EnemyController controller)
    {
        if (target == null)
            target = GameObject.FindGameObjectWithTag(targetTag);
    }

    public override void Think(EnemyController controller)
    {
        if (target == null) return;

        if (!controller.isDetected)
            MoveTowardsTarget(controller);
    }

    private void MoveTowardsTarget(EnemyController controller)
    {
        Vector2 newPosition = Vector2.Lerp(controller.transform.position, target.transform.position, Time.deltaTime * moveSpeed);
        controller.rigidbody.MovePosition(newPosition);
    }
}
