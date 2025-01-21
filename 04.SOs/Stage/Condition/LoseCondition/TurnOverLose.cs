using UnityEngine;

[CreateAssetMenu(fileName = "TurnOverLose", menuName = "GameConditionSO/TurnOverLose")]
public class TurnOverLose : GameConditionPredicate
{
    public int MaxTurn;
    
    public override bool IsConditionMet() => false; // TODO: 현재 턴 MaxTurn 비교 필요

    public override string Description()
    {
        var turnCount = 0f;
        return $"<color=#FFFFFF>{turnCount} / {MaxTurn}</color> 턴 이내 클리어.";
    }
}