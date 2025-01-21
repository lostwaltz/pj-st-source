using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;


namespace SO
{
    [CreateAssetMenu(fileName = "BehaviorFactory", menuName = "SOs/BehaviorFactory/ScaleTo")]
    public class ScaleToFactory : BehaviorFactory
    {
        public Vector3 StartScale;
        public Vector3 EndScale;
        public Ease Ease;

        public bool FreezeX;
        public bool FreezeY;

        public bool IgnoreStartValue;

        public bool turnOffOnEnd;
        
        public override BehaviorBase CreateBehaviorEffects()
        {
            return new ScaleTo(StartScale, EndScale, Ease, FreezeX, FreezeY, IgnoreStartValue, turnOffOnEnd);
        }
    }
}