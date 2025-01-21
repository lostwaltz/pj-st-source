using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CustomStageTool
{
#if UNITY_EDITOR
    public enum PlaceMode
    {
        None = -1,
        Cell,
        Obstacle,
        
        COUNT
    }

    public enum PlaceDirection
    {
        Forward = 0,
        Right,
        Backward,
        Left,
    }
    
    [ExecuteInEditMode]
    public class StagePlaceTool : SingletonForTool<StagePlaceTool>
    {
        StageGridTool gridTool => StageGridTool.Instance;
        
        [SerializeField] List<Transform> parents = new();
        
        [Space(10f)]
        [SerializeField] Color cursorColor = new (0.1f, 0.1f, 1f, 1f);
        [SerializeField] Vector3 cursorPoint = Vector3.zero;
        
        public PlaceMode CurPlaceMode = PlaceMode.None;
        [SerializeField] PlaceDirection curRotation = PlaceDirection.Forward;
        
        bool isHit = false;
        Vector3 cellSize = new Vector3(2f, 0.5f, 2f);
        Vector3 obstacleSize = new Vector3(1f, 1f, 1f);
        
        Dictionary<Type, Dictionary<Vector3, StageComponent>> placed = new();
        
        Dictionary<Vector3, bool> isOccupied = new();
        Dictionary<Vector3, List<Vector3>> occupiedMap = new();
        
        
        private Action<Vector3> onClicked;
        private Action<int> onKeyDown;
        
        protected override void Awake()
        {
            base.Awake();
            
            if (parents.Count == 0)
            {
                for (int i = 0; i < (int)PlaceMode.COUNT; i++)
                {
                    Transform obj = new GameObject(((PlaceMode)i).ToString()).transform;
                    parents.Add(obj);
                    obj.parent = transform;
                }
            }
        }

        private void OnEnable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }
      
        private void OnDrawGizmos()
        {
            if (CurPlaceMode != PlaceMode.None && isHit)
                DrawCursor();
        }

        

        public void EnablePlace(PlaceMode mode)
        {
            CurPlaceMode = mode;
        }

        public void DisablePlace()
        {
            CurPlaceMode = PlaceMode.None;
        }
        
        void OnSceneGUI(SceneView sceneView)
        {
            if(CurPlaceMode == PlaceMode.None)
                return;
            
            SetCursor();

            Event e = Event.current;
                
            if (e.type == EventType.KeyDown)
            {
                if (e.keyCode == KeyCode.A)
                    onKeyDown?.Invoke(-1);
                else if(e.keyCode == KeyCode.D)
                    onKeyDown?.Invoke(1);
            }

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                onClicked?.Invoke(cursorPoint);
                e.Use(); // 이후 클릭 방지
            }
            
            HandleUtility.Repaint();
        }

        void SetCursor()
        {
            var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            var hPlane = new Plane(Vector3.up, Vector3.up * gridTool.GridHeight);

            isHit = hPlane.Raycast(ray, out var point);

            if (isHit)
            {
                Vector3 pos = ray.GetPoint(point);
                Vector2Int half = gridTool.UnitSize / 2;
                
                Vector3 roundPos = new Vector3(Mathf.RoundToInt(pos.x), gridTool.GridHeight, Mathf.RoundToInt(pos.z));
                roundPos.x = roundPos.x % gridTool.UnitSize.x == 0 ? 
                    pos.x >= 0f ? Mathf.Floor(roundPos.x + half.x) : Mathf.Ceil(roundPos.x - half.y) : roundPos.x;
                roundPos.z = roundPos.z % gridTool.UnitSize.y == 0 ?
                    pos.z >= 0f ? Mathf.Floor(roundPos.z + half.x) : Mathf.Ceil(roundPos.z - half.y) : roundPos.z;
                
                cursorPoint = roundPos;
            }
        }
        
        void DrawCursor()
        {
            Gizmos.color = cursorColor;
            if (CurPlaceMode == PlaceMode.Cell)
            {
                Gizmos.DrawWireCube(cursorPoint, cellSize);    
            }
            else
            {
                ResizeByRotation(cursorPoint, obstacleSize, curRotation, 
                    out Vector3 newPoint, out Vector3 newSize);

                Gizmos.DrawWireCube(newPoint, newSize);
            }
        }

        void ResizeByRotation(Vector3 basePoint, Vector3 baseSize, PlaceDirection rotation, 
            out Vector3 newPoint, out Vector3 newSize)
        {
            newSize = rotation switch
            {
                PlaceDirection.Right => new Vector3(baseSize.z, baseSize.y, baseSize.x),
                PlaceDirection.Left => new Vector3(baseSize.z, baseSize.y, baseSize.x),
                _=> baseSize,
            };
            
            Vector3 offset = new Vector3(newSize.x / 2 - 1, 0f, newSize.z / 2 - 1);
                
            newPoint = rotation switch
            {
                PlaceDirection.Forward => basePoint + offset,
                PlaceDirection.Backward => basePoint - offset,
                PlaceDirection.Right => basePoint + offset,
                PlaceDirection.Left => basePoint - offset
            };
        }

        public void Place(StageCell component, Vector3 position)
        {
            Type type = component.GetType();
            
            if (!placed.ContainsKey(type))
                placed.TryAdd(type, new Dictionary<Vector3, StageComponent>());
            
            if (placed[type].TryGetValue(position, out var comp) && 
                comp != null)
                return;
            
            var newComp = Instantiate(component, parents[(int)CurPlaceMode]);
            
            newComp.transform.position = position;
            
            placed[type].TryAdd(position, newComp);
        }
        
        public void Place(StageObstacle component, Vector3 position, Quaternion rotation)
        {
            Type type = component.GetType();

            // 셀이 없는 곳에 배치할 순 없음.
            Type cell = typeof(StageCell);
            if (!placed.ContainsKey(cell))
                return;
                
            if(!placed[cell].ContainsKey(position)) 
                return;
            
            if (!placed.ContainsKey(type))
                placed.TryAdd(type, new Dictionary<Vector3, StageComponent>());
            
            if (placed[type].TryGetValue(position, out var comp) && comp != null)
                return;
            
            GetAreaByRotation(position, component.size, curRotation, out List<Vector3> areas);
            
            foreach (var area in areas)
            {
                if(!placed[cell].ContainsKey(area))
                    return;
                
                if (isOccupied.TryGetValue(area, out bool occupied) && occupied)
                    return;
            }
            
            var newComp = Instantiate(component, parents[(int)CurPlaceMode]);
            
            newComp.transform.position = position;
            newComp.transform.rotation = rotation;
            
            placed[type].TryAdd(position, newComp);
            occupiedMap.TryAdd(position, new List<Vector3>());

            foreach (var area in areas)
            {
                isOccupied.TryAdd(area, true);
                occupiedMap[position].Add(area);
            }
        }
        
        public void Erase<T>(Vector3 position) where T : StageComponent
        {
            Type type = typeof(T);
            
            if (!placed.ContainsKey(type))
                return;

            if (placed[type].TryGetValue(position, out var comp) &&
                comp != null)
            {
                DestroyImmediate(comp.gameObject);
                placed[type].Remove(position);
                
                if (type != typeof(StageCell))
                {
                    for (int i = 0; i < occupiedMap[position].Count; i++)
                        isOccupied.Remove(occupiedMap[position][i]);
                
                    occupiedMap.Remove(position);   
                }
            }
        }

        public void GetAreaByRotation(Vector3 basePoint, Vector2 baseSize, PlaceDirection rotation, 
            out List<Vector3> areaPosition)
        {
            areaPosition = new();
            Vector2Int unit = gridTool.UnitSize;
            
            Vector3 dir = rotation switch
            {
                // 장애물의 기준점(anchor)는 좌측 하단으로 고정되었다고 가정
                PlaceDirection.Forward => new Vector3(unit.x, 0f, unit.y),
                PlaceDirection.Backward => new Vector3(-unit.x, 0f, -unit.y),
                PlaceDirection.Right => new Vector3(unit.x, 0f, -unit.y),
                PlaceDirection.Left => new Vector3(-unit.x, 0f, unit.y),
            };

            Vector2 newSize = rotation switch
            {
                PlaceDirection.Right => new Vector2(baseSize.y, baseSize.x),
                PlaceDirection.Left => new Vector2(baseSize.y, baseSize.x),
                _ => baseSize,
            };
            
            // Debug.Log($"Get Area By Rotation : {basePoint}");
            for (int i = 0; i < newSize.x; i++)
            {
                for (int j = 0; j < newSize.y; j++)
                {
                    Vector3 newPos = new Vector3(basePoint.x + dir.x * i, basePoint.y, basePoint.z + dir.z * j);
                    // Debug.Log($"Get Area : {newPos}");
                    areaPosition.Add(newPos);
                }
            }
        }
        
        

        public void ChangeClickEvent(Action<Vector3> newAction)
        {
            onClicked = newAction;
        }
        
        public void ChangeKeyDownEvent(Action<int> newAction)
        {
            onKeyDown = newAction;
        }
        
        public void ChangeObstacleSize(Vector3 size)
        {
            obstacleSize = size;
        }

        public void ChangeRotate(PlaceDirection rotation)
        {
            curRotation = rotation;
        }


        public void ClearAllPlacement()
        {
            GameObject destroy  = new GameObject("Destroy");

            for (int i = 0; i < parents.Count; i++)
            {
                while (parents[i].childCount > 0)
                    parents[i].GetChild(0).transform.SetParent(destroy.transform);
            }
            
            DestroyImmediate(destroy);
            
            placed.Clear();
        }

        public Transform GetParent(PlaceMode mode)
        {
            return parents[(int)mode];
        }

        public List<Vector3> GetOccupied(Vector3 pos)
        {
            return occupiedMap[pos];
        }
    }
#endif
    
}
