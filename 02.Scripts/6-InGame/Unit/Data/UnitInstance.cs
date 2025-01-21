using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[Serializable]
public class UnitInstance
{
    public UnitInfo UnitBase;
    public int Level;
    public int CurrentExp;
    
    public GachaSystemManager GachaSystemManager;

    public int MaxAttackPower => UnitStatCalculator.CalculateAttack(UnitBase, Level, SpineLevel) + WeaponInfo.Attack;
    public int MaxDefensive => UnitStatCalculator.CalculateDefense(UnitBase, Level, SpineLevel);
    public int MaxHealth => UnitStatCalculator.CalculateHealth(UnitBase, Level, SpineLevel);
    public int MaxExp => UnitStatCalculator.CalculateExp(UnitBase, Level);

    public int TotalPower => MaxAttackPower + MaxDefensive + MaxHealth;
    
    public float GetExpPercentage() => (float)CurrentExp / MaxExp;
    
    // TODO: 스킬, 장비 인스턴스 데이터 추가
    public readonly WeaponInfo WeaponInfo;
    
    public const int SkillCount = 5;
    private readonly List<SkillInstance> skillList = new();
    public ReadOnlyCollection<SkillInstance> SkillList { get; private set; }
    
    public int SpineLevel; // 돌파 레벨 : 중추

    public const int MaxSpineLevel = 6;
    
    public int Pieces
    {
        get => Core.CurrencyManager.GetCurrency<Pieces>().GetPieces(UnitBase.Key);
        set => Core.CurrencyManager.GetCurrency<Pieces>().SetPieces(UnitBase.Key, value);
    }

    public UnitInstance(UnitInfo unitBase,  int level = 1)
    {
        UnitBase = unitBase;
        Level = level;

        WeaponInfo = Core.DataManager.WeaponTable.GetByKey(UnitBase.WeaponKey);

        foreach (var skillKey in UnitBase.Skill)
            skillList.Add(new SkillInstance(Core.DataManager.SkillTable.GetByKey(skillKey)));
        
        SkillList = new ReadOnlyCollection<SkillInstance>(skillList);
    }
    
    //가챠 시스템을 위한 생성자
    public UnitInstance(UnitInfo unitBase,  GachaSystemManager gachaSystemManager )
    {
        UnitBase = unitBase;
        GachaSystemManager = gachaSystemManager;

        Level = 1;
        WeaponInfo = Core.DataManager.WeaponTable.GetByKey(UnitBase.WeaponKey);

        foreach (var skillKey in UnitBase.Skill)
            skillList.Add(new SkillInstance(Core.DataManager.SkillTable.GetByKey(skillKey)));
        
        SkillList = new ReadOnlyCollection<SkillInstance>(skillList);
    }

    public void AddPieces(int amount)
    {
        Pieces pieces = Core.CurrencyManager.GetCurrency<Pieces>();
        pieces.SetCurrentUnitKey(UnitBase.Key).Add(amount);
    }

    public void SpineLevelUp(Action<int> successAction = null)
    {
        Pieces pieces = Core.CurrencyManager.GetCurrency<Pieces>();
        var success = pieces.SetCurrentUnitKey(UnitBase.Key).Spend(30);

        if (SpineLevel >= MaxSpineLevel)
            success = false;
        
        SpineLevel += success ? 1 : 0;

        if (!success) return;
        
        successAction?.Invoke(SpineLevel - 1);
        Core.EventManager.Publish(new AchievementEvent(ExternalEnums.AchActionType.Upgrade,
            ExternalEnums.AchTargetType.Spine, 1, UnitBase.Key));
    }
    
    public void AddExp(int exp)
    {
        CurrentExp += exp;
        
        while (CurrentExp >= MaxExp)
        {
            CurrentExp -= MaxExp;
            LevelUp(); // 레벨업 처리
        }

        if (!IsMaxLevel()) return;
        
        CurrentExp = MaxExp;
    }

    public void AddMaxExp()
    {
        AddExp(NeedExpToLevelUp());
    }

    public int NeedExpToLevelUp()
    {
        return MaxExp - CurrentExp;
    }
    
    private void LevelUp()
    {
        Level++;
        
        Level.Clamp(1, UnitBase.MaxLevel);
    }

    private bool IsMaxLevel()
    {
        return Level >= UnitBase.MaxLevel;
    }
}