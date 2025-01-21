using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class UIMenuPanel : UIBase
{
    public Button menuBtn;
    public GameObject autoBattleBtn;
    public GameObject doubleSpeedBtn;
    public Button menuBtn3;
    public Button menuBtn4;

    public TMP_Text UndoCount;

    private bool toggleColor;

    private bool toggleSpeedColor;
    private bool isNormalSpeed = true;
    
    private void Start()
    {
        BindEvent(autoBattleBtn, _ => OnAutoBattleBtn());
        BindEvent(doubleSpeedBtn, _ => OnClickedToggleSpeed());
        
        menuBtn.onClick.AddListener(Undo);
        GameManager.Instance.CommandController.HistoryChanged += (value) =>
        {
            UndoCount.text = Utils.Str.Clear().Append(value).Append(" / 3").ToString();
        };
    }

    private void Undo()
    {
        GameManager.Instance.CommandUndoHandler.OnEnterUndoMode();
    }

    public void OnClickedMenuBtn()
    {
        //TODO: 턴 되돌리기 버튼 최대 3턴
    }
    public void OnAutoBattleBtn()
    {
        GameManager.Instance.ToggleAutoBtn?.Invoke();

        Utils.ToggleTrigger(ref toggleColor, () => SetColor(autoBattleBtn, new Color32(225, 225, 100, 255)),
            () => SetColor(autoBattleBtn, new Color32(255, 255, 255, 255)));
    }

    private void SetColor(GameObject go, Color color)
    {
        go.GetComponentInChildren<TMP_Text>().color = color;
    }
    
    public void OnClickedToggleSpeed()
    {
        //TODO: 게임 속도 증가
        isNormalSpeed = !isNormalSpeed;
        GameManager.Instance.TimeScaleHandler.SetScale(isNormalSpeed ? 1f : 2f);
        
        Utils.ToggleTrigger(ref toggleSpeedColor, () => SetColor(doubleSpeedBtn, new Color32(225, 225, 100, 255)),
            () => SetColor(doubleSpeedBtn, new Color32(255, 255, 255, 255)));
    }
    public void OnClickedMenuBtn4()
    {
        //TODO: 스킵 버튼 - 턴넘기기
    }
    
}
