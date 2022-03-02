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
    public static UIManager instance;

    [BoxGroup("Splash")]
    [SerializeField]
    private bool showSplash = true;

    [BoxGroup("Splash")]
    [SerializeField]
    private GameObject splash;

    [BoxGroup("Sequences")]
    [SerializeField]
    private SequenceController introSequence;

    [BoxGroup("Sequences")]
    [SerializeField]
    private SequenceController outroSequence;

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
    private PointsDisplay pointsDisplay;

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
        instance = this;

        GameManager.OnGameInitializedEvent += OnGameInitialized;
        GameManager.OnPlayerPointsUpdatedEvent += OnPlayerPointsUpdated;
        GameManager.OnGameStartEvent += OnGameStart;
        GameManager.OnStartGameRequestEvent += OnStartGameRequest;
        GameManager.OnGameOverCompleteEvent += OnGameOver;

        if (messageDisplay != null)
            messageDisplay.text = "";

    }



    private void OnDestroy()
    {
        GameManager.OnGameInitializedEvent -= OnGameInitialized;
        GameManager.OnPlayerPointsUpdatedEvent -= OnPlayerPointsUpdated;
        GameManager.OnGameStartEvent -= OnGameStart;
        GameManager.OnStartGameRequestEvent -= OnStartGameRequest;
        GameManager.OnGameOverCompleteEvent -= OnGameOver;

        if (flashlight != null)
            flashlight.OnEnergyUpdated -= UpdateFlashlightSlider;
    }

    public void ShowMainMenu()
    {
        if (mainMenuController != null)
            mainMenuController.OpenMenuSingle("MainMenu");
    }

    private void OnStartGameRequest(GameManager gameManager)
    {
        if (introSequence != null)
            introSequence.StartSequence();
    }

    private void OnGameInitialized(GameManager gameManager)
    {
        flashlight = FindObjectOfType<FlashlightController>();

        if (flashlight != null && flashlightSlider != null)
        {
            flashlight.OnEnergyUpdated += UpdateFlashlightSlider;


            flashlightSlider.maxValue = flashlight.maxEnergy;
            flashlightSlider.minValue = 0.0f;

            if (flashlightSlider.fillRect != null)
                flashLightSliderFillImage = flashlightSlider.fillRect.GetComponent<Image>();
        }

        if (pointsDisplay != null)
        {
            pointsDisplay.SetPoints(gameManager.currentPoints, false);
        }
    }

    private void Start()
    {
        if (flashlight != null && flashlightSlider != null)
        {
            flashlightSlider.maxValue = flashlight.maxEnergy;
        }

        if (splash != null)
            splash.SetActive(showSplash && PlayerPrefs.GetInt("INTRO_HAS_BEEN_SEEN") == 0);
    }

    private void UpdateFlashlightSlider(float value)
    {
        if (flashlightSlider == null) return;

        float sliderValuePercent = Time.deltaTime * 1 / sliderAnimationSettings.duration;
        flashlightSlider.value = Mathf.Lerp(flashlightSlider.value, value, sliderValuePercent);
        //flashlightSlider.DOValue(value, sliderAnimationSettings.duration).SetEase(sliderAnimationSettings.ease);

        if (flashLightSliderFillImage != null)
        {
            flashLightSliderFillImage.color = Color.Lerp(emptyColor, fullColor, flashlight.currentEnergy / flashlight.maxEnergy);
            //flashLightSliderFillImage.DOColor(Color.Lerp(emptyColor, fullColor, flashlight.currentEnergy / flashlight.maxEnergy), sliderAnimationSettings.duration).SetEase(sliderAnimationSettings.ease);
        }

    }

    // Await collectable animation
    private void OnPlayerPointsUpdated(int amount)
    {
        CancelInvoke();
        Invoke("UpdatePointsCounter", 1.0f);
    }

    private void UpdatePointsCounter()
    {
        if (pointsDisplay == null) return;

        pointsDisplay.SetPoints(GameManager.instance.currentPoints);
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

        if (PlayerPrefs.GetInt("INTRO_HAS_BEEN_SEEN") != 1)
            ShowMessage("Click to toggle flashlight", 4.0f);
    }

    private void OnGameOver(GameManager gameManager)
    {
        //if (mainMenuController != null)
        //    mainMenuController.OpenMenuSingle("GameOverMenu");
        //
        //if (endScoreDisplay != null)
        //    endScoreDisplay.text = $"You collected {gameManager.currentPoints} cookies";

        ShowMessage("");

        if (outroSequence != null)
            outroSequence.StartSequence();
    }
}
