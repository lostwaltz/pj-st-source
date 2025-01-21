using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class UIStageGoalPanel : UIBase
{
    public GameObject WinContainer;
    public GameObject LoseContainer;

    public GameObject WinElement;
    public GameObject LoseElement;

    private GameObject curWin;
    private GameObject curLose;
    
    protected override void OpenProcedure()
    {
        StageSO stageSo = Core.DataManager.SelectedStage;
        foreach (var loseCondition in stageSo.lostConditions)
            loseCondition.Init(this);
        
        foreach (var winCondition in stageSo.winConditions)
            winCondition.Init(this);
        
        UpdateUI();
    }

    public void UpdateUI()
    {
        StageSO stageSo = Core.DataManager.SelectedStage;

        foreach (var loseCondition in stageSo.lostConditions)
        {
            if(curLose != null) Destroy(curLose);
            
            curLose = Instantiate(LoseElement, LoseContainer.transform);
            curLose.GetComponentInChildren<TMP_Text>().text = loseCondition.Description();
            curLose.SetActive(true);
        }
        foreach (var winCondition in stageSo.winConditions)
        {
            if(curWin != null) Destroy(curWin);
            
            curWin = Instantiate(WinElement, WinContainer.transform);
            curWin.GetComponentInChildren<TMP_Text>().text =
                Utils.Str.Clear().Append("- ").Append(winCondition.Description()).ToString();
            curWin.SetActive(true);
        }   
    }
}
