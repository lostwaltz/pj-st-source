using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIStability : UIBase
{
    private UnitGaugeSystem gauge;
    private UnitStabilitySystem stability;

    [SerializeField] private Sprite[] stabilityIcons;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text labelText;

    private Func<int, int, string> process;
    
    public void SetInfo(Unit owner, Func<int, int, string> infoProcess)
    {
        gauge = owner.GaugeSystem;
        stability = owner.StabilitySystem;
        
        process = infoProcess;
        
        gauge.GetGauge<Stability>().Subscribe(ValueChangeType.ChangeValue, OnValueChanged);
        OnValueChanged(stability.Value, stability.MaxValue);
    }

    public void ClearEvent()
    {
        if(gauge == null) 
            return;
        
        gauge.GetGauge<Stability>().Subscribe(ValueChangeType.ChangeValue, OnValueChanged);
    }
    
    public void OnValueChanged(int value, int maxValue)
    {
        labelText.text = process?.Invoke(value, maxValue);

        int index = value >= maxValue ? 0 : value > 0 ? 1 : 2;
        icon.sprite = stabilityIcons[index];
    }
}
