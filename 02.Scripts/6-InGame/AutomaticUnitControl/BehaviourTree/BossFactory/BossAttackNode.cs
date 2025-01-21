using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

namespace UnitBT
{
    /// <summary>
    /// 보스의 스킬 공격을 담당하는 노드입니다.
    /// 경고 표시된 위치에 스킬을 사용합니다.
    /// </summary>
    public class BossAttackNode : IBehaviourNode
    {
        public BehaviourStatus Execute()
        {
            BehaviourContext context = AutomaticUnitController.Context;

            // 저장된 스킬 인덱스 가져오기
            int skillIndex = context.RetrieveAndClearSkillIndex();
            if (skillIndex == -1)
            {
                Debug.LogWarning("BossAttackNode: No stored skill index found");
                return BehaviourStatus.Failure;
            }

            // 경고 영역 체크
            if (context.WarningArea == null || context.WarningArea.Count == 0)
            {
                Debug.LogError("BossAttackNode: Warning area is empty");
                return BehaviourStatus.Failure;
            }

            List<Unit> targets = new();
            List<int> damages = new List<int>();

            // 경고 영역 내의 모든 유닛을 타겟으로 설정
            var playerUnits = GameUnitManager.Instance.Units[EnumTypes.UnitType.PlayableUnit];
            foreach (var unit in playerUnits)
            {
                if (context.WarningArea.Contains(unit.curCoord))
                {
                    targets.Add(unit);
                }
            }

            Debug.Log($"Found {targets.Count} targets in warning area");

            Vector2 attackPoint = context.WarningArea[0];

            if (targets.Count > 0)
            {
                DamageCalculator.CalculateDamage(context.Subject, skillIndex, attackPoint, ref targets, out damages);
                // context.Subject.CommandSystem.UpdateCommand(0, attackPoint, () =>
                // {
                //     return new AttackCommand(context.Subject, skillIndex, targets, damages);
                // });
            }
            else
            {
                Debug.Log("타겟없음");
                // 더미 유닛 생성
                // var enemyPrefab = Resources.Load<GameObject>("Prefabs/Units/EnemyUnitPrefab");
                // var info = Core.DataManager.UnitTable.GetByKey(100200);
                // var inst = new UnitInstance(info, 1);
                // var dummy = GameObject.Instantiate(enemyPrefab).GetComponent<EnemyUnit>();
                // dummy.Init(0, attackPoint, inst);
                // dummy.transform.position = new Vector3(attackPoint.x, attackPoint.y, 0);
                //
                // targets = new List<Unit> { dummy };
                // damages = new List<int> { 0 };
            }
            
            context.Subject.CommandSystem.UpdateCommand(0, attackPoint, () =>
            {
                return new AttackCommand(context.Subject, skillIndex, targets, damages);
            });

            // 경고 영역 인디케이터 제거
            // var indicator = GameManager.Instance.Indicator;
            // indicator.HideAttackRelatedIndicator();
            GameManager.Instance.Indicator.HideEnemyRelated();

            // 경고 영역 초기화
            context.WarningArea.Clear();

            return BehaviourStatus.Success;
        }
    }
}