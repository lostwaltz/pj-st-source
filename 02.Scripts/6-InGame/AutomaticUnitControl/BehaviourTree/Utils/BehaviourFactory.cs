using System;

namespace UnitBT
{
    //SimpleFactory
    public static class BehaviourFactory
    {
        public static IBehaviourNode CreateBehaviour(BehaviourType type)
        {
            return type switch
            {
                BehaviourType.Naive => new SequenceNode(
                    new FindTargetNode(),
                    new MoveRandomlyNode(),
                    new CheckAttackRangeNode(new AttackNode())
                ),
                
                BehaviourType.General => new SequenceNode(
                    new FindTargetNode(),
                    new SequenceNode(
                        new TacticalMovementNode(),
                        new CheckAttackRangeNode(new AttackNode()))
                ),
                
                BehaviourType.Support => new CheckConditionNode(unit =>
                    {
                        Unit target = GameUnitManager.Instance.CheckHealth(unit.type, unit);
                        return target != null;
                    },
                    new SequenceNode(
                        new FindLowHealthTeamNode(),
                        new TacticalMovementNode(),
                        new CheckAttackRangeNode(new HealNode(1))
                        ),
                    
                    new SequenceNode(
                        new FindTargetNode(),
                        new SelectorNode(
                            new CheckAttackRangeNode(new AttackNode()),
                            new SequenceNode(
                                new TacticalMovementNode(),
                                new CheckAttackRangeNode(new AttackNode())
                            )
                        )
                    )
                ),
                
                BehaviourType.Boss => new BossPatternNode(
                    new SequenceNode(new FindTargetNode(), new MoveToOptimalNode(), new AttackNode(1), new AttackNode(2)),
                    new SequenceNode(new FindTargetNode(), new MoveToOptimalNode(), new AttackNode(1), new AttackNode(3))
                ),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}