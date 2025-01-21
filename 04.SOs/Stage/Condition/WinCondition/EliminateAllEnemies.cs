using System;
using System.Collections;
using System.Collections.Generic;
using EnumTypes;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.PlayerLoop;


[CreateAssetMenu(fileName = "EliminateAllEnemies", menuName = "GameConditionSO/EliminateAllEnemies")]
public class EliminateAllEnemies : GameConditionPredicate
{
    private int enemyCnt;
    private int eliminateCnt;
    
    public override void Init(UIStageGoalPanel uiStageGoalPanel)
    {
        base.Init(uiStageGoalPanel);
        enemyCnt = GameUnitManager.Instance.Units[UnitType.EnemyUnit].Count;
        eliminateCnt = 0;
        
        Core.EventManager.Subscribe<UnitOnDeathEvent>(UpdateDesc);
    }

    private void UpdateDesc(UnitOnDeathEvent unitOnDeathEvent)
    {
        Debug.Log("unitOnDeathEvent");
        
        if(unitOnDeathEvent.UnitType == UnitType.PlayableUnit) return;

        eliminateCnt++;
        
        UIStageGoalPanel.UpdateUI();
    }
    
    public override bool IsConditionMet()
    {
        var result = GameUnitManager.Instance.Units[GameUnitManager.Enemy].Count <= 0;;

        return result;
    }
    
    public override string Description()
    {
        return $"모든 적 섬멸 - {eliminateCnt} / {enemyCnt}";
    }
}
