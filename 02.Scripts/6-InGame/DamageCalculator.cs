using System.Collections.Generic;
using UnityEngine;

public static class DamageCalculator
{
    public static void CalculateDamage(
        Unit attacker, int skillIndex, Vector2 attackPoint, 
        ref List<Unit> targets, out List<int> damages)
    {
        // 계산식 = (데미지 - 방어력) * (1 - 방어 관련 보너스 * 0.01f)
        // 데미지 : 공격자의 공격력 * 선택한 스킬의 공격력
        // 방어력 : 피격자의 방어력
        // 방어 관련 보너스 : 피격자의 안정 보너스 + 피격자의 엄폐 보너스
        
        damages = new List<int>();
        for (int i = 0; i < targets.Count; i++)
        {
            int damage = (int)(attacker.StatsSystem.Stats.Attack * attacker.SkillSystem.Skills[skillIndex].Data.SkillPower);

            int critical = GetCritical(attacker.data.UnitBase.Critical, attacker.data.UnitBase.Multiple,
                out bool isCritical);
            damage = (int)(damage * critical * 0.01f);
            
            int targetDefense = targets[i].StatsSystem.Stats.Defense;
            int addictiveBonus = targets[i].StabilitySystem.StabilityBonus + 
                                 targets[i].CoverSystem.GetCoverBonus(targets[i].curCoord, attackPoint);
            
            // Debug.Log($"damage: {damage}, target def: {targetDefense}, addictive bonus: {addictiveBonus}");
            
            int totalDamage = damage - targetDefense;
            totalDamage.Clamp(0, totalDamage);
            
            // Debug.Log($"totalDamage: {totalDamage}");
            
            totalDamage = (int)(totalDamage * (1f - addictiveBonus * 0.01f));
            // Debug.Log($"데미지 차감률: {addictiveBonus * 0.01f}, 반영된 데미지 비율: {1f - addictiveBonus * 0.01f}");
            // Debug.Log($"after calc: {totalDamage}");
            
            damages.Add(totalDamage);
        }
    }
    
    public static void CalculateDamage(
        Unit attacker, Skill skill, Vector2 attackPoint, 
        ref List<Unit> targets, out List<int> damages)
    {
        // 계산식 = (데미지 - 방어력) * (1 - 방어 관련 보너스 * 0.01f)
        // 데미지 : 공격자의 공격력 * 선택한 스킬의 공격력
        // 방어력 : 피격자의 방어력
        // 방어 관련 보너스 : 피격자의 안정 보너스 + 피격자의 엄폐 보너스
        
        damages = new List<int>();
        for (int i = 0; i < targets.Count; i++)
        {
            int damage = (int)(attacker.StatsSystem.Stats.Attack * skill.Data.SkillPower);

            int critical = GetCritical(attacker.data.UnitBase.Critical, attacker.data.UnitBase.Multiple,
                out bool isCritical);
            damage = (int)(damage * critical * 0.01f);
            
            int targetDefense = targets[i].StatsSystem.Stats.Defense;
            int addictiveBonus = targets[i].StabilitySystem.StabilityBonus + 
                                 targets[i].CoverSystem.GetCoverBonus(targets[i].curCoord, attackPoint);
            
            // Debug.Log($"damage: {damage}, target def: {targetDefense}, addictive bonus: {addictiveBonus}");
            
            int totalDamage = damage - targetDefense;
            totalDamage.Clamp(0, totalDamage);
            
            // Debug.Log($"totalDamage: {totalDamage}");
            
            totalDamage = (int)(totalDamage * (1f - addictiveBonus * 0.01f));
            // Debug.Log($"데미지 차감률: {addictiveBonus * 0.01f}, 반영된 데미지 비율: {1f - addictiveBonus * 0.01f}");
            // Debug.Log($"after calc: {totalDamage}");
            
            damages.Add(totalDamage);
        }
    }

    public static void CalculateHeal()
    {
        
    }
    
    public static int GetCritical(int criticalProbability, int criticalMultiplier, out bool isCritical)
    {
        isCritical = Random.Range(1, 101) <= criticalProbability;
        return isCritical ? criticalMultiplier : 100;
    }
}