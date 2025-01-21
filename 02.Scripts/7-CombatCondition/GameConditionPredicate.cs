
using UnityEngine;

public abstract class GameConditionPredicate : ScriptableObject
{
    protected UIStageGoalPanel UIStageGoalPanel;
    
    public string SimpleDescription;
    
    public virtual void Init(UIStageGoalPanel uiStageGoalPanel)
    {
        this.UIStageGoalPanel = uiStageGoalPanel;
    }
    
    public abstract bool IsConditionMet();
    public abstract string Description();
}
