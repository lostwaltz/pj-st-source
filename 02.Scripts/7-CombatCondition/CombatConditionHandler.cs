 using System;
 using System.Collections.Generic;
using System.Linq;

public class CombatConditionHandler
{
    private List<GameConditionPredicate> conditions;
    private List<GameConditionPredicate> loseConditions;

    public string CurSimpleDesc;
    
    public void InitConditions(List<GameConditionPredicate> win, List<GameConditionPredicate> lose)
    {
        conditions = new List<GameConditionPredicate>(win);
        loseConditions = new List<GameConditionPredicate>(lose);
    }

    public bool WinConditionsMet()
    {
        foreach (var predicate in conditions)
        {
            if (false == predicate.IsConditionMet())
                return false;
        } 
        return true;
    }
    
    public bool LoseConditionsMet()
    {
        foreach (var predicate in loseConditions)
        {
            if (predicate.IsConditionMet())
            {
                CurSimpleDesc = predicate.SimpleDescription;
                
                return true;
            }
        } 
        return false;
    }
}