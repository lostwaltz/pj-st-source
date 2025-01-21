using UnityEngine;
using UnityEngine.UIElements;

namespace CustomStageTool
{
    public class ObstacleTool : PaintTool
    {
        StageObstacle[] prefabs => Controller.obstacles;
        Texture2D[] prefabTex => Controller.obstacleTexs;
        
        StageObstacle selected;

        VisualElement rotateArea;
        PlaceDirection direction = PlaceDirection.Forward;
        
        public ObstacleTool(ToolController controller) : base(controller)
        {
            SetToolType(ToolType.ObstacleTool);
        }

        public override void Enter()
        {
            base.Enter();
            
            place.EnablePlace(PlaceMode.Obstacle);
            place.ChangeKeyDownEvent(RotateDirection);
            SelectObstacle(0);
        }

        public override void Exit()
        {
            base.Exit();
            place.ChangeKeyDownEvent(null);
        }

        public override void DrawGUI()
        {
            AddTitle("장애물 배치창", "맵에 유닛들이 엄폐할 장애물을 배치할 수 있습니다.");
            
            base.DrawGUI();
            
            Label desc = new Label("키보드 A,D를 이용해 회전할 수 있습니다.");
            Controller.Body.Add(desc);
            
            rotateArea = new Box();
            Controller.Body.Add(rotateArea);
            RefreshRotateArea();

            Box spacer = new Box();
            spacer.style.height = 20f;
            Controller.Body.Add(spacer);
            
            Label listLabel = new Label("장애물 목록");
            listLabel.style.fontSize = 15;
            Controller.Body.Add(listLabel);
            
            ScrollView list = new ScrollView(ScrollViewMode.Vertical);
            list.style.height = 300f;
            for (int i = 0; i < prefabs.Length; i++)
                list.Add(GetItem(i));
            
            Controller.Body.Add(list);
        }

        VisualElement GetItem(int idx)
        {
            Button item = new Button(() => { SelectObstacle(idx);});
            item.style.marginBottom = 2f;
            item.style.marginTop = 2f;
            
            item.style.paddingBottom = 2f;
            item.style.paddingTop = 2f;
            item.style.paddingLeft = 2f;
            item.style.paddingRight = 2f;
            
            item.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
            
            VisualElement img = new VisualElement();
            img.style.backgroundImage = prefabTex[idx];
            img.style.width = 50f;
            img.style.height = 50f;
            
            img.style.flexShrink = 0f;
            img.style.flexGrow = 0f;
            item.hierarchy.Add(img);
            
            Label itemLabel  = new Label($"{prefabs[idx].name}\nSize : {prefabs[idx].size}");
            itemLabel.style.position = Position.Absolute;
            itemLabel.style.left = 60;
            itemLabel.style.top = 0;
            
            itemLabel.style.alignSelf = Align.FlexStart;

            itemLabel.style.flexShrink = 0f;
            itemLabel.style.flexGrow = 0f;
            
            itemLabel.style.color = Color.white;
            itemLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
            
            item.hierarchy.Add(itemLabel);
            
            return item;
        }

        void SelectObstacle(int idx)
        {
            selected = prefabs[idx];
            
            Vector2 size = selected.size;
            Vector3 newSize = new Vector3(size.x * grid.UnitSize.x, 1f, size.y * grid.UnitSize.y);
            place.ChangeObstacleSize(newSize);
        }

        
        protected override void ChangeMode(PaintType type)
        {
            switch (type)
            {
                case PaintType.Paint:
                    place.ChangeClickEvent(PlaceObstacle);
                    break;
                case PaintType.Erase:
                    place.ChangeClickEvent(EraseObstacle);
                    break;
            }
        }

        public void PlaceObstacle(Vector3 point)
        {
            place.Place(selected, point, Quaternion.Euler(0f, (int)direction * 90f, 0f));
        }

        public void EraseObstacle(Vector3 point)
        {
            place.Erase<StageObstacle>(point);
        }

        void RotateDirection(int dir)
        {
            int newVal = (int)direction + dir;
            
            if(newVal < 0)
                newVal = 3;
            else if (newVal > 3)
                newVal = 0;
            
            direction = (PlaceDirection)newVal;
            
            RefreshRotateArea();
        }

        void RefreshRotateArea()
        {
            rotateArea.Clear();
            
            Label rotateLabel = new Label($"현재 회전 : {(int)direction * 90f} {direction.ToString()}");
            
            rotateArea.Add(rotateLabel);
            
            place.ChangeRotate(direction);
        }
    }
    
}