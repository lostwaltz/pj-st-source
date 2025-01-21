using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Structs
{
    public struct CommandContext
    {
        [CanBeNull] public Unit TargetUnit;
    }
}
