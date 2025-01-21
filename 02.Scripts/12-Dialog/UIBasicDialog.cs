using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBasicDialog : UIAnimation
{
    public GameObject interactionObject;
    
    public List<BasicDialog> DialogsContainer = new();
    private int curIndex = 0;

    public event Action OnClose;
    
    protected override void Start()
    {
        base.Start();
        
        UpdateDialog(DialogsContainer[curIndex]);
    }

    public virtual void StartDialog(List<BasicDialog> dialogs)
    {
        DialogsContainer.Clear(); 
        
        curIndex = 0;
        DialogsContainer = dialogs;
        Open();
    }
    

    public void NextDialog(PointerEventData eventData)
    {
        DialogsContainer[curIndex].OnExitDialog();
        
        curIndex++;

        if (curIndex >= DialogsContainer.Count)
        {
            Close();
            interactionObject.SetActive(false);
            
            return;
        }
        
        UpdateDialog(DialogsContainer[curIndex]);
    }

    protected override void OpenProcedure()
    {
        base.OpenProcedure();
        
        BindEvent(interactionObject, NextDialog);
    }

    protected override void CloseProcedure()
    {
        base.CloseProcedure();
        
        OnClose?.Invoke();
    }


    protected virtual void UpdateDialog(BasicDialog dialog)
    {
        dialog.OnEnterDialog();
    }

    public void SetActiveInteractable(bool active)
    {
        interactionObject.SetActive(active);
    }
}
