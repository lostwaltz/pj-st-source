using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIUnitInfoPanel : UIBase
{
    [SerializeField] private UIRemoldingGauge gauge;
    [SerializeField] private UIStability stability;
    [SerializeField] private UIShield shield;
    
    [SerializeField] private Image playableUnitImg;
    [SerializeField] private Image playableUnitHP;
    [SerializeField] private TMP_Text playableUnitHPTxt;

    [SerializeField] private TMP_Text nameTxt;
    [SerializeField] private TMP_Text levelTxt;

    [SerializeField] private TMP_Text attackPowerTxt;
    [SerializeField] private TMP_Text defensivePowerTxt;
    [SerializeField] private TMP_Text criticalTxt;
    [SerializeField] private TMP_Text multipleTxt;
    
    public void SetUnitInfo(Unit selectedUnit)
    {
        UnitInstance instance = selectedUnit.data;
        UnitInfo unitInfo = instance.UnitBase;
        UnitHealthSystem unitHealthSystem = selectedUnit.HealthSystem;
            
        playableUnitImg.sprite = Resources.Load<Sprite>(unitInfo.Path);

        nameTxt.text = unitInfo.Name;
        levelTxt.text = $"Lv. {instance.Level}";

        playableUnitHP.fillAmount = unitHealthSystem.GetPercentage();
        playableUnitHPTxt.text = $"{unitHealthSystem.Health}/{unitHealthSystem.MaxHealth}";

        attackPowerTxt.text = selectedUnit.StatsSystem.Stats.Attack.ToString();
        defensivePowerTxt.text = selectedUnit.StatsSystem.Stats.Defense.ToString();
        //아래 스텟 관련 추후수정필요
        criticalTxt.text = unitInfo.Critical.ToString();
        multipleTxt.text = unitInfo.Multiple.ToString();

        ReMolding reMolding = selectedUnit.GaugeSystem.GetGauge<ReMolding>();
        gauge.OnValueChanged(reMolding.Value, reMolding.MaxValue);
        
        stability.ClearEvent();
        stability.SetInfo(selectedUnit, (value, max) => { return $"{value}/{max}";});
        
        shield.SetInfo(selectedUnit);
    }
}
