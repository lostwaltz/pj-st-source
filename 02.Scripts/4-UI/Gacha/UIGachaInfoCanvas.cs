using System;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class UIGachaInfoCanvas : UIPopup
{
    [SerializeField] private TMP_Text unitDescriptionTxt;
    [SerializeField] private TMP_Text unitNameTxt;
    [SerializeField] private TMP_Text unitLevelTxt;
    [SerializeField] private TMP_Text unitTotalPowerTxt;
    [SerializeField] private Image unitImg;
    [SerializeField] private Image weaknessImg1;
    [SerializeField] private Image weaknessImg2;
    [SerializeField] private Image skillImg1;
    [SerializeField] private Image skillImg2;
    [SerializeField] private Image skillImg3;
    [SerializeField] private Image skillImg4;
    [SerializeField] private Image skillImg5;
    [SerializeField] private Image specialityImg;
    [SerializeField] private Button exitbtn;
    [SerializeField] private GameObject closeInteraction;
    [SerializeField] private UIUnitManageSkillInfo skillInfo;

    UnitInstance unitInstance;
    private int skillIndex;
    
    private void Awake()
    {
        exitbtn.onClick.AddListener(OnClickExitSlot);
        BindEvent(closeInteraction, data => OnClickExitSlot());
    }

    public void SetDataOpenUI(UnitInstance unit)
    {
        Init(unit);
        Open();
    }

    public void ShowSkillUI()
    {
        skillInfo.ShowSkillInfo(unitInstance.SkillList[skillIndex].SkillBase);
    }

    public void SetSkillIndex(int skillIndex)
    {
        this.skillIndex = skillIndex;
    }
    
    public void Init(UnitInstance unit)
    {
        unitInstance = unit;
        
        Duration = 0.2f;
        UnitInfo unitInfo = unit.UnitBase;
        unitImg.sprite = Resources.Load<Sprite>(unitInfo.BustPath);
        unitDescriptionTxt.text = unit.UnitBase.UnitDescription;
        unitNameTxt.text = unitInfo.Name;
        unitLevelTxt.text = unit.Level.ToString(); //TODO : 처음 레벨 찾아 넣기
        unitTotalPowerTxt.text = unit.TotalPower.ToString();
        
        //UnitSkillSystem skillSystem = unit.Skill;
        List<SkillInstance> skillInfoList = new List<SkillInstance>(unit.SkillList);
        
        skillImg1.sprite = Resources.Load<Sprite>(skillInfoList[0].SkillBase.Path);
        skillImg2.sprite = Resources.Load<Sprite>(skillInfoList[1].SkillBase.Path);
        skillImg3.sprite = Resources.Load<Sprite>(skillInfoList[2].SkillBase.Path);
        skillImg4.sprite = Resources.Load<Sprite>(skillInfoList[3].SkillBase.Path);
        skillImg5.sprite = Resources.Load<Sprite>(skillInfoList[4].SkillBase.Path);
        
        specialityImg.sprite = Resources.Load<Sprite>(unitInfo.SpecialityPath);
        
    }
    public void OnClickExitSlot()
    {
        UIBattle.PlayUIBattleClickOtherSound();
        skillInfo.Close();
        Close();
    }

}
