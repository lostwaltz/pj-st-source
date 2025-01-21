using System;
using UnityEngine;
using UnityEditor;

namespace CustomStageTool
{
    public class StageToolWindow : EditorWindow
    {
        private static EditorWindow window;
        private static ToolController tools;
        
        public event Action OnClosed;
    
        [MenuItem("CustomEditor/Stage Tool")]
        public static void ShowWindow()
        { 
            window = GetWindow(typeof(StageToolWindow));
            
            window.titleContent = new GUIContent("Stage Tool");
            window.minSize = new Vector2(300, 500);
            window.maxSize = new Vector2(1000, 1000);
            
            window.Show();
        }

        private void CreateGUI()
        {
            if (tools == null)
                tools = new ToolController();
            
            tools.Initialize(this);
        }

        private void OnDisable()
        {
            OnClosed?.Invoke();
            tools = null;
        }
    }
}


