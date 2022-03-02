using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Events;
using System.Reflection;

public class FlashlightController : MonoBehaviour
{
    [BoxGroup("Settings")]
    [Range(0, 1)]
    public float flickerTreshold = 0.2f;

    [BoxGroup("Settings")]
    public float maxEnergy = 100.0f;

    [BoxGroup("Settings")]
    public float energyUsagePerSeconds = 10.0f;

    [BoxGroup("Settings")]
    public float energyReloadPerSecond = 20.0f;

    [BoxGroup("Settings")]
    [Range(1.0f, 10.0f)]
    [SerializeField]
    private float turnSpeed = 8.0f;

    [BoxGroup("Audio")]
    [SerializeField]
    private AudioObject[] OnFlashlightToggledSFX;
  
    [BoxGroup("References")]
    [SerializeField]
    private Light2D flashlight;

    [BoxGroup("References")]
    [SerializeField]
    private PolygonCollider2D polygonCollider2d;


    [BoxGroup("Debug")]
    [ProgressBar(0, 100)]
    public float currentEnergy;

    private bool outOfEnergy { get => currentEnergy <= 0.0f; }

    [ShowInInspector]
    public bool isActivated { get => flashlight != null && flashlight.gameObject.activeSelf; }

    private Camera cam;
    private SpriteMask spriteMask;
    private float flashlightIntensity;

    public UnityAction<float> OnEnergyUpdated;

    private void Awake()
    {
        spriteMask = GetComponentInChildren<SpriteMask>();

        if (flashlight != null)
            flashlightIntensity = flashlight.intensity;

        cam = Camera.main;
    }

    private void Start()
    {
        UpdateEnergy(maxEnergy);
    }

    public void ToggleLight()
    {
        if (!isActivated && outOfEnergy)
        {
            Debug.Log($"{typeof(FlashlightController).Name}: Fleshlight is out of energy!");
            return;
        }

        if (flashlight != null)
        {
            flashlight.intensity = flashlightIntensity;
            flashlight.gameObject.SetActive(!isActivated);
        }
            
        if (polygonCollider2d != null)
            polygonCollider2d.enabled = isActivated;

        if (OnFlashlightToggledSFX.Length > 0)
        {
            AudioObject audioObject = OnFlashlightToggledSFX[Random.Range(0, OnFlashlightToggledSFX.Length)];
            AudioManager2.Instance.PlayOneShot(audioObject);
        }
    }

    private void Update()
    {
        float energyRate = (isActivated) ? -energyUsagePerSeconds : energyReloadPerSecond;

        UpdateEnergy(currentEnergy + energyRate * Time.deltaTime);

        if (outOfEnergy)
            ToggleLight();

        if (!outOfEnergy && currentEnergy < maxEnergy * flickerTreshold)
            Flicker();

        RotateTowardsMousePosition();
    }

    private void UpdateEnergy(float amount)
    {
        float clampedAmount = Mathf.Clamp(amount, 0.0f, maxEnergy);

        currentEnergy = clampedAmount;

        OnEnergyUpdated?.Invoke(currentEnergy);
    }

    private void RotateTowardsMousePosition()
    {
        if (cam == null) return;

        Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, mousePosition - transform.position);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
    }

    private void Flicker()
    {
        int randomValue = Random.Range(0, 2);

        flashlight.intensity = (randomValue == 0) ? 0.0f : flashlightIntensity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDetectable detectable))
        {
            detectable.GetDetected(true);
            //Debug.Log("Detected " + collision.transform.name);
        };
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDetectable detectable))
        {
            detectable.GetDetected(false);
        };
    }
}
