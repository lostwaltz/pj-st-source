using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EnumTypes;
using UnityEngine;

public class UnitSkillSystem : CommandSwitcher
{
    private Unit unit;
    private UnitAnimation animation;
    private UnitAnimationEventHandler animEvent;
    private UnitStatsSystem stat;

    public readonly List<Skill> Skills = new();
    // Active
    public readonly List<ActiveSkill> ActiveSkills = new();
    
    public void Initialize(Unit owner, ReadOnlyCollection<SkillInstance> instances)
    {
        unit = owner;
        animation = unit.Animation;
        animEvent = unit.AnimationEventHandler;
        stat = unit.StatsSystem;

        Skills.Clear();
        ActiveSkills.Clear();
        
        // 일반 스킬들
        for (int i = 0; i < instances.Count - 1; i++)
        {
            int idx = i;
            ActiveSkill skill = new ActiveSkill(idx, unit, instances[i]);
            ActiveSkills.Add(skill);
            Skills.Add(skill);
        }

        Skill passive = new SkillFactory().CreatePassiveSkill(instances.Last().SkillBase.Key, unit, instances.Last());
        Skills.Add(passive);
    }
    
    public IEnumerator ActivateSKill(int skillIndex, List<Unit> targets, List<int> damages)
    {
        ActiveSkill curSkill = ActiveSkills[skillIndex];
        Quaternion prevRotation = transform.rotation;
        
        unit.GaugeSystem.GetGauge<ReMolding>().Use(ActiveSkills[skillIndex].Data.Cost);
        // 스킬별 애니메이션 실행 파트

        // Action action = () => curSkill.ActionHandler.ActivateSkillActions(curSkill, targets, damages);
        // animEvent.OnAnimationEventTriggered += action;
        animEvent.OnAnimationEventTriggered += curSkill.ActivateEffect;
        
        animation.SetAttack(skillIndex);
        
        yield return curSkill.Activate(targets, damages);
        
        transform.rotation = prevRotation;
        animation.ReturnAttack();
        
        curSkill.ActivatePostAction();
        animEvent.OnAnimationEventTriggered -= curSkill.ActivateEffect;
        // animEvent.OnAnimationEventTriggered -= action;
    }

    public Skill GetSkill(int skillIndex)
    {
        return Skills[skillIndex];
    }
    
    
    public override bool Ready(int opt, bool isReverted = false)
    {
        // 이때 들어오는 opt는 스킬 인덱스임
        Option = opt;
        
        CommandCoord = unit.curCoord;
        
        bool isSwitched = SwitchReceiver(ActiveSkills[opt]);
        
        if(!isSwitched)
            SwitchReceiver(ActiveSkills[0]); // 선택한 스킬을 못쓰는 경우 (리몰딩 지수 부족 등등) 일반 스킬로 대체
        
        return isSwitched;
    }
    
    public override bool SwitchReceiver(ICommandReceiver newReceiver)
    {
        Clear();
        
        curReceiver = newReceiver; 
        
        return curReceiver.Ready((int)PlayerCommandType.UseSkill, false); // 선택한 스킬의 인디케이터 표시
    }
}