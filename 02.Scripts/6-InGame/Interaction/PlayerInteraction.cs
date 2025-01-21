using System;
using EnumTypes;
using UnityEngine;

namespace Refactor
{
    public class PlayerInteraction : MonoBehaviour
    {
        Camera mainCam;
        public int OriginLayer = 1 << 11 | 1 << 12 | 1 << 22;
        public LayerMask layerMask = 1 << 11 | 1 << 12 | 1 << 22;

        public PlayerTurnPhase TurnHandler { get; set; }
        [field: SerializeField] public bool CursorIsOnUI { get; set; }

        public PlayerUnit curSelected;
        public InteractionStateMachine StateMachine;
        InteractionMediator mediator;

        // UI를 위한 것
        public event Action<IClickable> OnClicked;
        public event Action<PlayerUnit> OnPlayerUnitSelected;

        void Awake()
        {
            mainCam = Camera.main;
            StateMachine = new InteractionStateMachine();
        }

        private void Start()
        {
            mediator = GameManager.Instance.Mediator;
            mediator.OnSkillSelected += SelectSkill;
            mediator.OnRevertCalled += RevertCommand;

            GameManager.Instance.CommandController.OnPostProcessed += (phase) =>
            {
                if (phase == GamePhase.PlayerTurn)
                    ProceedTurn();
            };

            OnClicked += clickable =>
            {
                if (clickable is Unit)
                    CameraSystem.EventHandler.Publish(CameraEventTrigger.OnPlayerClicked, new CameraEventContext(clickable as Unit));
            };
        }

        public void Activate()
        {
            curSelected = null;
            StateMachine.ChangeState(StateMachine.SelectState);
        }

        public void Deactivate()
        {
            StateMachine.ChangeState(StateMachine.PauseState);
        }

        public void CallClickEvent(IClickable thing)
        {
            OnClicked?.Invoke(thing);
        }

        // 공용 메서드
        public bool Click(out RaycastHit hit)
        {
            if (CursorIsOnUI)
            {
                hit = new RaycastHit();
                return false;
            }

            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            bool isHit = Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);

            if (isHit && hit.collider.TryGetComponent(out IClickable clickable))
                CallClickEvent(clickable);

            return isHit;
        }

        public void SelectPlayerUnit(PlayerUnit selected)
        {
            if (selected.CommandSystem.IsComplete)
            {
                Core.UIManager.GetUI<UIReselectionCanvas>().Open(Constants.CommonUI.WARNING_ON_COMPLETED_UNIT);
                return;
            }

            // 활성화
            curSelected = selected;

            StateMachine.ChangeState(StateMachine.CommandState);

            UIBattle.PlayUIBattleClickVirtualAttack();

            OnPlayerUnitSelected?.Invoke(selected);
        }

        public void Pause()
        {
            // 명령 수행 등을 위해서 일시정지 시킴
            StateMachine.ChangeState(StateMachine.PauseState);
        }

        public void ProceedTurn()
        {
            // 구조적으로 달라질 수 있음
            Utils.CheckCondition(TurnHandler.IsAutoPlay,
                TurnHandler.AutoProceed,
                TurnHandler.Proceed);
        }

        public void Interact()
        {
            // 클릭 중계
            curSelected.CommandSystem.InteractReceiver();
        }

        void SelectSkill(int num)
        {
            if (num >= 4)
                return;

            // 공격으로 전환
            // 스킬을 골랐을때 해 주어야할 일
            curSelected.CommandSystem.ChangeReceiver(curSelected.SkillSystem, PlayerCommandType.UseSkill, num);
        }

        public void ExecuteCommand()
        {
            Pause();

            // 행동 수행
            curSelected.CommandSystem.ExecuteCommand();
        }

        void RevertCommand()
        {
            // 행동 번복
            curSelected.CommandSystem.RevertCommand(out PlayerCommandType type, out ReceiverStep step);
            mediator.CallAfterReverted(type, step);
        }

        public void UpdateLayerMask(LayerMask newMask)
        {
            layerMask = newMask;
        }

        public void RestoreLayerMask()
        {
            layerMask = OriginLayer;
        }

    }
}

