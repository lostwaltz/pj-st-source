
public static class SkillStatCalculator
{
    public static float CalculatePower(SkillInfo baseData, int level)
    {
        var multiplier = baseData.PowerScaling * level;
        return (baseData.Power + multiplier);
    }
    
    public static int CalculateRange(SkillInfo baseData, int level)
    {
        var multiplier = baseData.RangeAmount * (level / baseData.RangeAmountStep);
        return (int)(baseData.Range + multiplier);
    }
}
