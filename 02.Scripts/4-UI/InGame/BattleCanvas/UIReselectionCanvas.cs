using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIReselectionCanvas : UIBase
{
    private readonly string defalutMessage = "스킬 범위 내의 적군 목표 1개를 클릭하십시오.";
    [SerializeField] private TMP_Text reselectionTxt; // TMP_Text
    /// <summary>
    /// 기본 노출 시간
    /// </summary>
    [SerializeField] private int lifeTime = 3;

    private void OnEnable()
    {
        UISound.PlayError();
        
        if (IsInvoking("AutoClose"))
            CancelInvoke("AutoClose");
        
        Invoke("AutoClose", lifeTime);
    }

    public void Open(string msg)
    {
        reselectionTxt.text = msg.Equals(String.Empty) ? defalutMessage : msg;
        base.Open();
    }

    void AutoClose()
    {
        base.Close();
    }
}
