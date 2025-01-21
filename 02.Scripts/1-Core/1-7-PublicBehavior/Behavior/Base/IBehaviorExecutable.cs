using System;
using DG.Tweening;
using UnityEngine;

public interface IBehaviorExecutable
{
    public BehaviorBase BehaviorExecute(Transform target, float duration);
}
