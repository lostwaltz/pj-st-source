

public abstract class Currency : DataModel
{
    public int Amount { get; protected set; }
    
    public void SetAmount(int amount) { Amount = amount; }

    public abstract void Add(int amount);

    public virtual bool Spend(int amount)
    {
        if(Amount < amount) return false;
        
        Amount -= amount;
        
        return true;
    }
}