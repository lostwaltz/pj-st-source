using System;
using UnityEngine;
using EnumTypes;

public class UnitHealthSystem : MonoBehaviour, IDamagable
{
    UnitStatsSystem stat;
    UnitShieldSystem shield;
    public bool IsDead { get; private set; }

    public int MaxHealth => stat.Stats.Health;
    [SerializeField] int health;


    public int Health
    {
        get => health;
        set
        {
            if (IsDead)
                return;
            
            health = value;

            if (health <= 0f)
                health = 0;
        }
    }

    public event Action<int> OnDamageTaken;
    public event Action OnDead;
    public event Action<float> OnHealthRestored;


    public void Initialize()
    {
        stat = GetComponent<UnitStatsSystem>();
        shield = GetComponent<UnitShieldSystem>();

        health = MaxHealth;
        IsDead = false;

        GameManager.Instance.CommandController.OnPostProcessed += CheckDead;
    }

    public float GetPercentage()
    {
        return (float)health / MaxHealth;
    }

    public float CalculatePercentage(int value)
    {
        return (float)value / MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (IsDead)
            return;

        Core.EventManager.Publish(new AchievementEvent(ExternalEnums.AchActionType.Score,
            ExternalEnums.AchTargetType.Damage, damage));
        
        shield.TakeDamage(ref damage);

        Health -= damage;
        OnDamageTaken?.Invoke(damage);
    }

    public int RecoverHealth(int amount, bool isRevivable = false)
    {
        if (isRevivable)
        {
            IsDead = false;
        }
        else if (IsDead)
            return -1;

        var newHealth = Health + amount;
        if (newHealth > MaxHealth)
        {
            amount = newHealth - MaxHealth;
            newHealth = MaxHealth;
        }

        Health = newHealth;
        OnHealthRestored?.Invoke(GetPercentage());

        return amount;
    }

    public void CheckDead(GamePhase phase)
    {
        if (!IsDead && health == 0)
        {
            IsDead = true;
            OnDead?.Invoke();
        }
    }
}