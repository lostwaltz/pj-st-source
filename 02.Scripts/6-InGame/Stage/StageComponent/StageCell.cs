
using EnumTypes;
using UnityEngine;

public class StageCell : StageComponent
{
    // public float size = 2f;
    public int unitIndexInCell;
    public UnitType unitTypeInCell;
    
    public int Cost => obstacleCost < 0 ? placement.cost * obstacleCost : placement.cost + obstacleCost;
    int obstacleCost = 0;

    public override void Initialize(Placement newPlacement)
    {
        base.Initialize(newPlacement);

        UnitExit();
        
        gameObject.name = $"({placement.coord.x}, {placement.coord.y})";
        gameObject.SetActive(false);
    }

    public void AdjustObstacleCost(int value)
    {
        /* 비용 처리
         * 
         * value : -1 인 경우 지나가지 못하는 것을 표현
         * 기본 코스트에 곱해서 음수로 만들 것임 -> 경로 탐색 과정에서 음수 코스트가 나오면 제외할 것.
         * value > 0 인 경우 비용 더하기
         */
        obstacleCost = value;
    }

    public void UnitEnter(Unit unit)
    {
        unitIndexInCell = unit.index;
        unitTypeInCell = unit.type;
    }

    public void UnitExit()
    {
        unitIndexInCell = -1;
    }
}
