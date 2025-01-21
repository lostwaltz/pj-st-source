using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIEventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler, IBeginDragHandler, IEndDragHandler
{
    public Action<PointerEventData> OnClickEvent { get; set; }
    public Action<PointerEventData> OnDragEvent { get; set; }
    public Action<PointerEventData> OnBeginDragEvent { get; set; }
    public Action<PointerEventData> OnEndDragEvent { get; set; }
    public Action<PointerEventData> OnEnterEvent { get; set; }
    public Action<PointerEventData> OnExitEvent { get; set; }
    public Action<PointerEventData> OnDownEvent { get; set; }
    public Action<PointerEventData> OnUpEvent { get; set; }

    public UnityEvent OnClick;
        
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickEvent?.Invoke(eventData);
        
        OnClick?.Invoke();
    }
    public void OnDrag(PointerEventData eventData)
    {
        OnDragEvent?.Invoke(eventData);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        OnBeginDragEvent?.Invoke(eventData);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnEndDragEvent?.Invoke(eventData);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnEnterEvent?.Invoke(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnExitEvent?.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnUpEvent?.Invoke(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDownEvent?.Invoke(eventData);
    }
}
