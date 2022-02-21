using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using DG.Tweening;

public class CircularSelectionController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private float distanceFromCenter = 0.5f;
    
    [SerializeField]
    private float duration = 0.5f;

    [SerializeField]
    private List<CircularSelectionItem> selectionItems = new List<CircularSelectionItem>();

    private List<CircularSelectionItem> instantiatedItems = new List<CircularSelectionItem>();

    private Dictionary<CircularSelectionItem, Vector3> itemPositionDictionary = new Dictionary<CircularSelectionItem, Vector3>();

    public bool isActive = false;


    public void Start() => Initialize();

    [Button]
    public void Initialize()
    {
        for (int i = 0; i < selectionItems.Count; i++)
        {
            float radians = 2 * Mathf.PI / selectionItems.Count * i + Mathf.PI * 0.5f;

            float vertical = Mathf.Sin(radians);
            float horizontal = Mathf.Cos(radians);

            RectTransform rectTransform = (RectTransform)transform;

            Vector3 direction = new Vector3(horizontal, vertical, 0);
            Vector3 position = transform.position + direction * rectTransform.sizeDelta.x * distanceFromCenter;

            CircularSelectionItem newSelectionItem = Instantiate(selectionItems[i], transform.position, Quaternion.identity, transform);

            itemPositionDictionary.Add(newSelectionItem, position);
            instantiatedItems.Add(newSelectionItem);
        }

        this.transform.SetAsFirstSibling();
    }

    private void InitializeSelectionItem(CircularSelectionItem selectionItem)
    {
        CircularSelectionItem newSelectionItem = Instantiate(selectionItem, this.transform);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //foreach (CircularSelectionItem item in instantiatedItems)
        //{
        //    item.gameObject.transform.DOMove(itemPositionDictionary[item], 1.0f);
        //}
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //foreach (CircularSelectionItem item in instantiatedItems)
        //{
        //    item.gameObject.transform.DOMove(transform.position, 1.0f);
        //}
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (CircularSelectionItem item in instantiatedItems)
        {
            Vector3 position = isActive ? transform.position : itemPositionDictionary[item];
            Ease ease = isActive ? Ease.OutSine : Ease.OutBack;
            item.gameObject.transform.DOMove(position, duration).SetEase(ease);
        }

        

        isActive = !isActive;
    }
}
