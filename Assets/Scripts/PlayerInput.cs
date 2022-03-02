using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    

    [SerializeField]
    private KeyCode toggleFlashlight = KeyCode.Space;

    [SerializeField]
    private KeyCode run = KeyCode.LeftShift;

    private FlashlightController flashlight;
    private PlayerMovement playerMovement;


    private void Awake()
    {
        flashlight = FindObjectOfType<FlashlightController>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (flashlight != null && Input.GetKeyDown(toggleFlashlight))
            flashlight.ToggleLight();

        if (playerMovement != null)
        {
            if (Input.GetKeyDown(run))
                playerMovement.isRunning = true;

            if (Input.GetKeyUp(run))
                playerMovement.isRunning = false;
        }
    }
}
