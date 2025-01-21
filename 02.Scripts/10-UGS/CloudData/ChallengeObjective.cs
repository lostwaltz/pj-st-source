using System;
using System.Collections.Generic;

namespace UGS
{
    [Serializable]
    public class ChallengeObjective : DataModel
    {
        public bool isCompleted;          // 완료 여부
        public int StageKey;              // 스테이지 키
        public int goalKey;               // 목표 키

        public ChallengeObjective()
        {
            isCompleted = false;
            StageKey = 0;
            goalKey = 0;
            Key = GenerateKey(StageKey, goalKey);
        }

        public ChallengeObjective(int stageKey, int goalKey)
        {
            isCompleted = false;
            StageKey = stageKey;
            this.goalKey = goalKey;
            Key = GenerateKey(stageKey, goalKey);
        }

        private int GenerateKey(int stageKey, int goalKey)
        {
            // stageKey를 1000배 하여 3자리 공간을 확보하고 goalKey를 더함
            // 예: stageKey=1, goalKey=2 -> Key=1002
            return (stageKey + 1) * 1000 + goalKey;
        }

        public void Complete()
        {
            isCompleted = true;
        }
    }
}