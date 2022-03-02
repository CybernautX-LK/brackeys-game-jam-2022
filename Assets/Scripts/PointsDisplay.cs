using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class PointsDisplay : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private AnimationSettings iconAnimationSettings;

    [SerializeField]
    private TextMeshProUGUI pointsCounter;

    [SerializeField]
    private AnimationSettings pointsCounterAnimationSettings;

    private RectTransform rectTransform;



    private AllIn1ShaderAnimator iconShaderAnimator;

    private void Awake()
    {
        if (icon != null)
            iconShaderAnimator = icon.GetComponent<AllIn1ShaderAnimator>();

        rectTransform = GetComponent<RectTransform>();
    }

    public void SetPoints(int amount, bool animate = true)
    {
        float iconDuration = animate ? iconAnimationSettings.duration : 0.0f;
        float pointsDuration = animate ? pointsCounterAnimationSettings.duration : 0.0f;

        if (pointsCounter != null)
            pointsCounter.DOCounter(int.Parse(pointsCounter.text), amount, pointsDuration).SetEase(pointsCounterAnimationSettings.ease);

        if (icon != null)
        {
            icon.transform.DOScale(icon.transform.localScale + icon.transform.localScale * 0.1f, iconDuration).SetEase(iconAnimationSettings.ease).SetLoops(2, LoopType.Yoyo);
            icon.transform.DOShakePosition(iconDuration + 1).SetEase(iconAnimationSettings.ease);

            if (iconShaderAnimator != null && animate)
                iconShaderAnimator.PlayAnimation("Glow");
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        
    }
}
