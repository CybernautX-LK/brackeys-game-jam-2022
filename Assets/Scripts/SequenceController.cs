using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class SequenceController: MonoBehaviour
{
    public abstract void StartSequence();
    public abstract void CompleteSequence();

    public UnityEvent OnSequenceStartEvent = new UnityEvent();
    public UnityEvent OnSequenceCompleteEvent = new UnityEvent();
}
