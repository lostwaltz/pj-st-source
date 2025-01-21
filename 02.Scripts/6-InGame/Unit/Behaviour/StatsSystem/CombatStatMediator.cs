using System;
using System.Collections.Generic;

public class CombatStatMediator
{
    private readonly LinkedList<CombatStatModifier> modifiers = new();

    public event EventHandler<Query> Queries;
    public void  PerformQuery(object sender, Query query) => Queries?.Invoke(sender, query);

    public void AddModifier(CombatStatModifier modifier)
    {
        modifiers.AddLast(modifier);
        Queries += modifier.Handle;

        modifier.OnDisposed += _ =>
        {
            modifiers.Remove(modifier);
            Queries -= modifier.Handle;
        };

        Update(0f);
    }

    public void Update(float deltaTime)
    {
        var node = modifiers.First;
        while (node != null)
        {
            var modifer = node.Value;
            modifer.Update(deltaTime);
            node = node.Next;
        }
        
        node = modifiers.First;
        while (node != null)
        {
            var nextNode = node.Next;

            if (node.Value.MarkedForRemoval)
                node.Value.Dispose();
            
            node = nextNode;
        }
    }
}

public class Query
{
    public readonly CombatStatType CombatStatType;
    public float Value;
    
    public Query(CombatStatType combatStatType, int value)
    {
        CombatStatType = combatStatType;
        Value = value;
    }
}