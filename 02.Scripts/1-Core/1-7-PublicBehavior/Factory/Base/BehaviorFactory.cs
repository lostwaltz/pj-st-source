using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorFactory : ScriptableObject
{
    public abstract BehaviorBase CreateBehaviorEffects();
}