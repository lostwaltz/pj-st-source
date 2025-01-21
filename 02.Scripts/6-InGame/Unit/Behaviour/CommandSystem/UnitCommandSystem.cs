using System;
using System.Collections.Generic;
using EnumTypes;
using Structs;
using UnityEngine;

public class UnitCommandSystem : MonoBehaviour
{
    Unit unit;

    PlayerCommandType curCommandType; 
    ICommandReceiver curReceiver;
    
    [field:SerializeField] public bool IsComplete { get; set; }
    public List<Func<IUnitCommand>> commands = new (); // 기본 명령 2, 필요에 따란 추가 명령 1
    public Dictionary<int, Vector2> commandCoord = new ();

    public event Action OnStandBy;
    public event Action OnCommandStarted; // unit의 명령 전 처리 사항
    public event Action OnCommandEnded;   // unit의 명령 후 처리 사항
    
    public void Initialize(Unit owner)
    {
        // 생성 시, 최초 한번 초기화
        unit = owner;
    }

    
    public void Standby()
    {
        // 매 턴 시작 때 호출
        ClearCommand();
        IsComplete = false;
        
        OnStandBy?.Invoke();
    }

    public void StartListening()
    {
        // unit이 선택될 때 호출
        ClearCommand();
        ChangeReceiver(unit.Movement, (int)PlayerCommandType.Move); // 기본 명령은 이동

        GameManager.Instance.Indicator.ShowSelectedPlayer(unit.curCoord);
    }

    public void ChangeReceiver(
        ICommandReceiver receiver, 
        PlayerCommandType commandType, 
        int opt = 0,
        bool isReverted = false)
    {
        curReceiver?.Clear();
        
        curCommandType = commandType;
        
        curReceiver = receiver;
        
        curReceiver.Ready(opt, isReverted);
    }

    public void InteractReceiver()
    {
        curReceiver.Interact();
    }
    
    
    public void ClearCommand()
    {
        commands.Clear();
        commandCoord.Clear();
    }

    public bool RevertCommand(out PlayerCommandType commandType, out ReceiverStep commandStep)
    {
        if (commands.Count == 0)
        {
            // 취소할 명령이 없는데 번복이 들어온 상태
            commandType = PlayerCommandType.Move;
            commandStep = ReceiverStep.None;
            
            // 해결법 1 이동 명령으로 변경
            ClearCommand();
            ChangeReceiver(unit.Movement, (int)PlayerCommandType.Move);
            
            return false;
        }
        
        if (curReceiver.ReceiverStep == ReceiverStep.Determine)
        {
            int idx = commands.Count - 1;
            commands.RemoveAt(idx);
            commandCoord.Remove(idx);
        }
        
        curReceiver?.Revert();
        
        commandType = curCommandType;
        commandStep = (curReceiver?.ReceiverStep ?? ReceiverStep.Ready);
        
        return true; //commands.Count > 0;
    }
    
    public void UpdateCommand(int index, Vector2 coord, Func<IUnitCommand> command)
    {
        if (commands.Count <= index)
        {
            for (int i = commands.Count; i < index + 1; i++)
            {
                commands.Add(null);
                if (!commandCoord.ContainsKey(index))
                    commandCoord.Add(index, unit.curCoord);
            }
        }
        
        commands[index] = command;
        commandCoord[index] = coord;
    }

    public void ExecuteCommand()
    {
        IsComplete = true; // TODO : 패시브에 따라서 완료가 안될 수 있음. 추후에 수정할 필요가 있음
        GameManager.Instance.Interaction.RestoreLayerMask();
        
        // 행동 수행
        for (int i = 0; i < commands.Count; i++)
        {
            if(commands[i] == null) 
                continue;

            GameManager.Instance.CommandController.AddCommand(commands[i].Invoke());
        }
        
        GameManager.Instance.CommandController.ExecuteCommand(OnCommandStarted, OnCommandEnded);
        
    }
}