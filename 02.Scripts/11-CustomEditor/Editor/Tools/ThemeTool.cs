using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace CustomStageTool
{
    public class ThemeTool : ToolBase
    {
        // List<SceneView> sceneViews = new List<SceneView>();>
        private string[] themes;
        private readonly string themePath = "Assets/01.Scenes/StageScenes/StageTheme/";
        
        public ThemeTool(ToolController controller) : base(controller)
        {
            themes = Directory.GetFiles(themePath, "*.unity");
            // SetToolType(ToolType.ThemeTool);
        }

        public override void DrawGUI()
        {
            AddTitle($"테마 선택창", "스테이지의 테마를 골라 넣을 수 있습니다.");
            
            ScrollView scrollView = new ScrollView(ScrollViewMode.VerticalAndHorizontal);
            scrollView.style.flexDirection = FlexDirection.Row;
            scrollView.style.height = 200;
            // scrollView.contentContainer.style.flexDirection = FlexDirection.Row;
            // scrollView.contentContainer.style.flexWrap = Wrap.Wrap;

            for (int i = 0; i < themes.Length; i++)
            {
                int idx = i;
                Button selectBtn = new Button(() => OnClickSelectTheme(idx)) { text = themes[i] };
                scrollView.Add(selectBtn);
            }
            
            Controller.Body.Add(scrollView);
        }

        void OnClickSelectTheme(int index)
        {
            Debug.Log($"{themes[index]} is selected");
            // EditorSceneManager.OpenScene(themes[index]);
            // EditorSceneManager.
            // Scene source = SceneManager.GetSceneByPath(themes[index]);
            // Scene curScene = SceneManager.GetActiveScene();
            // SceneManager.MergeScenes(source, curScene);
        }
    }
}