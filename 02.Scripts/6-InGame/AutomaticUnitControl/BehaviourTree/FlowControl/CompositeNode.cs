using System.Collections.Generic;

namespace UnitBT
{
    public abstract class CompositeNode : IBehaviourNode
    {
        protected readonly List<IBehaviourNode> children = new();

        public void AddChild(IBehaviourNode node)
        {
            children.Add(node);
        }

        public abstract BehaviourStatus Execute();
    }
}