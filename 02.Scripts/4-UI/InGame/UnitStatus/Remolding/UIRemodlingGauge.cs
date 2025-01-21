using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Object = System.Object;

public class UIRemoldingGauge : UIBase
{
    [SerializeField] private HorizontalLayoutGroup layoutGroup;
    [SerializeField] private Unit unit;
    [SerializeField] private UIRemoldingElement element;
    [SerializeField] private Transform content;
    
    private readonly List<UIRemoldingElement> uiRemoldingElements = new();
    
    private ReMolding remolding;
    
    private void Start()
    {

        if (unit != null)
        {
            remolding = unit.GaugeSystem.GetGauge<ReMolding>();
            
            remolding.Subscribe(ValueChangeType.ChangeValue, OnValueChanged);
        
            for(int i = 0; i < remolding.MaxValue; i++)
            {
                uiRemoldingElements.Add(Instantiate(element, content));
                uiRemoldingElements[i].gameObject.SetActive(true);
            }

            OnValueChanged(remolding.Value, remolding.MaxValue);
        
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }
    }

    public void OnValueChanged(int value, int maxValue)
    {
        for (int i = 0; i < maxValue; i++)
        {
            if (uiRemoldingElements.Count <= i)
            {
                uiRemoldingElements.Add(Instantiate(element, content));
                uiRemoldingElements[i].gameObject.SetActive(true);
            }
            
            uiRemoldingElements[i].SetRemolding(i < value);
        }
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }
}
