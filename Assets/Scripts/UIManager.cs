using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;
using System;

public class UIManager : MonoBehaviour
{
    [BoxGroup("Flashlight")]
    [SerializeField]
    private FlashlightController flashlight;

    [BoxGroup("Flashlight")]
    [SerializeField]
    private Slider flashlightSlider;

    [BoxGroup("Flashlight")]
    [SerializeField]
    private Color fullColor = Color.green;

    [BoxGroup("Flashlight")]
    [SerializeField]
    private Color emptyColor = Color.red;

    [BoxGroup("Flashlight")]
    [SerializeField]
    private AnimationSettings sliderAnimationSettings;

    private Image flashLightSliderFillImage;

    private void Awake()
    {
        if (flashlight != null && flashlightSlider != null)
        {
            flashlight.OnEnergyUpdated += UpdateFlashlightSlider;

            if (flashlightSlider.fillRect != null)
                flashLightSliderFillImage = flashlightSlider.fillRect.GetComponent<Image>();
        }

    }

    private void Start()
    {
        if (flashlight != null && flashlightSlider != null)
        {
            flashlightSlider.maxValue = flashlight.maxEnergy;
        }
    }

    private void OnDestroy()
    {
        if (flashlight != null)
            flashlight.OnEnergyUpdated -= UpdateFlashlightSlider;
    }

    private void UpdateFlashlightSlider(float value)
    {
        if (flashlightSlider == null) return;

        flashlightSlider.DOValue(value, sliderAnimationSettings.duration).SetEase(sliderAnimationSettings.ease);

        if (flashLightSliderFillImage != null)
            flashLightSliderFillImage.DOColor(Color.Lerp(emptyColor, fullColor, flashlight.currentEnergy / flashlight.maxEnergy), sliderAnimationSettings.duration).SetEase(sliderAnimationSettings.ease);
    }
}
