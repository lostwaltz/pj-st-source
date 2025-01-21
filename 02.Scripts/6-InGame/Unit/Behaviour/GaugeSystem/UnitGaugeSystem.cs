using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitGaugeSystem : MonoBehaviour
{
    private readonly Dictionary<Type, Gauge> gauges = new();

    public void InitGauge<T>(int value, int maxValue) where T : Gauge, new()
    {
        GetGauge<T>().InitMaxValue(maxValue);
        Charge<T>(value);
    }
    
    private T CreateGauge<T>() where T : Gauge, new()
    {
        if (false == gauges.TryGetValue(typeof(T), out var currency))
            currency = new T();

        return (T)currency;
    }

    public void Charge<T>(int amount) where T : Gauge, new()
    {
        Type type = typeof(T);
        gauges[typeof(T)] = gauges.TryGetValue(type, out var gauge) ? gauge : CreateGauge<T>();
        
        gauges[typeof(T)].Charge(amount);
    }

    public bool Use<T>(int amount) where T : Gauge, new()
    {
        Type type = typeof(T);
        gauges[typeof(T)] = gauges.TryGetValue(type, out var gauge) ? gauge : CreateGauge<T>();

        return gauges[type].Use(amount);
    }

    public int GetValue<T>() where T : Gauge
    {
        Type type = typeof(T);
        return gauges.TryGetValue(type, out var gauge) ? gauge.Value : 0;
    }

    public T GetGauge<T>() where T : Gauge, new()
    {
        Type type = typeof(T);
        gauges[typeof(T)] = gauges.TryGetValue(type, out var gauge) ? gauge : CreateGauge<T>();

        return gauges[type] as T;
    }
}