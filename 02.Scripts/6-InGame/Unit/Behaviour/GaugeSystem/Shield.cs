using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Gauge
{
    public override void Charge(int amount)
    {
        base.Charge(amount);
    }

    public override bool Use(int amount)
    {
        Publish(ValueChangeType.TryValueUse, Value, MaxValue);
        var prevValue = Value;
        
        if (Value < amount)
        {
            Publish(ValueChangeType.LackValue, Value, MaxValue);
            
            Value -= amount;
            Value = HandleValueChange(Value, prevValue);
            Publish(ValueChangeType.UseValue, Value, MaxValue);
            return false;
        }

        Value -= amount;
        Value = HandleValueChange(Value, prevValue);
        Publish(ValueChangeType.UseValue, Value, MaxValue);

        return true;
    }
}
