using System;
using UnityEngine;

public interface IRewardable
{
    public void ApplyReward();
}

public enum RewardType { Credits, Shard, Unit }

[System.Serializable]
public class RewardableFactory
{
    public RewardType RewardType;
    
    [Header("Common Properties")]
    public int Amount = 1;
    
    //For Unit Reward
    [Header("Unit Reward")]
    public int UnitKey;
    public int UnitLevel;
    
    public IRewardable CreateReward()
    {
        return RewardType switch
        {
            RewardType.Credits => new CurrencyReward<Credits>(Amount),
            RewardType.Shard => new CurrencyReward<Shard>(Amount),
            RewardType.Unit => new UnitReward(UnitKey, UnitLevel),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}