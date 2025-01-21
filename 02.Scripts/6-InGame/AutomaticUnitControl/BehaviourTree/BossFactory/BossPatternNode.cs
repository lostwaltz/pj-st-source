using UnityEngine;

namespace UnitBT
{
    public class BossPatternNode : IBehaviourNode
    {
        private static bool useFirstPattern = true;
        private readonly IBehaviourNode pattern1;
        private readonly IBehaviourNode pattern2;

        public BossPatternNode(IBehaviourNode firstPattern, IBehaviourNode secondPattern)
        {
            pattern1 = firstPattern;
            pattern2 = secondPattern;
        }

        public BehaviourStatus Execute()
        {
            Debug.Log($"Executing Boss Pattern: {(useFirstPattern ? "Pattern 1" : "Pattern 2")}");
            var result = useFirstPattern ? pattern1.Execute() : pattern2.Execute();
            useFirstPattern = !useFirstPattern; // 패턴 전환
            Debug.Log($"Boss Pattern Execution Result: {result}");
            return result;
        }
    }
}