public enum CombatStatType
{ Attack, Defense, Health }

public class CombatStat
{
    private readonly CombatStatMediator mediator;
    public UnitInstance UnitInstance { get; private set; }

    public CombatStatMediator Mediator => mediator;

    public int Attack
    {
        get
        {
            var q = new Query(CombatStatType.Attack, UnitInstance.MaxAttackPower);
            mediator.PerformQuery(this, q);
            return (int)q.Value;
        }
    }
    
    public int Defense
    {
        get
        {
            var q = new Query(CombatStatType.Defense, UnitInstance.MaxDefensive);
            mediator.PerformQuery(this, q);
            return (int)q.Value;
        }
    }
    
    public int Health
    {
        get
        {
            var q = new Query(CombatStatType.Health, UnitInstance.MaxHealth);
            mediator.PerformQuery(this, q);
            return (int)q.Value;
        }
    }
    

    public CombatStat(CombatStatMediator mediator, UnitInstance unitInstance)
    {
        this.mediator = mediator;
        this.UnitInstance = unitInstance;
    }

    public override string ToString() => $"Attack: {Attack} Defense: {Defense} Health: {Health}";
}