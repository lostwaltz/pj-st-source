using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

namespace CustomStageTool
{
    public class CellTool : PaintTool
    {
        StageCell cell => Controller.cell;
        Vector2Int mapSize;
        
        public CellTool(ToolController controller) : base(controller)
        {
            SetToolType(ToolType.CellTool);
        }

        public override void Enter()
        {
            base.Enter();
            place.EnablePlace(PlaceMode.Cell);
        }


        public override void DrawGUI()
        {
            AddTitle("타일(셀) 배치창", "맵에 유닛들이 이동할 셀을 배치할 수 있습니다.");
            
            base.DrawGUI();
            
            Label rangePlacing = new Label("범위 배치 도구");
            rangePlacing.style.fontSize = 15f;
            Controller.Body.Add(rangePlacing);

            Vector2IntField rangeInput = new Vector2IntField("범위");
            rangeInput.RegisterCallback<ChangeEvent<Vector2Int>>((evt) => { mapSize = evt.newValue; });
            Controller.Body.Add(rangeInput);

            Button placeBtn = new Button(PlaceCells);
            placeBtn.text = "범위 배치하기";
            placeBtn.style.height = 30f;
            Controller.Body.Add(placeBtn);

            Button eraseBtn = new Button(EraseCells);
            eraseBtn.text = "범위 삭제하기";
            eraseBtn.style.height = 30f;
            Controller.Body.Add(eraseBtn);

            Button clearBtn = new Button(ClearCells);
            clearBtn.text = "전체 삭제하기";
            clearBtn.style.height = 20f;
            Controller.Body.Add(clearBtn);;
        }

        void PlaceCells()
        {
            SelectRange(place.Place);
        }

        void EraseCells()
        {
            SelectRange((comp, pos)=> place.Erase<StageCell>(pos));
        }

        void ClearCells()
        {
            place.ClearAllPlacement();
        }

        void SelectRange(Action<StageCell, Vector3> action)
        {
            Vector2Int unit = grid.UnitSize;
            Vector2Int halfUnit = unit / 2;
            
            for (int i = 0; i < mapSize.x; i++)
            {
                for (int j = 0; j < mapSize.y; j++)
                {
                    int x = i * unit.x + halfUnit.x;
                    int y = j * unit.y + halfUnit.y;
                    
                    int offsetX = unit.x * mapSize.x / 2;
                    int offsetY = unit.y * mapSize.y / 2;
                    
                    offsetX = offsetX % unit.x == 0 ? offsetX : offsetX - halfUnit.x;
                    offsetY = offsetY % unit.y == 0 ? offsetY : offsetY - halfUnit.y;

                    x -= offsetX;
                    y -= offsetY;
                    
                    Vector3 pos = new Vector3(x, grid.GridHeight, y);
                    action?.Invoke(cell, pos);
                }
            }
        }

        protected override void ChangeMode(PaintType type)
        {
            switch (type)
            {
                case PaintType.Paint:
                    place.ChangeClickEvent(PaintCell);
                    break;
                case PaintType.Erase:
                    place.ChangeClickEvent(EraseCell);
                    break;
            }
        }

        void PaintCell(Vector3 point)
        {
            place.Place(cell, point);
        }

        void EraseCell(Vector3 point)
        {
            place.Erase<StageCell>(point);
        }
    }
}