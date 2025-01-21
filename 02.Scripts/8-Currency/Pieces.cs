using System;
using System.Collections.Generic;

public class Pieces : Currency
{
    private readonly Dictionary<int, int> unitPieces = new();
    private int currentUnitKey;

    public Pieces SetCurrentUnitKey(int unitKey)
    {
        currentUnitKey = unitKey;

        return this;
    }

    public override void Add(int amount)
    {
        unitPieces.TryAdd(currentUnitKey, 0);

        unitPieces[currentUnitKey] += amount;
    
        Amount += amount;
    }

    public override bool Spend(int amount)
    {
        if (!unitPieces.ContainsKey(currentUnitKey) || unitPieces[currentUnitKey] < amount) return false;

        unitPieces[currentUnitKey] -= amount;
        return true;
    }

    public int GetPieces(int unitKey)
    {
        return unitPieces.GetValueOrDefault(unitKey, 0);
    }

    public void SetPieces(int unitKey, int amount)
    {
        unitPieces.TryAdd(unitKey, 0);
        
        unitPieces[unitKey] += amount;
        
        Amount += amount;
    }
}