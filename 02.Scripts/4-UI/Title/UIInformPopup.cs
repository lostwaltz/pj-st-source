using System;
using Constants;
using TMPro;
using UnityEngine;

public class UIInformPopup : UIPopup
{
    [Header("PopUp")]
    [SerializeField] TextMeshProUGUI txtMessage;

    [SerializeField] TextMeshProUGUI txtButton;

    private Action onClickConfirmed;

    public void Initialize(string msg, string btnText = CommonUI.CONFIRM, Action onConfirmed = null)
    {
        txtMessage.text = msg;
        txtButton.text = btnText;
        onClickConfirmed = onConfirmed;

        Open();
    }

    public void OnClickConfirm()
    {
        UIBattle.PlayUIBattleClickNormalSound();
        onClickConfirmed?.Invoke();
        Close();
    }
}