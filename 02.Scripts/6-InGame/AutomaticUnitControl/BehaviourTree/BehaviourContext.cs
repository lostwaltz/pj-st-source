using System.Collections.Generic;
using UnityEngine;

namespace UnitBT
{
    public class BehaviourContext
    {
        public Unit Subject { get; set; }
        public Unit Target { get; set; }
        public Vector2 AttackCoord { get; set; }
        public List<Vector2> StoredTargetPosition { get; set; } = new List<Vector2>();

        // 스킬 관련 정보 추가
        public int CurrentSkillIndex { get; private set; } = -1;
        public float CurrentSkillScope { get; set; }
        public List<Vector2> WarningArea { get; set; } = new List<Vector2>();


        public void StoreSkillIndex(int skillIndex)
        {
            CurrentSkillIndex = skillIndex;
            Debug.Log($"보스 경고 영역 스킬 인덱스 저장: {skillIndex}");
        }

        public int RetrieveAndClearSkillIndex()
        {
            int index = CurrentSkillIndex;
            CurrentSkillIndex = -1;
            Debug.Log($"보스 경고 영역 스킬 인덱스 검색 및 초기화: {index}");
            return index;
        }

        public bool HasStoredSkill => CurrentSkillIndex != -1;
    }
}