using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDialog
{
    public DialogInfo Data;

    public event Action OnEnter;
    public event Action OnExit;
    
    public BasicDialog(DialogInfo dialogInfo)
    {
        Data = dialogInfo;
    }

    public virtual void OnEnterDialog()
    {
        OnEnter?.Invoke();
    }

    public virtual void OnExitDialog()
    {
        OnExit?.Invoke();
    }
}
