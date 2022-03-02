using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

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
        bool allowedToMove = !controller.isDetected && !considerTargetMovement;


        if (allowedToMove)
        {
            MoveTowardsTarget(controller);
            FaceTarget(controller);

            //controller.animator.SetBool("isDetected", false);
        }

        if (controller.animator != null)
            controller.animator.SetBool("isMoving", allowedToMove);

        if (controller.walkAudioSource != null && controller.walkAudioSource.isPlaying && !allowedToMove)
            controller.walkAudioSource.Stop();
        else if (controller.walkAudioSource != null && !controller.walkAudioSource.isPlaying && allowedToMove)
            controller.walkAudioSource.Play();
    }



    private void MoveTowardsTarget(EnemyController controller)
    {
        if (target == null) return;

        Vector2 newPosition = Vector2.Lerp(controller.transform.position, target.transform.position, Time.deltaTime * moveSpeed);
        controller.rigidbody.MovePosition(newPosition);

        controller.currentLifeTime += Time.deltaTime;
    }

    private void FaceTarget(EnemyController controller)
    {
        if (target == null) return;

        if (target.transform.position.x > controller.transform.position.x && !controller.facingRight)
        {
            Flip(controller);
        }
        else if (target.transform.position.x < controller.transform.position.x && controller.facingRight)
        {
            Flip(controller);
        }
    }

    private void Flip(EnemyController controller)
    {
        // Switch the way the player is labelled as facing.
        controller.facingRight = !controller.facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = controller.transform.localScale;
        theScale.x *= -1;
        controller.transform.localScale = theScale;
    }

    public override void Die(EnemyController controller)
    {
        controller.transform.DOScale(0.0f, dieAnimationSettings.duration).SetEase(dieAnimationSettings.ease).OnComplete(() => Destroy(controller.gameObject));
    }
}
