using EnumTypes;
using UnityEngine;

public class PlayerUnit : Unit
{
    public override void Init(int unitIndex, Vector2 initCoord, Vector3 initRotate, UnitInstance instance)
    {
        type = UnitType.PlayableUnit;
        base.Init(unitIndex, initCoord, initRotate, instance);
    }

    public override string GetInfo()
    {
        return base.GetInfo();
    }
}
