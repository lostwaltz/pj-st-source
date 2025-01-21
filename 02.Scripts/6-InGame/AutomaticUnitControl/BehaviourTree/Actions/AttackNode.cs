using System.Collections.Generic;
using UnityEngine;

namespace UnitBT
{
    public class AttackNode : IBehaviourNode
    {
        private readonly int skillIndex;

        public AttackNode(int skillIdx = 0)  // 기본값은 기본공격(0번)
        {
            skillIndex = skillIdx;
        }
        public BehaviourStatus Execute()
        {
            BehaviourContext context = AutomaticUnitController.Context;

            List<Unit> targets = new();
            List<int> damages = new List<int>();

            // ActiveSkill skill = context.Subject.SkillSystem.ActiveSkills[skillIdx];
            // Vector2 attackCoord = context.AttackCoord;

            // Debug.Log($"context target : {context.Target}, coord { context.Target.curCoord }");
            // skill.Selector.Select(skill, ref attackCoord, ref context.Target.curCoord, ref targets);

            targets.Add(context.Target);
            DamageCalculator.CalculateDamage(context.Subject, skillIndex, context.AttackCoord, ref targets, out damages);

            // 다음 사용 가능한 명령 인덱스 찾기
            int nextCommandIndex = context.Subject.CommandSystem.commands.Count;

            // 공격 명령 업데이트
            context.Subject.CommandSystem.UpdateCommand(nextCommandIndex, context.AttackCoord, () =>
            {
                return new AttackCommand(context.Subject, skillIndex, targets, damages);
            });

            return BehaviourStatus.Success;
        }
    }
}