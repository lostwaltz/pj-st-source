using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public static class DialogHelper
{
    public static List<DialogInfo> GetDialogDataList(int tableID)
    {
        List<DialogInfo> result = new();
        
        DialogTable data = Core.DataManager.DialogTable.GetByKey(tableID);

        foreach (var infoKey in data.DialogList)
            result.Add(Core.DataManager.DialogInfo.GetByKey(infoKey));

        return result;
    }
}
