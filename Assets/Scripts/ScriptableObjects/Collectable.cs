using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class Collectable : ScriptableObject
{
    public int points = 10;
    public float pickUpDistance = 1.25f;
    public float destroyAfterSeconds = 3.0f;
    public AudioObject[] OnPickedUpSFX;
    public AudioObject[] OnCollectedSFX;

    public virtual void OnPickedUp(CollectableController controller) 
    {
        if (OnPickedUpSFX.Length > 0)
        {
            AudioObject audioObject = OnPickedUpSFX[Random.Range(0, OnPickedUpSFX.Length)];
            AudioManager2.Instance.PlayOneShot(audioObject);
        }

        if (DOTween.IsTweening(controller.transform))
            DOTween.Kill(controller.transform);

        controller.transform.DOLocalMove(-Vector3.right * pickUpDistance / controller.pickedUpBy.localScale.y * controller.pickedUpBy.localScale.y, controller.onCollectedAnimationSettings.duration).SetEase(controller.onCollectedAnimationSettings.ease).OnComplete(() =>
        {
            if (controller.animator != null)
                controller.animator.Play("Collect");

            if (OnCollectedSFX.Length > 0)
            {
                AudioObject audioObject = OnCollectedSFX[Random.Range(0, OnCollectedSFX.Length)];
                AudioManager2.Instance.PlayOneShot(audioObject);
            }
        });
    }

    public abstract void OnCollectedComplete(CollectableController controller);
}
