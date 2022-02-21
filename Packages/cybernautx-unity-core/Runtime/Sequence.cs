using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public abstract class Sequence : MonoBehaviour
{
    [System.Serializable]
    public class SequencePartEvent : UnityEvent<SequencePart> { };

    public enum SequencePart { Part1, Part2, Part3, Part4, Part5, Part6, Part7, Part8, Part9 }

    [BoxGroup("Settings")]
    [ShowInInspector]
    [ReadOnly]
    public bool isRunning { get; protected set; }

    [BoxGroup("Settings")]
    [ShowInInspector]
    [ReadOnly]
    public bool isComplete = false;

    [BoxGroup("Settings")]
    [ShowInInspector]
    [ReadOnly]
    public bool initialized = false;

    [ShowInInspector]
    [BoxGroup("Settings")]
    public SequencePart currentPart;

    

    [BoxGroup("Events")]
    public UnityEvent OnSequenceStart = new UnityEvent();

    [BoxGroup("Events")]
    public SequencePartEvent OnSequencePartStart = new SequencePartEvent();

    [BoxGroup("Events")]
    public SequencePartEvent OnSequencePartComplete = new SequencePartEvent();

    [BoxGroup("Events", Order = 10)]
    public UnityEvent OnSequenceComplete = new UnityEvent();

    [BoxGroup("Events")]
    public UnityEvent OnSequenceStop = new UnityEvent();

    

    protected Coroutine currentCoroutine = null;

    public virtual void Init()
    {
        if (initialized) return;
        initialized = true;
    }

    public virtual void Reset()
    {
        if (!initialized) return;
        initialized = false;
    }

    [Button]
    public virtual void StartSequence(SequencePart part)
    {
        isComplete = false;

        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        Init();

        currentCoroutine = StartCoroutine(PlaySequencePart(part));

        Debug.Log("Sequence started at " + part);
    }

    [Button]
    public virtual void RestartSequence()
    {
        StartSequence(SequencePart.Part1);

        Debug.Log("Sequence restarted.");
    }

    [Button]
    public virtual void ContinueSequence()
    {
        if (!isComplete)
            StartSequence(currentPart);

        Debug.Log("Sequence continued with " + currentPart);
    }

    [Button]
    public virtual void StopSequence()
    {
        isRunning = false;

        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        Reset();

        OnSequenceStop?.Invoke();

        Debug.Log("Sequence stopped.");
    }

    protected abstract IEnumerator PlaySequencePart(SequencePart part);

}
