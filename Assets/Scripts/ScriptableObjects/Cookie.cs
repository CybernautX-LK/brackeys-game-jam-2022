using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Brackeys/Collectable/Cookie")]
public class Cookie : Collectable
{
    public override void OnCollectedComplete(CollectableController controller)
    {
        Destroy(controller.gameObject);
    }

    public override void OnPickedUp(CollectableController controller)
    {
        base.OnPickedUp(controller);

        if (controller.shaderAnimator != null)
            controller.shaderAnimator.PlayAnimation("Glow");
    }
}
