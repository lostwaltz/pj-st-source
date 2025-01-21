using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Structs;

namespace UnitBT.Actions
{
    /// <summary>
    /// 보스의 스킬 경고 영역을 표시하는 노드입니다.
    /// </summary>
    public class BossSkillWarningNode : IBehaviourNode, IUnitCommand
    {
        private readonly int skillIndex;
        private Unit subject;
        private BehaviourContext context;

        public BossSkillWarningNode(int skillIndex)
        {
            this.skillIndex = skillIndex;
            this.context = AutomaticUnitController.Context;
            this.subject = context.Subject;
        }

        public Unit GetUnit()
        {
            return subject;
        }

        public CommandContext GetContext()
        {
            if (context == null || context.Target == null)
            {
                context = AutomaticUnitController.Context;
            }

            var commandContext = new CommandContext
            {
                TargetUnit = context.Target
            };

            return commandContext;
        }

        IEnumerator IUnitCommand.Execute()
        {
            yield return new WaitForSeconds(1f);

            if (context == null || context.Target == null)
            {
                context = AutomaticUnitController.Context;
            }

            // 스킬 정보 저장 왜 두번
            var skill = subject.SkillSystem.ActiveSkills[skillIndex];
            if (skill == null)
            {
                Debug.LogWarning($"BossSkillWarningNode: Skill {skillIndex} is not available");
                yield break;
            }

            context.StoreSkillIndex(skillIndex);
            context.CurrentSkillScope = skill.Data.Scope;

            var maps = StageManager.Instance.cellMaps;
            Vector2 targetPos = context.Target.curCoord;
            List<Vector2> warningArea = new List<Vector2>();

            // 스킬의 Scope에 따라 경고 영역 계산
            StageManager.Interaction.GetNearCoord(targetPos, out warningArea, skill.Data.Scope);

            // 경고 영역 좌표 저장
            context.WarningArea = new List<Vector2>(warningArea);

            foreach (Vector2 coord in warningArea)
            {
                if (!maps.ContainsKey(coord)) continue;

                // 경고 인디케이터 표시
                // GameManager.Instance.Indicator.Show<IndicatorSelectedCell>(coord, IndicatorOption.Multiple);
                GameManager.Instance.Indicator.GetInEnemy<IndicatorSelectedCell>().Show(coord);
            }

            // 공격 방향 인디케이터 표시
            GameManager.Instance.Indicator.ShowAttackIndicator(subject.curCoord, targetPos);

            yield return new WaitForSeconds(1f);


        }

        public BehaviourStatus Execute()
        {
            if (context == null) context = AutomaticUnitController.Context;
            subject = context.Subject;

            var skill = subject.SkillSystem.ActiveSkills[skillIndex];


            context.StoreSkillIndex(skillIndex);
            context.CurrentSkillScope = skill.Data.Scope;

            context.Subject.CommandSystem.UpdateCommand(3, context.AttackCoord, () =>
            {
                return this;
            });

            return BehaviourStatus.Success;
        }

        // TODO: 언두 기능 개발 
        // public void Undo()
        // {
        //     // 이게 실제로 해당 행동 번복용
        // }
    }
}
