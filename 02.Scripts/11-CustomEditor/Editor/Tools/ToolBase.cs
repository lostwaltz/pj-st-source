using UnityEngine.UIElements;

namespace CustomStageTool
{
    public abstract class ToolBase : IStageTool
    {
        protected ToolController Controller { get; set; }
        protected int ToolNum { get; set; }

        public ToolBase(ToolController controller)
        {
            Controller = controller;
        }

        public virtual void Enter()
        {
            DrawGUI();
        }

        public virtual void Exit()
        {
            Controller.Body.Clear();
        }

        public abstract void DrawGUI();

        public virtual void AddTitle(string title, string desc)
        {
            Label titleLabel = new Label() { text = $"{ToolNum}. {title}" };
            titleLabel.style.fontSize = 15;
            
            Label descLabel = new Label();
            descLabel.text = desc;
            
            Label space = new Label();
            space.style.fontSize = 10;
            
            Controller.Body.Add(titleLabel);
            Controller.Body.Add(descLabel);
            Controller.Body.Add(space);
        }

        protected void SetToolType(ToolType toolType)
        {
            ToolNum = (int)toolType + 1;
        }
    }
    
}