

using System;
using System.Collections.Generic;
using EnumTypes;

public enum ValueChangeType
{
    TryValueCharge,
    TryValueUse,
    
    ChangeValue,
    
    ChargeValue,
    UseValue,
    
    EmptyValue,
    FullValue,
    
    LackValue
}

public abstract class Gauge
{
    private readonly Dictionary<ValueChangeType, Action<int, int>> onValueChanged = new();

    public int Value { get; protected set; }
    public int MaxValue { get; protected set; }

    public float GetPercentage() => (float)Value / MaxValue;

    public void InitMaxValue(int maxValue)
    {
        MaxValue = maxValue;
    }
    
    public virtual void Charge(int amount)
    {
        Publish(ValueChangeType.TryValueCharge, Value, MaxValue);

        var prevValue = Value;
        Value += amount;

        Value = HandleValueChange(Value, prevValue);
        Publish(ValueChangeType.ChargeValue, Value, Value);
    }

    public virtual bool Use(int amount)
    {
        Publish(ValueChangeType.TryValueUse, Value, MaxValue);

        if (Value < amount)
        {
            Publish(ValueChangeType.LackValue, Value, MaxValue);
            return false;
        }

        var prevValue = Value;
        Value -= amount;

        Value = HandleValueChange(Value, prevValue);
        Publish(ValueChangeType.UseValue, Value, MaxValue);

        return true;
    }
    
    protected int HandleValueChange(int newValue, int prevValue)
    {
        if (newValue > MaxValue) newValue = MaxValue;
        if (newValue < 0) newValue = 0;

        if (newValue != prevValue)
            Publish(ValueChangeType.ChangeValue, newValue, MaxValue);

        if (newValue == MaxValue)
            Publish(ValueChangeType.FullValue, newValue, MaxValue);

        if (newValue == 0)
            Publish(ValueChangeType.EmptyValue, newValue, MaxValue);

        return newValue;
    }
    
    public void Subscribe(ValueChangeType trigger, Action<int, int> action)
    {
        if (onValueChanged.TryGetValue(trigger, out Action<int, int> changeTypeAction))
        {
            changeTypeAction += action;
            onValueChanged[trigger] = changeTypeAction;
        }
        else
        {
            changeTypeAction = action;
            onValueChanged.Add(trigger, changeTypeAction);
        }
    }

    public void Unsubscribe(ValueChangeType trigger, Action<int, int> action)
    {
        if (!onValueChanged.TryGetValue(trigger, out Action<int, int> changeTypeAction)) return;
        
        changeTypeAction -= action;
        
        if(changeTypeAction == null)
            onValueChanged.Remove(trigger);
        else
            onValueChanged[trigger] = changeTypeAction;
    }

    protected void Publish(ValueChangeType trigger, int curValue, int maxValue)
    {
        if(onValueChanged.TryGetValue(trigger, out Action<int, int> valueChangedAction))
            valueChangedAction?.Invoke(curValue, maxValue);
    }
}