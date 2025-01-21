namespace CustomStageTool
{
    public class DecoTool : ToolBase
    {
        public DecoTool(ToolController controller) : base(controller)
        {
            
            // SetToolType(ToolType.DecoTool);
        }


        public override void DrawGUI()
        {
            AddTitle("장식 배치창", "맵에 여러 장식물을 배치할 수 있습니다.");
        }
    }
}