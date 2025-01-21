using System;

public class BasicCombatStatModifier : CombatStatModifier
{
    private readonly CombatStatType type;
    private readonly Func<float, float> operation;

    public BasicCombatStatModifier(CombatStatType type, float duration, Func<float, float> operation) : base(duration)
    {
        this.type = type;
        this.operation = operation;
    }

    public override void Handle(object sender, Query query)
    {
        if (query.CombatStatType != type) return;
            
        query.Value = operation(query.Value);
    }
}