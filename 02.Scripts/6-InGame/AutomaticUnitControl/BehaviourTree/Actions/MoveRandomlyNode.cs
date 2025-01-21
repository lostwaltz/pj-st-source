using UnityEngine;

namespace UnitBT
{
    public class MoveRandomlyNode : IBehaviourNode
    {
        int maxCalcCount = 100;
        
        public BehaviourStatus Execute()
        {
            Debug.Log("Move Randomly");
            
            BehaviourContext context = AutomaticUnitController.Context;
            Vector2 coord = GetRandomCoord(context.Subject, out bool isMoving);
            if (isMoving)
            {
                context.AttackCoord = coord; // 이동 후 좌표가 1차 공격 개시 좌표
                StageCell destination = StageManager.Instance.cellMaps[coord];
                context.Subject.CommandSystem.UpdateCommand(0, coord, () =>
                {
                    return new MoveCommand(context.Subject, destination);
                });
            }
            else
            {
                context.Subject.CommandSystem.UpdateCommand(0, coord, null);
            }
            
            return BehaviourStatus.Success;
        }
        
        
        Vector2 GetRandomCoord(Unit subject, out bool isMoving)
        {
            int count = 0;
            isMoving = true;
        
            Vector2 result;
            do
            {
                if (count >= maxCalcCount)
                {
                    // 무한루프 방지 : 제한 횟수 이상이 되면 아예 움직이지 않도록 수정
                    isMoving = false;
                    result = subject.curCoord;
                    break;
                }
            
                count++;
            
                result = subject.curCoord;
                result.x += Random.Range(0, subject.data.UnitBase.StepRange) * Random.Range(0,10) > 4 ? 1 : - 1;
                result.y += Random.Range(0, subject.data.UnitBase.StepRange) * Random.Range(0,10) > 4 ? 1 : - 1;
            
            } while (!StageManager.Instance.cellMaps.ContainsKey(result) ||
                     StageManager.Instance.obstaclesMaps.ContainsKey(result) ||
                     StageManager.Instance.cellMaps[result].unitIndexInCell >= 0 ||
                     StageManager.Instance.cellMaps[result].Cost < 0);
        
            return result;
        }
    }
}