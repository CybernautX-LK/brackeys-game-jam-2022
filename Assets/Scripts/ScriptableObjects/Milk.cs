using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Brackeys/Collectable/Milk")]
public class Milk : Collectable
{
    public override void OnPickedUp(CollectableController controller)
    {
        base.OnPickedUp(controller);

        controller.transform.DORotate(controller.transform.rotation.eulerAngles + controller.transform.forward * 180, 1.0f);

        if (controller.shaderAnimator != null)
            controller.shaderAnimator.PlayAnimation("Glow");
    }

    public override void OnCollectedComplete(CollectableController controller)
    {
        controller.transform.DOScale(Vector3.zero, 1.0f).OnComplete(() => Destroy(controller.gameObject));
    }
}
