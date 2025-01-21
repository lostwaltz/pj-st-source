using System;
using EnumTypes;
using UnityEngine;

public class UnitShieldSystem : MonoBehaviour
{
    private Unit unit;
    private UnitGaugeSystem gauge;
    
    private Shield shield;
    
    private CountdownTimer timer;
    
    public event Action OnDestroyShield;
    public event Action<float> OnValueChange;
    
    public void Initialize(Unit owner)
    {
        unit = owner;
        gauge = unit.GaugeSystem;
        
        gauge.InitGauge<Shield>(0, int.MaxValue);
        shield = gauge.GetGauge<Shield>();
        
        GameManager.Instance.SubscribePhaseEvent(GamePhase.PhaseCount, UpdateShield);
        shield.Subscribe(ValueChangeType.EmptyValue, DestroyShield);
        shield.Subscribe(ValueChangeType.ChangeValue, OnValueChanged);
    }

    public void TakeDamage(ref int damage)
    {
        if (shield.Use(damage))
        {
            damage = 0;
            return;
        }

        damage -= shield.Value;
    }

    public void ChargeShield(int amount, int duration)
    {
        shield.Charge(amount);
        
        timer = new CountdownTimer(duration);
        timer.OnTimerStop += () => shield.Use(int.MaxValue);
        timer.Start();
    }

    public void UpdateShield()
    {
        if(shield.Value <= 0)
            return;
        
        timer?.Tick(1f);
    }

    private void DestroyShield(int min, int max)
    {
        OnDestroyShield?.Invoke();
        
        OnDestroyShield = null;
    }

    private void OnValueChanged(int value, int max)
    {
        OnValueChange?.Invoke((float)value / unit.HealthSystem.MaxHealth);
    }
}
