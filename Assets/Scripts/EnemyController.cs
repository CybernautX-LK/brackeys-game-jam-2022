using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDetectable
{
    [SerializeField]
    private Color detectedColor = Color.green;

    [SerializeField]
    private Color undetectedColor = Color.grey;

    public SpriteRenderer spriteRenderer;


    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        spriteRenderer.color = undetectedColor;
    }

    public void GetDetected(bool status)
    {
        spriteRenderer.color = (status) ? detectedColor : undetectedColor;
    }
}
