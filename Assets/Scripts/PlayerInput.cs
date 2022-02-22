using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    

    [SerializeField]
    private KeyCode toggleFlashlight = KeyCode.Space;

    private FlashlightController flashlight;


    private void Awake()
    {
        flashlight = FindObjectOfType<FlashlightController>();
    }

    private void Update()
    {
        if (flashlight != null && Input.GetKeyDown(toggleFlashlight))
            flashlight.ToggleLight();
    }
}
