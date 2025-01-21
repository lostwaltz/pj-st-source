using System.Collections.Generic;
using UnityEngine.UIElements;

namespace CustomStageTool
{
    public enum PaintType
    {
        Paint,
        Erase
    }

    public abstract class PaintTool: ToolBase
    {
        protected StageGridTool grid => Controller.Grid;
        protected StagePlaceTool place => Controller.Place;
        public PaintTool(ToolController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            base.Enter();
            ChangeMode(0);
        }

        public override void Exit()
        {
            base.Exit();
            place.DisablePlace();
            place.ChangeClickEvent(null);
        }

        public override void DrawGUI()
        {
            Label normalPlacing = new Label("일반 배치 도구");
            normalPlacing.style.fontSize = 15f;
            Controller.Body.Add(normalPlacing);
            
            DropdownField toolMode = new DropdownField("툴 모드", new List<string>{PaintType.Paint.ToString(), PaintType.Erase.ToString()}, 0);
            toolMode.RegisterValueChangedCallback((evt) => { ChangeMode((PaintType)toolMode.index); });
            Controller.Body.Add(toolMode);

            Box spacer = new Box();
            spacer.style.height = 20f;
            Controller.Body.Add(spacer);
        }

        protected abstract void ChangeMode(PaintType type);
    }
}