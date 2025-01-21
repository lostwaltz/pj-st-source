using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "BehaviorFactory", menuName = "SOs/BehaviorFactory/FadeTo")]
    public class FadeToFactory : BehaviorFactory
    {
        public float startAlpha;
        public float endAlpha;
        public Ease ease;

        public bool ignoreStartValue;
        public bool turnOffOnEnd;
        public bool turnOffOnDestroy;
        
        public override BehaviorBase CreateBehaviorEffects()
        {
            return new FadeTo(startAlpha, endAlpha, ease, ignoreStartValue, turnOffOnEnd, turnOffOnDestroy);
        }
    }
}
