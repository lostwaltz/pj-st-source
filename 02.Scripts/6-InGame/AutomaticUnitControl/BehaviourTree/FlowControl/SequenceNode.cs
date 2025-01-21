using System.Collections.Generic;
using UnityEngine;

namespace UnitBT
{
    public class SequenceNode : CompositeNode
    {
        public SequenceNode(params IBehaviourNode[] child)
        {
            children.AddRange(child);
        }
        
        public override BehaviourStatus Execute()
        {
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].Execute() == BehaviourStatus.Failure)
                    return BehaviourStatus.Failure;
            }
            
            return BehaviourStatus.Success;
        }
    }
}