using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIActionCancelPanel : UIBase
{
    public void OnClickActionCancelBtn()
    {
        GameManager.Instance.Interaction.ExecuteCommand();
    }
}
