using System;
using UnityEngine;
using Random = UnityEngine.Random;


// 안정지수 관리 클래스
public class UnitStabilitySystem : MonoBehaviour
{
    Unit unit;
    private UnitGaugeSystem gauge;
    
    // 은폐 엄폐 시, 보너스 합산해 줄 것
    readonly int bonus = 60;
    
    public int StabilityBonus => IsStable ? bonus : 0;
    public bool IsStable => gauge.GetGauge<Stability>().Value > 0;
    public int Value => gauge.GetGauge<Stability>().Value;
    public int MaxValue => gauge.GetGauge<Stability>().MaxValue;

    public void Initialize(Unit owner)
    {
        unit = owner;
        gauge = unit.GaugeSystem;
        
        gauge.InitGauge<Stability>(unit.data.UnitBase.Stability, unit.data.UnitBase.Stability);  
    }

    // 안정지수 데미지
    public void TakeStableDamage(int damage)
    {
        gauge.GetGauge<Stability>().Use(damage);
    }
    
    // 안정지수 회복
    public void RecoverStability(int amount)
    {
        // 안정지수는 자연 회복은 안되고, 유닛의 효과로만 가능
        gauge.GetGauge<Stability>().Charge(amount);
    }
}

