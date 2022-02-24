using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Brackeys/Enemy/Chaser")]
public class Chaser : Enemy
{
    
    public string targetTag = "Player";
    public GameObject target;
    public bool onlyMoveOnTargetMovement = false;

    public AnimationSettings awakeAnimationSettings;
    public AnimationSettings dieAnimationSettings;

    private PlayerMovement playerMovement; 

    public override void Initialize(EnemyController controller)
    {
        if (target == null)
            target = GameObject.FindGameObjectWithTag(targetTag);

        playerMovement = target.GetComponent<PlayerMovement>();

        controller.transform.DOScale(controller.transform.localScale, awakeAnimationSettings.duration).From(0.0f).SetEase(awakeAnimationSettings.ease);
    }

    public override void Think(EnemyController controller)
    {
        if (target == null || playerMovement == null) return;

        bool considerTargetMovement = onlyMoveOnTargetMovement && !playerMovement.playerInput;

        if (!controller.isDetected && !considerTargetMovement)
            MoveTowardsTarget(controller);
    }

    private void MoveTowardsTarget(EnemyController controller)
    {
        Vector2 newPosition = Vector2.Lerp(controller.transform.position, target.transform.position, Time.deltaTime * moveSpeed);
        controller.rigidbody.MovePosition(newPosition);

        controller.currentLifeTime += Time.deltaTime;
    }

    public override void Die(EnemyController controller)
    {
        controller.transform.DOScale(0.0f, dieAnimationSettings.duration).SetEase(dieAnimationSettings.ease).OnComplete(() => Destroy(controller.gameObject));     
    }
}
