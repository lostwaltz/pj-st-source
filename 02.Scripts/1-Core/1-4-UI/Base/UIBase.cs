using System;
using EnumTypes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;


public abstract class UIBase : MonoBehaviour
{
    public static void BindEvent(GameObject go, Action<PointerEventData> action, EnumTypes.UIEvent type = EnumTypes.UIEvent.Click)
    {
        UIEventHandler evt = go.GetOrAddComponent<UIEventHandler>();

        switch (type)
        {
            default:
            case UIEvent.Click:
                evt.OnClickEvent -= action;
                evt.OnClickEvent += action;
                break;
            case UIEvent.Up:
                evt.OnUpEvent -= action;
                evt.OnUpEvent += action;
                break;
            case UIEvent.Down:
                evt.OnDownEvent -= action;
                evt.OnDownEvent += action;
                break;
            case UIEvent.Drag:
                evt.OnDragEvent -= action;
                evt.OnDragEvent += action;
                break;
            case UIEvent.BeginDrag:
                evt.OnBeginDragEvent -= action;
                evt.OnBeginDragEvent += action;
                break;
            case UIEvent.EndDrag:
                evt.OnEndDragEvent -= action;
                evt.OnEndDragEvent += action;
                break;
            case UIEvent.Enter:
                evt.OnEnterEvent -= action;
                evt.OnEnterEvent += action;
                break;
            case UIEvent.Exit:
                evt.OnExitEvent -= action;
                evt.OnExitEvent += action;
                break;
        }
    }
    
    public virtual void Open()
    {
        gameObject.SetActive(true);
        OpenProcedure();
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
        CloseProcedure();
    }
    
    public virtual void Destroy()
    {
        Destroy(gameObject);
    }
    
    
    protected virtual void OpenProcedure()
    {
    }
    
    protected virtual void CloseProcedure()
    {
        
    }
}
