using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;
using Object = UnityEngine.Object;

public class DialogManager : Singleton<DialogManager>
{
    public UIBasicDialog curDialog;

    public T GetCurDialog<T>() where T : UIBasicDialog
    {
        return curDialog as T;
    }
    
    
    public T ShowDialog<T>(int dialogID) where T : UIBasicDialog
    {
        ReleaseCurDialog();
        
        List<BasicDialog> dialogs = new ();
        
        List<DialogInfo> dialogList = DialogHelper.GetDialogDataList(dialogID);

        foreach (var info in dialogList)
        {
            dialogs.Add(new BasicDialog(info));
        }

        T ui = Resources.Load<T>(Utils.Str.Clear().Append(Path.UIPath).Append(typeof(T)).ToString());
        ui = Instantiate(ui);
        
        DontDestroyOnLoad(ui);
        ui.StartDialog(dialogs);

        curDialog = ui;
        
        return ui;
    }

    public void ReleaseCurDialog()
    {
        if(curDialog == null) return;
        
        Destroy(curDialog.gameObject);
        curDialog = null;
    }
}
