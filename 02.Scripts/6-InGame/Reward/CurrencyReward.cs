using System;

public class CurrencyReward<T> : IRewardable where T : Currency, new()
{
    private readonly int rewardAmount;
    
    public CurrencyReward(int rewardAmount)
    {
        this.rewardAmount = rewardAmount;
    }
    
    public void ApplyReward()
    {
        Core.CurrencyManager.Add<T>(rewardAmount);
    }
}