using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EnumTypes;

public class SupportFire : PassiveSkill
{
    private ExecuteCommandEvent commandEvent;

    private List<Unit> targetList;
    private List<int> damageList;

    public Unit GetAttackTarget()
    {
        if (targetList == null)
        {
            return null;
        }
        else if (targetList.Count == 0)
        {
            return null;
        }

        return targetList.First();
    }

    public SupportFire(Unit owner, SkillInstance inst) : base(owner, inst)
    {
        Core.EventManager.Subscribe<ExecuteCommandEvent>(SupportFireTarget);
        OwnerUnit.HealthSystem.OnDead += () => Core.EventManager.Unsubscribe<ExecuteCommandEvent>(SupportFireTarget);
        OwnerUnit.OnReleaseUnit += () => Core.EventManager.Unsubscribe<ExecuteCommandEvent>(SupportFireTarget);
    }

    public SupportFire(Unit owner, SkillInstance inst, ExecuteCommandEvent commandEvent) : base(owner, inst)
    {
        this.commandEvent = commandEvent;
    }


    public void SupportFireTarget(ExecuteCommandEvent commandEvent)
    {
        // 발동자가 다른 유닛 타입이면
        if (OwnerUnit.type != commandEvent.Command.GetUnit().type) return;

        if (commandEvent.Command.GetContext().TargetUnit != null &&
            OwnerUnit.type == commandEvent.Command.GetContext().TargetUnit?.type) return;

        // 발동자가 자기 자신이라면
        if (OwnerUnit == commandEvent.Command.GetUnit()) return;
        // 타겟이 없다면
        if (commandEvent.Command.GetContext().TargetUnit == null) return;
        if (commandEvent.CommandPhase != CommandPhase.BeforeCommand) return;

        if (UnitType.EnemyUnit == OwnerUnit.type) return;

        if (OwnerUnit == commandEvent.Command.GetUnit()) return;

        Unit target = commandEvent.Command.GetContext().TargetUnit;

        if (target == null) return;

        if (5 < ((OwnerUnit.curCoord.y - target.curCoord.y).Abs()) +
            (OwnerUnit.curCoord.x - target.curCoord.x).Abs()) return;

        List<Unit> targets = new() { target };
        DamageCalculator.CalculateDamage(OwnerUnit, 4, OwnerUnit.curCoord, ref targets, out List<int> damages);
        SupportFire supportFire = new SupportFire(OwnerUnit, Data, commandEvent)
        {
            targetList = targets,
            damageList = damages
        };

        commandEvent.PushCommandList.Add(supportFire);
    }

    public override IEnumerator Execute()
    {
        yield return OwnerUnit.SkillSystem.ActivateSKill(0, targetList, damageList);

        commandEvent = null;
        yield return null;
    }

    // TODO: 언두기능 개발
    // public override void Undo()
    // {
    //     for (int i = 0; i < targetList.Count; i++)
    //     {
    //         targetList[i].HealthSystem.RecoverHealth(damageList[i], true);
    //     }
    // }
}