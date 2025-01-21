using UnityEngine;

namespace Refactor
{
    public class SelectState : IInteractionState
    {
        public InteractionStateMachine Machine { get; set; }
        public SelectState(InteractionStateMachine machine)
        {
            Machine = machine;
        }
        
        public void Enter()
        {
            Machine.Interaction.curSelected = null;
            Core.UIManager.GetUI<UIBattleCanvas>().CloseUIPlayerableUnit();
            
            GameManager.Instance.Input.OnClickPressed += OnClickCalled;
        }

        public void Exit()
        {
            GameManager.Instance.Input.OnClickPressed -= OnClickCalled;
        }
        
        void OnClickCalled()
        {
            if (Machine.Interaction.Click(out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out PlayerUnit unit))
                    Machine.Interaction.SelectPlayerUnit(unit);
            }
            
        }

    }
}