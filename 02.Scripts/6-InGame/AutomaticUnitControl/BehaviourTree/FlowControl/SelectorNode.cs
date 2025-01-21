using System.Collections.Generic;

namespace UnitBT
{
    public class SelectorNode : CompositeNode
    {
        public SelectorNode(params IBehaviourNode[] child)
        {
            children.AddRange(child);
        }
        
        public override BehaviourStatus Execute()
        {
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].Execute() == BehaviourStatus.Success)
                    return BehaviourStatus.Success;
            }
            
            return BehaviourStatus.Failure;
        }
    }   
}