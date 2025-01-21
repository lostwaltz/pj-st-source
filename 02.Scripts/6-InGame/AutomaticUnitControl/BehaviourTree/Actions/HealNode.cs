using System.Collections.Generic;
using UnityEngine;

namespace UnitBT
{
    public class HealNode : IBehaviourNode
    {
        private readonly int skillIndex;

        public HealNode(int skillIdx = 0)  // 기본값은 기본공격(0번)
        {
            skillIndex = skillIdx;
        }
        public BehaviourStatus Execute()
        {
            BehaviourContext context = AutomaticUnitController.Context;

            List<Unit> targets = new();
            List<int> damages = new List<int>();

            targets.Add(context.Target);
            DamageCalculator.CalculateDamage(context.Subject, skillIndex, context.AttackCoord, ref targets, out damages);
            
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