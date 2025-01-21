using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeConditionHandler
{
    public ChallengeConditionHandler(List<GameConditionPredicate> conditions)
    {
        challengeConditionList = new List<GameConditionPredicate>(conditions);
    }

    private readonly List<GameConditionPredicate> challengeConditionList;

    public List<GameConditionPredicate> GetListClearConditions()
    {
        List<GameConditionPredicate> result = new();
        
        foreach (var conditionPredicate in challengeConditionList)
        {
            if(conditionPredicate.IsConditionMet())
                result.Add(conditionPredicate);
        }
        
        return result;
    }
}
