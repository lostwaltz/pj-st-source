using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DefeatedAllUnits", menuName = "GameConditionSO/DefeatedAllUnits")]
public class DefeatedAllUnits : GameConditionPredicate
{
    public override bool IsConditionMet() => GameUnitManager.Instance.Units[GameUnitManager.Playable].Count <= 0;

    public override string Description()
    {
        return "유닛 전멸.";
    }
}