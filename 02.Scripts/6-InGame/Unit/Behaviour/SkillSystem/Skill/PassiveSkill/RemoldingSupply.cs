using System.Collections;
using System.Collections.Generic;
using EnumTypes;
using Structs;
using UnityEngine;

public class RemoldingSupply : PassiveSkill
{
    public RemoldingSupply(Unit owner, SkillInstance inst) : base(owner, inst)
    {
        Core.EventManager.Subscribe<TurnEnterCommandEvent>(SupplyRemolding);
        
        OwnerUnit.HealthSystem.OnDead += () => Core.EventManager.Unsubscribe<TurnEnterCommandEvent>(SupplyRemolding);
        OwnerUnit.OnReleaseUnit += () => Core.EventManager.Unsubscribe<TurnEnterCommandEvent>(SupplyRemolding);

    }

    public void SupplyRemolding(TurnEnterCommandEvent turnEvent)
    {
        switch (turnEvent.GamePhase)
        {
            case GamePhase.PlayerTurn when OwnerUnit.type != UnitType.PlayableUnit:
            case GamePhase.EnemyTurn when OwnerUnit.type != UnitType.EnemyUnit:
                return;
            default:
                turnEvent.CommandQueue.Enqueue(this);
                break;
        }
    }
    
    public override IEnumerator Execute()
    {
        var damages = new List<int>();
        var context = new EffectContext(EffectContext.StartPos, EffectContext.StartRot, EffectContext.TargetTransform,
            EffectContext.MultiTarget, damages,
            Data.SkillBase.AttackCount, Data.SkillBase.HitPerAttack, Data.SkillBase.AttackMethod);
        
        EffectHandler.Play(OwnerUnit, context);
        
        OwnerUnit.GaugeSystem.Charge<ReMolding>(1);
        
        yield return base.Execute();
    }

    // TODO: 언두기능 개발 
    // public override void Undo()
    // {
    //     OwnerUnit.GaugeSystem.Use<ReMolding>(1);
    // }
}