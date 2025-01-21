using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UIPopupMessage : UIPopup
{
    [SerializeField] private TMP_Text messageBox;
    [SerializeField] private float autoCloseTime = 1.0f;

    private Coroutine coroutine;
    
    private void Awake()
    {
        Duration = 0.15f;
    }

    public void ShowMessage(string message, float duration = 1.0f, bool autoClose = true)
    {
        autoCloseTime = duration;
        messageBox.text = message;
        Open();

        if(!autoClose) return;
        
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        
        coroutine = StartCoroutine(AutoClose());
    }

    public void ShowMessage(string message, ref Action callback)
    {
        callback += WrapCoroutine;
        ShowMessage(message, 0f, false);
    }

    private void WrapCoroutine()
    {
        autoCloseTime = 0f;
        StartCoroutine(AutoClose());
    }

    private IEnumerator AutoClose()
    {
        yield return new WaitForSeconds(autoCloseTime);
        Close();
    }
}