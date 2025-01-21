public static class UnitStatCalculator
{
    public static int CalculateAttack(UnitInfo baseData, int level, int spine)
    {
        var multiplier = baseData.AttackScaling * level;
        var spineMultiplier = spine * 0.1f;
        
        return baseData.AttackPower + (int)(baseData.AttackPower * multiplier) + (int)(baseData.AttackPower * spineMultiplier);
    }

    public static int CalculateHealth(UnitInfo baseData, int level, int spine)
    {
        var multiplier = baseData.HealthScaling * level;
        var spineMultiplier = spine * 0.1f;
        return baseData.Health + (int)(baseData.Health * multiplier) + (int)(baseData.Health * spineMultiplier);
    }

    public static int CalculateDefense(UnitInfo baseData, int level, int spine)
    {
        var multiplier = baseData.DefensiveScaling * level;
        var spineMultiplier = spine * 0.1f;
        return baseData.DefensivePower + (int)(baseData.DefensivePower * multiplier) + (int)(baseData.DefensivePower * spineMultiplier);
    }
    
    public static int CalculateExp(UnitInfo baseData, int level)
    {
        var multiplier = baseData.ExpScaling * level;
        return (int)baseData.MaxExp + (int)(baseData.MaxExp * multiplier);
    }
}