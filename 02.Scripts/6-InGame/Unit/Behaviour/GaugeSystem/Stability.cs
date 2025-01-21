using System;

public class Stability : Gauge
{
    public override void Charge(int amount)
    {
        base.Charge(amount);
    }

    public override bool Use(int amount)
    {
        Publish(ValueChangeType.TryValueUse, Value, MaxValue);

        Value = Math.Max(Value - amount, 0);
        
        Publish(ValueChangeType.UseValue, Value, MaxValue);
        Publish(ValueChangeType.ChangeValue, Value, MaxValue);
        
        if(Value <= 0)
            Publish(ValueChangeType.EmptyValue, Value, MaxValue);
        
        return true;
        
    }
}