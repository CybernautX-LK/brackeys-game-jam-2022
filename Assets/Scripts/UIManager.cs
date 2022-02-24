using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;
using System;
using TMPro;
using CybernautX;

public class UIManager : MonoBehaviour
{
    [BoxGroup("Menus")]
    [SerializeField]
    private MainMenuController mainMenuController;

    [BoxGroup("Game Messages")]
    [SerializeField]
    private TextMeshProUGUI messageDisplay;

    [BoxGroup("Game Messages")]
    [SerializeField]
    private AnimationSettings messageDisplayAnimationSettings;

    [BoxGroup("Point Counter")]
    [SerializeField]
    private TextMeshProUGUI pointCounter;

    [BoxGroup("Point Counter")]
    [SerializeField]
    private AnimationSettings pointCounterAnimationSettings;

    [BoxGroup("Point Counter")]
    [SerializeField]
    private TextMeshProUGUI endScoreDisplay;

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
        GameManager.OnPlayerPointsUpdatedEvent += UpdatePointsCounter;
        GameManager.OnGameStartEvent += OnGameStart;
        GameManager.OnGameOverEvent += OnGameOver;

        if (flashlight != null && flashlightSlider != null)
        {
            flashlight.OnEnergyUpdated += UpdateFlashlightSlider;

            if (flashlightSlider.fillRect != null)
                flashLightSliderFillImage = flashlightSlider.fillRect.GetComponent<Image>();
        }

        if (messageDisplay != null)
            messageDisplay.text = "";

    }



    private void OnDestroy()
    {
        GameManager.OnPlayerPointsUpdatedEvent -= UpdatePointsCounter;
        GameManager.OnGameStartEvent += OnGameStart;
        GameManager.OnGameOverEvent -= OnGameOver;

        if (flashlight != null)
            flashlight.OnEnergyUpdated -= UpdateFlashlightSlider;
    }



    private void Start()
    {
        if (flashlight != null && flashlightSlider != null)
        {
            flashlightSlider.maxValue = flashlight.maxEnergy;
        }
    }

    private void UpdateFlashlightSlider(float value)
    {
        if (flashlightSlider == null) return;

        flashlightSlider.value = Mathf.Lerp(flashlightSlider.value, value, Time.deltaTime * 1 / sliderAnimationSettings.duration);
        //flashlightSlider.DOValue(value, sliderAnimationSettings.duration).SetEase(sliderAnimationSettings.ease);

        if (flashLightSliderFillImage != null)
        {
            flashLightSliderFillImage.color = Color.Lerp(emptyColor, fullColor, flashlight.currentEnergy / flashlight.maxEnergy);
            //flashLightSliderFillImage.DOColor(Color.Lerp(emptyColor, fullColor, flashlight.currentEnergy / flashlight.maxEnergy), sliderAnimationSettings.duration).SetEase(sliderAnimationSettings.ease);
        }
         
    }

    private void UpdatePointsCounter(int amount)
    {
        if (pointCounter == null) return;

        pointCounter.DOCounter(int.Parse(pointCounter.text), amount, pointCounterAnimationSettings.duration).SetEase(pointCounterAnimationSettings.ease);
    }

    public void ShowMessage(string message, float seconds = 2.0f)
    {
        if (messageDisplay == null) return;

        messageDisplay.text = message;

        if (DOTween.IsTweening(messageDisplay))
            DOTween.Kill(messageDisplay);

        Tween enterTween = messageDisplay.DOFade(1.0f, messageDisplayAnimationSettings.duration).SetEase(messageDisplayAnimationSettings.ease).From(0.0f);
        Tween exitTween = messageDisplay.DOFade(0.0f, messageDisplayAnimationSettings.duration).SetEase(messageDisplayAnimationSettings.ease).SetDelay(messageDisplayAnimationSettings.duration + seconds);
    }

    private void OnGameStart(GameManager gameManager)
    {
        if (mainMenuController != null)
            mainMenuController.CloseAllMenus();
    }

    private void OnGameOver(GameManager gameManager)
    {
        if (mainMenuController != null)
            mainMenuController.OpenMenuSingle("GameOverMenu");

        if (endScoreDisplay != null)
            endScoreDisplay.text = $"You collected {gameManager.currentPoints} cookies";
    }
}
