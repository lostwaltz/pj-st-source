using UnityEngine;

public abstract class CommandSwitcher : MonoBehaviour, ICommandReceiver
{
    public int Option { get; set; }
    public Vector2 CommandCoord { get; set; }
    public ReceiverStep ReceiverStep => curReceiver.ReceiverStep;
    
    
    protected ICommandReceiver curReceiver;
    public abstract bool SwitchReceiver(ICommandReceiver newReceiver);

    public abstract bool Ready(int opt, bool isReverted);

    public virtual void Interact()
    {
        curReceiver?.Interact();
    }

    public virtual void Revert()
    {
        curReceiver?.Revert();
    }

    public virtual void Clear()
    {
        curReceiver?.Clear();
    }

    public virtual IUnitCommand CreateCommand()
    {
        return curReceiver?.CreateCommand();
    }
}