using System;
using JetBrains.Annotations;
using UnityEngine;

public abstract class BehaviorBase : IBehaviorExecutable
{
    protected Action Action;
    
    public abstract BehaviorBase BehaviorExecute(Transform target, float duration);

    public BehaviorBase OnComplete(Action callback)
    {
        Action -= callback;
        Action += callback;

        return this;
    }
}
