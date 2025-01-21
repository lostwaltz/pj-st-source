using System.Collections.Generic;
using UnityEngine;

namespace CustomStageTool
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
    public class StageDataTool : SingletonForTool<StageDataTool>
    {
        StagePlaceTool placeTool => StagePlaceTool.Instance;
        
        public void ReadCell(StageSO file, Transform cellParent, Vector2Int unit)
        {
            file.cellPlacements = new Placement[cellParent.childCount];

            for (int i = 0; i < cellParent.childCount; i++)
            {
                StageCell cell = cellParent.GetChild(i).GetComponent<StageCell>();
            
                Placement placement = new Placement()
                {
                    id = 0,
                    cost = 1, // 길은 비용 1로 고정
                    coord = ConvertToCoord(cell.transform.position, unit), //new Vector2(cell.transform.position.x / unit.x - 0.5f, cell.transform.position.z / unit.y - 0.5f),
                    position = cell.transform.position
                };
            
                file.cellPlacements[i] = placement;
            }
        }
        
        public void ReadObstacles(StageSO file, Transform obstacleParent, Vector2Int unit)
        {
            file.obstaclePlacements = new Placement[obstacleParent.childCount];
            file.obstacleAreas = new PlacementArea[obstacleParent.childCount];

            for (int i = 0; i < obstacleParent.childCount; i++)
            {
                int index = i;
                StageObstacle obstacle = obstacleParent.GetChild(i).GetComponent<StageObstacle>();

                Placement placement = new Placement()
                {
                    // 장애물 기본 정보 반영
                    id = obstacle.placement.id,
                    cost = obstacle.placement.cost,

                    coord = ConvertToCoord(obstacle.transform.position, unit),
                    position = obstacle.transform.position,
                    rotation = obstacle.transform.rotation
                };
                
                var occupied = placeTool.GetOccupied(placement.position);
                List<Vector2> occupiedCoord = new List<Vector2>();
                for (int j = 0; j < occupied.Count; j++)
                {
                    occupiedCoord.Add(ConvertToCoord(occupied[j], unit));
                }

                PlacementArea area = new PlacementArea()
                {
                    obstacleIndex = index,
                    areaCoord = occupiedCoord
                };

                file.obstaclePlacements[i] = placement;
                file.obstacleAreas[i] = area;
            }
        }

        Vector2 ConvertToCoord(Vector3 position, Vector2Int unit)
        {
            return new Vector2(position.x / unit.x - 0.5f, position.z / unit.y - 0.5f);
        }
    }
    
#endif
}


