using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIToolTipMessage : UIBase
{
    [SerializeField] TextMeshProUGUI textMessage;

    public void SetMessage(string msg, Vector3 position)
    {
        transform.position = position;
        textMessage.text = msg;
        
        Open();
    }
}
