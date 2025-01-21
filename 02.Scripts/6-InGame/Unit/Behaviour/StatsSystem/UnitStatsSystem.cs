using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OperatorType
{ Add, Multiply, Override }

public class UnitStatsSystem : MonoBehaviour
{
    public CombatStat Stats { get; private set; }

    public void InitStats(UnitInstance unitInstance)
    {
        Stats = new CombatStat(new CombatStatMediator(), unitInstance);
    }

    public void AddModifier(CombatStatType statType, OperatorType operatorType, float amount, float duration)
    {
        CombatStatModifier modifier = operatorType switch
        {
            OperatorType.Add => AddModifiers(statType, duration, v => v + amount),
            OperatorType.Multiply => AddModifiers(statType, duration, v => v * amount),
            OperatorType.Override => AddModifiers(statType, duration, v => amount),
            _ => throw new ArgumentOutOfRangeException(nameof(operatorType), operatorType, null)
        };

        Stats.Mediator.AddModifier(modifier);
    }

    private CombatStatModifier AddModifiers(CombatStatType statType, float duration, Func<float, float> operation)
    {
        return new BasicCombatStatModifier(statType, duration, operation);
    }

    public void OnNewTurnStarted()
    {
        Stats.Mediator.Update(1f);
    }
}
