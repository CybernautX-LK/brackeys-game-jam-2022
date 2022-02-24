using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Events;
public class FlashlightController : MonoBehaviour
{


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

    [BoxGroup("References")]
    [SerializeField]
    private Camera cam;

    [BoxGroup("References")]
    [SerializeField]
    private Light2D fleshlight;

    [BoxGroup("References")]
    [SerializeField]
    private PolygonCollider2D polygonCollider2d;


    [BoxGroup("Debug")]
    [ProgressBar(0, 100)]
    public float currentEnergy;

    private bool outOfEnergy { get => currentEnergy <= 0.0f; }

    [ShowInInspector]
    public bool isActivated { get => fleshlight != null && fleshlight.enabled; }

    private SpriteMask spriteMask;

    public UnityAction<float> OnEnergyUpdated;

    private void Awake()
    {
        spriteMask = GetComponentInChildren<SpriteMask>();
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

        if (fleshlight != null)
            fleshlight.enabled = !isActivated;

        if (polygonCollider2d != null)
            polygonCollider2d.enabled = fleshlight.enabled;

        if (spriteMask != null)
            spriteMask.enabled = fleshlight.enabled;
    }

    private void Update()
    {
        float energyRate = (isActivated) ? -energyUsagePerSeconds : energyReloadPerSecond;
        UpdateEnergy(currentEnergy + energyRate * Time.deltaTime);

        RotateTowardsMousePosition();
    }

    private void UpdateEnergy(float amount)
    {
        float clampedAmount = Mathf.Clamp(amount, 0.0f, maxEnergy);

        currentEnergy = clampedAmount;

        if (outOfEnergy)
            ToggleLight();

        OnEnergyUpdated?.Invoke(currentEnergy);
    }

    private void RotateTowardsMousePosition()
    {
        if (cam == null) return;

        Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, mousePosition - transform.position);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDetectable detectable))
        {
            detectable.GetDetected(true);
            Debug.Log("Detected " + collision.transform.name);
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
