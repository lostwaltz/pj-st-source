using UnityEngine;
using Object = UnityEngine.Object;

public enum SkillSelectType
{
    SingleUnit, // 반경 내, 적 대상 1기 선택
    AllUnits,   // 반경 내, 적 전체 선택
    AllUnitsAround, // 유닛 주변, 적 전체
    SingleCell, // 범위 좌표 1개 선택
    Direction,  // 특정 방향 선택
}

public enum SkillType
{
    Active,
    Passive,
}


public abstract class Skill
{
    public SkillType Type;
    
    private readonly string defaultEffectPath = $"{Constants.Path.Effects}effect_buff_heal";
    private readonly string defaultActionPath = $"{Constants.Path.SkillAction}action_default_attack";

    public readonly Unit OwnerUnit;
    public readonly SkillInstance Data;
    public readonly EffectHandler EffectHandler;
    public readonly ActionHandler ActionHandler;
    
    protected WaitForSeconds WaitForSeconds = new(0.5f);
    
    protected readonly UnitRequirement Requirement;
    protected EffectContext EffectContext;
    
    
    protected Skill(Unit owner, SkillInstance inst)
    {
        if(owner == null) return;
        if(inst == null) return;
        
        OwnerUnit = owner;
        Data = inst;
        
        var isPathEmptyEffect = Data.SkillBase.EffectPath.Equals("...") || 
                           Data.SkillBase.EffectPath.Equals(string.Empty);
        
        var isPathEmptyAction = Data.SkillBase.ActionPath.Equals("...") || 
                                Data.SkillBase.ActionPath.Equals(string.Empty);
        
        EffectHandler = Resources.Load<EffectHandler>(isPathEmptyEffect
            ? defaultEffectPath
            : Utils.Str.Clear().Append(Constants.Path.Effects).Append(Data.SkillBase.EffectPath).ToString());
        
        ActionHandler = Resources.Load<ActionHandler>(isPathEmptyAction
            ? defaultActionPath
            : Utils.Str.Clear().Append(Constants.Path.SkillAction).Append(Data.SkillBase.ActionPath).ToString());
        
        EffectHandler = Object.Instantiate(EffectHandler, OwnerUnit.transform);
        ActionHandler = Object.Instantiate(ActionHandler, OwnerUnit.transform);
        
        Requirement = OwnerUnit.Requirement;
    }

    public void ActivateEffect()
    {
        if(EffectContext == null) return;
        
        EffectHandler.Play(OwnerUnit, EffectContext);
    }


}
