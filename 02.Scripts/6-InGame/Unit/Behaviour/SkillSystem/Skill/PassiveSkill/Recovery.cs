using System.Collections;
using System.Collections.Generic;
using EnumTypes;
using Unity.VisualScripting;
using UnityEngine;

public class Recovery : PassiveSkill
{
    private int amount;
    
    public Recovery(Unit owner, SkillInstance inst) : base(owner, inst)
    {
        Core.EventManager.Subscribe<TurnEnterCommandEvent>(HealthRecovery);
        
        OwnerUnit.HealthSystem.OnDead += () => Core.EventManager.Unsubscribe<TurnEnterCommandEvent>(HealthRecovery);
        OwnerUnit.OnReleaseUnit += () => Core.EventManager.Unsubscribe<TurnEnterCommandEvent>(HealthRecovery);

        WaitForSeconds = new WaitForSeconds(1f);
    }

    private void HealthRecovery(TurnEnterCommandEvent turnEvent)
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
        var context = new EffectContext(OwnerUnit.transform.position, Quaternion.identity, EffectContext.TargetTransform, EffectContext.MultiTarget, damages,
            Data.SkillBase.AttackCount, Data.SkillBase.HitPerAttack, Data.SkillBase.AttackMethod);
        EffectHandler.Play(OwnerUnit, context);

        amount = (int)(OwnerUnit.StatsSystem.Stats.Health * Data.SkillPower);
        amount = OwnerUnit.HealthSystem.RecoverHealth(amount);
            
        yield return base.Execute();
    }

    // TODO: 언두기능 개발
    // public override void Undo()
    // {
    //     OwnerUnit.HealthSystem.Health = OwnerUnit.HealthSystem.Health -= amount;
    // }
}