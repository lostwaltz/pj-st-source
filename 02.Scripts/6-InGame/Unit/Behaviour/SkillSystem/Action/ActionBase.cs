using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActionSkill
{
    public void ActivateAction(ActiveSkill skill, List<Unit> targets, List<int> damages);
}

public abstract class ActionBase : MonoBehaviour, IActionSkill
{
    public ActionHandler handler;
    
    public abstract void ActivateAction(ActiveSkill skill, List<Unit> targets, List<int> damages);
    
    public abstract void UndoAction();

    public abstract void ActivatePostAction(ActiveSkill skill);
}
