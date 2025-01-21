using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CustomStageTool
{
    public class ImportExportTool : ToolBase
    {
        private ObjectField field;
        StagePlaceTool place => Controller.Place;
        StageDataTool data => Controller.Data;
        
        public ImportExportTool(ToolController controller) : base(controller)
        {
            SetToolType(ToolType.ImportExportTool);
        }


        public override void DrawGUI()
        {
            AddTitle("저장/불러오기", "현재 진행 상황을 저장하거나 다른 파일을 불러옵니다.");

            if (field == null)
            {
                field = new ObjectField("저장 대상 SO");
                field.objectType = typeof(StageSO);
            }
            Controller.Body.Add(field);
            
            Button saveBtn = new Button(Save);
            saveBtn.text = "저장하기";
            Controller.Body.Add(saveBtn);
            
            Button loadBtn = new Button(Load);
            loadBtn.text = "불러오기";
            Controller.Body.Add(loadBtn);
        }

        void Save()
        {
            if (field.value == null)
            {
                Debug.Log("저장할 파일이 없습니다.\n파일을 할당 후 다시 진행해주세요.");
                return;
            }
            
            StageSO file = field.value as StageSO;
            
            data.ReadCell(file, place.GetParent(PlaceMode.Cell), Controller.Grid.UnitSize);
            data.ReadObstacles(file, place.GetParent(PlaceMode.Obstacle), Controller.Grid.UnitSize);
        }
        
        void Load()
        {
            if (field.value == null)
            {
                Debug.Log("저장할 파일이 없습니다.\n파일을 할당 후 다시 진행해주세요.");
                return;
            }
            
            place.ClearAllPlacement();
            
            StageSO file = field.value as StageSO;

            place.EnablePlace(PlaceMode.Cell);
            foreach (var data in file.cellPlacements)
                place.Place(Controller.cell, data.position);
            
            place.EnablePlace(PlaceMode.Obstacle);
            foreach (var data in file.obstaclePlacements)
                place.Place(Controller.obstacles[data.id], data.position, data.rotation);
            
            place.DisablePlace();
        }
        
    }
}