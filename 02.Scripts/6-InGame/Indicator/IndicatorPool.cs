using System;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public enum IndicatorOption
{
    Multiple = 0,
    Unique = 1,

    // 그 외엔 int로 알아서 입력할 것
}

public class IndicatorPool
{
    Dictionary<Type, Dictionary<int, List<IndicatorComponent>>> indicatorPool = new();
    Queue<IndicatorComponent> activeIndicators = new();

    T Create<T>(IndicatorOption option) where T : IndicatorComponent
    {
        Type type = typeof(T);
        T obj = Resources.Load<T>(Path.Indicators + type);
        T inst = GameObject.Instantiate(obj);

        if (!indicatorPool.ContainsKey(type))
        {
            indicatorPool.Add(type, new Dictionary<int, List<IndicatorComponent>>());
            indicatorPool[type].Add((int)option, new List<IndicatorComponent>());
        }

        indicatorPool[type][(int)option].Add(inst);

        return inst;
    }

    public T TryGet<T>(IndicatorOption option) where T : IndicatorComponent
    {
        Type type = typeof(T);

        // 처음 호출하는 경우
        if (!indicatorPool.ContainsKey(type) ||
            !indicatorPool[type].ContainsKey((int)option))
        {
            var comp = Create<T>(option);
            activeIndicators.Enqueue(comp);
            return comp;
        }

        if (option == IndicatorOption.Unique) // 유일 속성
        {
            var comp = indicatorPool[type][(int)option][0] as T;
            activeIndicators.Enqueue(comp);
            return comp;
        }

        // 사용할 수 있는 것 꺼내기
        foreach (var comp in indicatorPool[type][(int)option])
        {
            if (!comp.gameObject.activeInHierarchy)
            {
                activeIndicators.Enqueue(comp);
                return comp as T;
            }
        }
        
        // 사용할 수 있는 것이 없어 새로 만들기
        var newOne = Create<T>(option);
        activeIndicators.Enqueue(newOne);
        return newOne;
    }

    public void Hide<T>(IndicatorOption option) where T : IndicatorComponent
    {
        Type type = typeof(T);
        if (indicatorPool.TryGetValue(type, out var dic) &&
            dic.TryGetValue((int)option, out var list))
        {
            for (int i = 0; i < list.Count; i++)
                list[i].Hide();
        }
    }

    public void HideAll()
    {
        // O(n)
        while (activeIndicators.Count > 0)
        {
            activeIndicators.Dequeue().Hide();
        }
        
        // O(n ^ 3)
        // foreach (var key in indicatorPool.Keys)
        //     foreach (var opt in indicatorPool[key].Keys)
        //         foreach (var comp in indicatorPool[key][opt])
        //             comp.Hide();
    }

}