using System;
using EnumTypes;

public class InteractionMediator
{
    public event Action<int> OnSkillSelected; // 스킬 선택
    public event Action<IClickable> OnSkillInteracted; // 선택한 스킬로 대상 선택
    public event Action OnRevertCalled;
    public event Action<PlayerCommandType, ReceiverStep> OnAfterReverted; // 명령 번복

    public bool BlockInteraction;
    
    public void CallSkillSelect(int skillId)
    {
        if(BlockInteraction) return;
        
        OnSkillSelected?.Invoke(skillId);
    }

    public void CallSkillInteracted(IClickable thing)
    {
        if(BlockInteraction) return;
        
        OnSkillInteracted?.Invoke(thing);
    }

    public void CallRevert()
    {
        if(BlockInteraction) return;
        
        OnRevertCalled?.Invoke();
    }

    public void CallAfterReverted(PlayerCommandType commandType, ReceiverStep commandStep)
    {
        if(BlockInteraction) return;
        
        OnAfterReverted?.Invoke(commandType, commandStep);
    }
}