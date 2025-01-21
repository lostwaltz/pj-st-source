using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragHandler
{
    private readonly GameObject target;

    public bool StopHandleDrag = false;
    
    public UIDragHandler(GameObject target)
    {
        this.target = target;
    }

    public void HandleDragBegin(PointerEventData eventData)
    {
        // BeginDrag 호출
        ExecuteEvents.Execute(target, eventData, ExecuteEvents.beginDragHandler);
    }

    public void HandleDrag(PointerEventData eventData)
    {
        if(StopHandleDrag) return;
        
        // Drag 호출
        ExecuteEvents.Execute(target, eventData, ExecuteEvents.dragHandler);
    }

    public void HandleDragEnd(PointerEventData eventData)
    {
        // EndDrag 호출
        ExecuteEvents.Execute(target, eventData, ExecuteEvents.endDragHandler);
    }
}