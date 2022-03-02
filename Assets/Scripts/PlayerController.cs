using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [BoxGroup("Settings")]
    [Range(0.1f, 1.0f)]
    [SerializeField]
    private float shrinkSize = 0.5f;

    [BoxGroup("Settings")]
    [Range(1.0f, 20.0f)]
    [SerializeField]
    private float shrinkDuration = 10.0f;

    [BoxGroup("Settings")]
    [SerializeField]
    private AnimationSettings shrinkAnimationSetting;

    [BoxGroup("Settings")]
    [SerializeField]
    private AnimationSettings growAnimationSettings;

    [BoxGroup("Audio")]
    [SerializeField]
    private AudioObject OnPlayerShrinkSFX;

    [BoxGroup("Audio")]
    [SerializeField]
    private AudioObject OnPlayerGrowSFX;

    [BoxGroup("References")]
    [SerializeField]
    private SpriteMask gameOverMask;

    private PlayerInput playerInput;
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;
    private Animator animator;
    private FlashlightController flashlightController;

    private Vector3 originalSize;
    public bool isShrinked { get; private set; }

    private void Awake()
    {
        GameManager.OnGameOverStartEvent += OnGameOver;
        CollectableController.OnCollectedEvent += OnCollectableCollected;

        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
        animator = GetComponentInChildren<Animator>();
        flashlightController = GetComponentInChildren<FlashlightController>();

        if (gameOverMask != null)
            gameOverMask.gameObject.SetActive(false);

        originalSize = transform.localScale;

        SetPlayerInput(true);
    }

    private void OnDestroy()
    {
        GameManager.OnGameOverStartEvent -= OnGameOver;
        CollectableController.OnCollectedEvent -= OnCollectableCollected;
    }

    public void SetPlayerInput(bool status)
    {
        if (playerMovement != null)
            playerMovement.enabled = status;

        if (playerInput != null)
            playerInput.enabled = status;
    }

    private void OnCollectableCollected(CollectableController controller)
    {
        if (animator != null)
            animator.SetTrigger("OnCollectableCollected");
    }

    private void OnGameOver(GameManager gameManager)
    {
        if (animator != null)
            animator.SetBool("GameOver", true);

        if (flashlightController != null && flashlightController.isActivated)
            flashlightController.ToggleLight();

        if (gameOverMask != null)
            gameOverMask.gameObject.SetActive(true);

        SetPlayerInput(false);
    }

    public void Shrink(float delay = 0.0f)
    {
        if (OnPlayerShrinkSFX != null)
            AudioManager2.Instance.PlayOneShot(OnPlayerShrinkSFX);

        if (DOTween.IsTweening(transform))
            DOTween.Kill(transform);

        if (playerMovement != null)
            playerMovement.SetSpeedMode(PlayerMovement.SpeedMode.Fast);

        transform.DOScale(shrinkSize, shrinkAnimationSetting.duration).SetEase(shrinkAnimationSetting.ease).SetDelay(delay).OnStart(() => isShrinked = true); ;
        transform.DOScale(originalSize, growAnimationSettings.duration).SetEase(growAnimationSettings.ease).SetDelay(delay + shrinkDuration).OnStart(() =>
        {
            if (OnPlayerGrowSFX != null)
                AudioManager2.Instance.PlayOneShot(OnPlayerGrowSFX);

            if (playerMovement != null)
                playerMovement.SetSpeedMode(PlayerMovement.SpeedMode.Normal);

            isShrinked = false;
        });

}
}
