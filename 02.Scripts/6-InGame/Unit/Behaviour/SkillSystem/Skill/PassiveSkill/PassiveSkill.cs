using System.Collections;
using System.Collections.Generic;
using Structs;

public abstract class PassiveSkill : Skill, IUnitCommand
{
    public Unit GetUnit()
    {
        return OwnerUnit;
    }

    public virtual CommandContext GetContext()
    {
        return new CommandContext();
    }
    
    protected PassiveSkill(Unit owner, SkillInstance inst) : base(owner, inst)
    {
        Type = SkillType.Passive;
        EffectContext = new EffectContext(null, null, null, null, null,
            inst.SkillBase.AttackCount, inst.SkillBase.HitPerAttack, inst.SkillBase.AttackMethod);
    }

    public virtual IEnumerator Execute()
    {
        yield return WaitForSeconds;
    }

    //TODO: 언두 기능 개발
    //public abstract void Undo();
}