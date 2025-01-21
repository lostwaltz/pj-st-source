using System;
using EnumTypes;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICursorDetector : MonoBehaviour
{
    private Action OnDisableDetector;
    
    protected virtual void Awake()
    {
        UIBase.BindEvent(gameObject, OnCursorEnter, UIEvent.Enter);
        UIBase.BindEvent(gameObject, OnCursorExit, UIEvent.Exit);
        
        Core.UIManager.CursorDetectors.Add(this);
        
        OnDisableDetector += Exit;
    }

    void OnCursorEnter(PointerEventData eventData)
    {
        GameManager.Instance.Interaction.CursorIsOnUI = true;
    }

    void OnCursorExit(PointerEventData eventData)
    {
        if(null != GameManager.Instance.Interaction)
            GameManager.Instance.Interaction.CursorIsOnUI = false;
    }

    private void OnDisable()
    {
        OnDisableDetector?.Invoke();
    }

    private void Exit()
    {
        OnCursorExit(null);
    }

    public void Release()
    {
        OnDisableDetector = null;
    }
    
}