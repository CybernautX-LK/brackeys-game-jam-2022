using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Events;

public class OutroSequenceController : SequenceController
{
    [SerializeField]
    private float sequenceDelay = 2.0f;

    [SerializeField]
    private Button playAgainButton;

    [SerializeField]
    private AnimationSettings playAgainButtonAnimationSettings;

    [SerializeField]
    private Image background;

    [SerializeField]
    private Image highscoreTitle;

    [SerializeField]
    private CanvasGroup cookieCounter;

    [SerializeField]
    private AnimationSettings cookieCounterAnimationSettings;

    [SerializeField]
    private Image cookie;

    [SerializeField]
    private AnimationSettings highscoreTitleAnimationSettings;

    [SerializeField]
    private TextMeshProUGUI counter;

    [SerializeField]
    private AnimationSettings counterAnimationSettings;

    Sequence sequence;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartSequence();
    }

    private void OnDisable()
    {
        if (sequence != null)
            sequence.Kill();
    }

    public override void StartSequence()
    {
        gameObject.SetActive(true);

        counter.text = "0";

        sequence = DOTween.Sequence();

        Tween cookieCounterTween = cookieCounter.transform.DOScale(1.0f, cookieCounterAnimationSettings.duration).SetEase(cookieCounterAnimationSettings.ease).From(0.0f);
        Tween playAgainButtonTween = playAgainButton.image.DOFade(1.0f, playAgainButtonAnimationSettings.duration).SetEase(playAgainButtonAnimationSettings.ease).From(0.0f);
        Tween counterTween = counter.DOCounter(0, GameManager.instance.currentPoints, counterAnimationSettings.duration).SetEase(counterAnimationSettings.ease);
        Tween highscoreTitleTween =  highscoreTitle.transform.DOScale(1.0f, highscoreTitleAnimationSettings.duration).SetEase(highscoreTitleAnimationSettings.ease).From(0.0f);

        sequence.AppendInterval(sequenceDelay).Append(highscoreTitleTween).Append(cookieCounterTween).Append(counterTween).Append(playAgainButtonTween).AppendCallback(() => CompleteSequence());

        OnSequenceStartEvent?.Invoke();
    }

    public override void CompleteSequence()
    {
        //gameObject.SetActive(false);

        OnSequenceCompleteEvent?.Invoke();
    }
}
