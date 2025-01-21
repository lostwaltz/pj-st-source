using UnityEngine;

public enum ReceiverStep
{
    Ready = 0,
    Determine = 1,
    None = -1,
}

public interface ICommandReceiver
{
    public int Option { get; set; }
    public Vector2 CommandCoord { get; set; }
    public ReceiverStep ReceiverStep { get; }
    
    public bool Ready(int opt, bool isReverted);
    public void Interact();
    public void Revert();
    public void Clear();
    public IUnitCommand CreateCommand();
}