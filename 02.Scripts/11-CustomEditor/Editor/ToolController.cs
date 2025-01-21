using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CustomStageTool
{
    public enum ToolType
    {
        // ThemeTool,
        CellTool,
        ObstacleTool,
        // DecoTool,
        ImportExportTool,
        
        COUNT
    }

    public class ToolController
    {
        Dictionary<ToolType, IStageTool> tools;
        IStageTool CurTool { get; set; }
        
        public StageGridTool Grid { get; private set; }
        public StagePlaceTool Place { get; private set; }
        public StageDataTool Data { get; private set; }

        public StageCell cell { get; private set; }
        public StageObstacle[] obstacles { get; private set; }
        public Texture2D[] obstacleTexs { get; private set; }
        

        public VisualElement Root { get; private set; }
        public VisualElement Body { get; set; }
        
        public event Action OnClosed;
        

        public ToolController()
        {
            tools = new Dictionary<ToolType, IStageTool>()
            {
                // { ToolType.ThemeTool, new ThemeTool(this)},
                { ToolType.CellTool, new CellTool(this)},
                { ToolType.ObstacleTool, new ObstacleTool(this)},
                // { ToolType.DecoTool, new DecoTool(this)},
                { ToolType.ImportExportTool, new ImportExportTool(this)}
            };
        }

        public void Initialize(StageToolWindow window)
        {
            Root = window.rootVisualElement;
            
            Grid = StageGridTool.Instance;
            Place = StagePlaceTool.Instance;
            Data = StageDataTool.Instance;
            cell = Resources.Load<StageCell>(ToolResources.CELL_PATH);
            
            SetUI();
            ChangeTool(tools[ToolType.CellTool]);
            
            obstacles = Resources.LoadAll<StageObstacle>(ToolResources.OBSTACLE_PATH);
            obstacleTexs = new Texture2D[obstacles.Length];
            for (int i = 0; i < obstacles.Length; i++)
                obstacleTexs[i] = AssetPreview.GetAssetPreview(obstacles[i].gameObject);

            window.OnClosed -= ClearTools;
            window.OnClosed += ClearTools;
        }
        
        public void ChangeTool(IStageTool newTool)
        {
            CurTool?.Exit();
            
            CurTool = newTool;
            
            CurTool.Enter();
        }

        void SetUI()
        {
            Root.Clear();
            
            // Header : 공통사항
            Box header = new Box();
            header.name = "Header";
            
            Label title = new Label("Stage Tool Kit");
            title.style.fontSize = 20;
            title.style.unityTextAlign = TextAnchor.MiddleCenter;
            header.Add(title);
            
            Label empty = new Label();
            empty.style.fontSize = 10;
            header.Add(empty);
            
            Toolbar toolbar = new Toolbar();
            
            int toolCount = (int)ToolType.COUNT;
            for (int i = 0; i < toolCount; i++)
            {
                ToolType type = (ToolType)i;
                
                ToolbarButton btn = new ToolbarButton();
                btn.text = $"<b>{ i + 1 }.</b> { type.ToString() }";
                btn.clickable.clicked += () => { ChangeTool(tools[type]); };
                
                toolbar.Add(btn);
            }
            header.Add(toolbar);
            Root.Add(header);
            
            
            // Body : 하위 toolkit에서 정의될 것
            Body?.Clear();
            Box body = new Box();
            Root.Add(body);
            
            Body = body;
        }

        void ClearTools()
        {
            OnClosed?.Invoke();
            
            GameObject destroy = new GameObject("destroy");
            
            if (Grid != null)
                Grid.transform.SetParent(destroy.transform);
            
            if (Place != null)
                Place.transform.SetParent(destroy.transform);

            if (Data != null)
                Data.transform.SetParent(destroy.transform);

            GameObject.DestroyImmediate(destroy);

        }
    }
}