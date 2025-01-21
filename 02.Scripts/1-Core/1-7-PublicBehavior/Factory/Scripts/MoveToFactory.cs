using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;


namespace SO
{
    [CreateAssetMenu(fileName = "BehaviorFactory", menuName = "SOs/BehaviorFactory/MoveTo")]
    public class MoveToFactory : BehaviorFactory
    {
        public Vector3 StartPos;
        public Vector3 EndPos;
        public Ease Ease;

        public bool FreezeX;
        public bool FreezeY;

        public bool IgnoreStartValue;

        public bool turnOffOnEnd;
        
        public override BehaviorBase CreateBehaviorEffects()
        {
            return new MoveTo(StartPos, EndPos, Ease, FreezeX, FreezeY, IgnoreStartValue, turnOffOnEnd);
        }
    }
}