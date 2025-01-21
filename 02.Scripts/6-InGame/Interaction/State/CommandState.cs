namespace Refactor
{
    public class CommandState : IInteractionState
    {
        public InteractionStateMachine Machine { get; set; }
        PlayerUnit SelectedUnit => Machine.Interaction.curSelected;
        
        public CommandState(InteractionStateMachine machine)
        {
            Machine = machine;
        }
        
        public void Enter()
        {
            // 명령 초기화
            SelectedUnit.CommandSystem.StartListening();
            
            PlayerInput input = GameManager.Instance.Input;
            input.OnEscapePressed += OnEscapeCalled;
            input.OnClickPressed += OnClickCalled;
            input.OnSkillPressed += OnSkillCalled;
            input.OnEndActionPressed += OnEndActionCalled;
        }

        public void Exit()
        {
            GameManager.Instance.Indicator.HideAll();
            
            PlayerInput input = GameManager.Instance.Input;
            input.OnEscapePressed -= OnEscapeCalled;
            input.OnClickPressed -= OnClickCalled;
            input.OnSkillPressed -= OnSkillCalled;
            input.OnEndActionPressed -= OnEndActionCalled;
        }

        void OnEscapeCalled()
        {
            // Machine.Interaction.RevertCommand();
            GameManager.Instance.Mediator.CallRevert();
        }
        
        void OnClickCalled()
        {
            Machine.Interaction.Interact();
        }

        void OnSkillCalled(int num)
        {
            // Machine.Interaction.SelectSkill(num);
            GameManager.Instance.Mediator.CallSkillSelect(num);
        }

        void OnEndActionCalled()
        {
            Machine.Interaction.ExecuteCommand();
        }
    }
}