using System;
using UnityEngine;

namespace CustomStageTool
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
    public class StageGridTool : SingletonForTool<StageGridTool>
    {
        [SerializeField] Color gridColor = new Color(0.1f, 0.1f, 0.1f, 0.5f);
        public float GridHeight = 0.0f;
        public Vector2Int UnitSize = new Vector2Int(2, 2);
        [SerializeField] Vector2Int maxSize = new Vector2Int(1000, 1000);
     
        
        private void OnDrawGizmos()
        {
            DrawGrid();
        }

        void DrawGrid()
        {
            Vector3 pos = Vector3.zero;
            Vector2 halfSize = new Vector2(maxSize.x * 0.5f, maxSize.y * 0.5f);

            Gizmos.color = gridColor;
            // draw x
            for (float x = pos.x - halfSize.x; x < pos.x + halfSize.x; x += UnitSize.x)
            {
                Vector3 from = new Vector3(MathF.Floor(x / UnitSize.x) * UnitSize.x, GridHeight, -halfSize.x);
                Vector3 to = new Vector3(MathF.Floor(x / UnitSize.x) * UnitSize.x, GridHeight, halfSize.x);
                Gizmos.DrawLine(from, to);
            }
            
            //draw y
            for (float y = pos.y - halfSize.y; y < pos.y + halfSize.y; y += UnitSize.y)
            {
                Vector3 from = new Vector3(-halfSize.y, GridHeight, MathF.Floor(y / UnitSize.y) * UnitSize.y);
                Vector3 to = new Vector3(halfSize.y, GridHeight, MathF.Floor(y / UnitSize.y) * UnitSize.y);
                Gizmos.DrawLine(from, to);
            }
        }
        
    }
#endif
}
