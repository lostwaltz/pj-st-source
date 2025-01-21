public class SkillInstance
{
    public readonly SkillInfo SkillBase;
    public int Level;
    
    public float SkillPower => SkillStatCalculator.CalculatePower(SkillBase, Level);
    public int SkillRange => SkillStatCalculator.CalculateRange(SkillBase, Level);
    public int Scope => SkillBase.Scope;
    public int Cost => SkillBase.Cost;

    public SkillInstance(SkillInfo skillBase, int level = 1)
    {
        SkillBase = skillBase;
        Level = level;
    }
}