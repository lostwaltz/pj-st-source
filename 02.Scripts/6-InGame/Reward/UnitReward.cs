using System.Diagnostics;

public class UnitReward : IRewardable
{
    private readonly UnitInfo unitInfo;
    private readonly int level;
    
    public UnitReward(int unitKey, int level = 1)
    {
        unitInfo = Core.DataManager.UnitTable.GetByKey(unitKey);
        this.level = level;
    }
    
    public void ApplyReward()
    {
        Core.UnitManager.AddUnitInfo(new UnitInstance(unitInfo, level));
    }
}