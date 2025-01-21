using UnityEngine;

public abstract class StageComponent : MonoBehaviour, IClickable
{
    public Placement placement;
    
    public virtual void Initialize(Placement newPlacement)
    {
        placement = newPlacement;
        
        transform.position = placement.position;
        transform.rotation = placement.rotation;
    }

    public virtual string GetInfo()
    {
        // TODO : Undo 기능 구현에 쓸 unit의 저장 정보 Json으로 만들어줄 메서드
        return "";
    }
}