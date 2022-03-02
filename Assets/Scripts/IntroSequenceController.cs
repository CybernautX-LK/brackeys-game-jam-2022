using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSequenceController : SequenceController
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public override void StartSequence()
    {
        gameObject.SetActive(true);

        OnSequenceStartEvent?.Invoke();
    }

    public override void CompleteSequence()
    {
        gameObject.SetActive(false);

        OnSequenceCompleteEvent?.Invoke();
    }

}
