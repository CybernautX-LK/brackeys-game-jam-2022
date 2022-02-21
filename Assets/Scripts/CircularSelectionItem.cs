using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;


public class CircularSelectionItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public enum GraphicType { Background, Icon }

    [Range(25, 200)]
    public float sizePercentage = 100.0f;

    public Color neutralColor = Color.white;
    public Color highlightColor = Color.cyan;
    public Image backgroundImage;
    public Image iconImage;
    public Sprite backgroundSprite;
    public Sprite iconSprite;
    public DOTweenAnimation onPointerEnterAnimation;
    public DOTweenAnimation onPointerExitAnimation;

    private void Start() => Initialize();


    public void Initialize()
    {
        UpdateGraphic(GraphicType.Background, neutralColor, backgroundSprite);
        UpdateGraphic(GraphicType.Icon, neutralColor, iconSprite);
    }

    public void UpdateGraphic(GraphicType type, Color color, Sprite sprite)
    {
        switch (type)
        {
            case GraphicType.Background:

                if (backgroundImage == null) return;
                backgroundImage.color = color;
                backgroundImage.sprite = sprite;

                break;
            case GraphicType.Icon:
                
                if (iconImage == null) return;
                iconImage.color = color;
                iconImage.sprite = sprite;

                break;
            default:
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (DOTween.IsTweening("OnPointerEnter", true)) return;

        if (DOTween.IsTweening(gameObject))
            DOTween.Complete(gameObject);

        DOTween.Restart(gameObject, "OnPointerEnter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (DOTween.IsTweening("OnPointerExit", true)) return;

        if (DOTween.IsTweening(gameObject))
            DOTween.Complete(gameObject);

        DOTween.Restart(gameObject, "OnPointerExit");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (DOTween.IsTweening("OnPointerDown", true)) return;

        if (DOTween.IsTweening(gameObject))
            DOTween.Complete(gameObject);

        DOTween.Restart(gameObject, "OnPointerDown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (DOTween.IsTweening("OnPointerUp", true)) return;

        if (DOTween.IsTweening(gameObject))
            DOTween.Complete(gameObject);

        DOTween.Restart(gameObject, "OnPointerUp");
    }
}
