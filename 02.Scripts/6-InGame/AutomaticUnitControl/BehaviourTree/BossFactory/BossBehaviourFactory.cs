using UnitBT;
using UnityEngine;
using UnitBT.Actions;


public static class BossBehaviourFactory
{

    public static IBehaviourNode CreateBehaviour()
    {
        BehaviourContext context = AutomaticUnitController.Context;

        IBehaviourNode pattern1 = new SequenceNode(
            new FindTargetNode(),
             new AvoidObstacleMovementNode(),
             new CheckAttackRangeNode(new AttackNode()),
                new BossSkillWarningNode(1)


        );

        IBehaviourNode pattern2 = new SequenceNode(
             new BossAttackNode()




        );

        return new BossPatternNode(pattern1, pattern2);
    }
}