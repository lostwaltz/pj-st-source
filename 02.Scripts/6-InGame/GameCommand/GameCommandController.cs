using System;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;
using IEnumerator = System.Collections.IEnumerator;

// Invoker 요소
public class GameCommandController : MonoBehaviour
{
    public GamePhase prevPhase;
    public Coroutine CommandCoroutineHandle;
    
    public const int MaxHistorySize = 3; // 최대 히스토리 크기
    public Stack<Stack<Stack<IUnitCommand>>> gameHistory = new();
    // 한 개체 유닛의 명령 모음 ( 공격, 움직임 등 )
    // 위 명령 모음을 묶는 하나의 명령 히스토리 즉, 여러 개체의 명령 모음
    // 여러개체의 명령 모음을 묶는 턴 모음의 히스토리
    // 즉 3중 스택이 필요함
    
    public Stack<Stack<IUnitCommand>> currentTurnHistory;
    
    public Action<int> HistoryChanged;

    
    // insert를 위해서 List로 구현
    List<IUnitCommand> commandList = new List<IUnitCommand>();
    Queue<IUnitCommand> commandQueue = new();
    
    private void Start()
    {
        GameManager.Instance.SubscribePhaseEvent(GamePhase.EnemyTurn, OnTurnStarted);
        GameManager.Instance.SubscribePhaseEvent(GamePhase.PlayerTurn, OnTurnStarted);
    }

    private void OnDisable()
    {
        OnPreProcessed = null;
        OnPostProcessed = null;
    }

    private void OnTurnStarted()
    {
        if (currentTurnHistory != null)
        {
            AddToHistory(currentTurnHistory);
            HistoryChanged?.Invoke(gameHistory.Count);
        }
        
        currentTurnHistory = new Stack<Stack<IUnitCommand>>();
    }
    
    private void AddToHistory(Stack<Stack<IUnitCommand>> turnHistory)
    {
        if (gameHistory.Count >= MaxHistorySize)
        {
            var tempStack = new Stack<Stack<Stack<IUnitCommand>>>();
            while (gameHistory.Count > 1)
                tempStack.Push(gameHistory.Pop());

            gameHistory.Pop();

            while (tempStack.Count > 0)
                gameHistory.Push(tempStack.Pop());
        }

        gameHistory.Push(turnHistory);
    }

    public event Action<GamePhase> OnPreProcessed;
    public event Action<GamePhase> OnPostProcessed;
    
    // 행동 추가
    public void AddCommand(IUnitCommand command)
    {
        commandList.Add(command); // 명령 리스트에 추가
    }

    // 행동 구조
    // 행동 전 처리 - 행동 처리 - 행동 후 처리
    public void ExecuteCommand(Action onCommandStarted, Action onCommandEnded)
    {
        if (CommandCoroutineHandle != null)
            return;
        
        CommandCoroutineHandle = StartCoroutine(ExecuteCommandRoutine(onCommandStarted, onCommandEnded, currentTurnHistory));
    }
    
    IEnumerator ExecuteCommandRoutine(Action onCommandStarted, Action onCommandEnded, Stack<Stack<IUnitCommand>> unitHistory)
    {  
        OnPreProcessed?.Invoke(GameManager.Instance.currentPhase);
        onCommandStarted?.Invoke();
        
        // 한 유닛의 명령 묶음
        var commandHistory = new Stack<IUnitCommand>();
        
        prevPhase = GameManager.Instance.currentPhase;
        GameManager.Instance.currentPhase = GamePhase.ProcessCommand;
        
        for (int i = 0; i < commandList.Count; i++)
        {
            yield return ExecuteCommandByEvent(commandList[i], CommandPhase.BeforeCommand, commandHistory);

            if(commandList[i] is MoveCommand)
                CameraSystem.EventHandler.Publish(CameraEventTrigger.OnUnitMove, new CameraEventContext(commandList[i]));
            else
                CameraSystem.EventHandler.Publish(CameraEventTrigger.OnUnitActivateSkill, new CameraEventContext(commandList[i]));
            
            commandHistory.Push(commandList[i]);
            yield return StartCoroutine(commandList[i].Execute());
            
            // if (commandList[i] is AttackCommand)
            yield return ExecuteCommandByEvent(commandList[i], CommandPhase.AfterCommand, commandHistory);
        }
        
        GameManager.Instance.currentPhase = prevPhase;
        
        // 여러 유닛의 명령 모음에 Push
        unitHistory.Push(commandHistory);
        
        CommandCoroutineHandle = null;
        commandList.Clear();
        
        onCommandEnded?.Invoke();
        OnPostProcessed?.Invoke(GameManager.Instance.currentPhase);
    }
    
    private IEnumerator ExecuteCommandByEvent(IUnitCommand command, CommandPhase phase, Stack<IUnitCommand> commandHistory)
    {
        var commandPushList = new List<IUnitCommand>();

        Core.EventManager.Publish(new ExecuteCommandEvent(command, commandPushList, phase));
        
        foreach (var context in commandPushList)
        {
            CameraSystem.EventHandler.Publish(CameraEventTrigger.OnUnitActivatePassive, new CameraEventContext(context));
            
            commandHistory.Push(context);
            yield return StartCoroutine(context.Execute());
        }
    }

    public void ExecuteCommandOnTurnEnter(GamePhase gamePhase, Action onComplete)
    {
        StartCoroutine(ExecuteCommand(gamePhase, onComplete));
    }

    private IEnumerator ExecuteCommand(GamePhase gamePhase, Action onComplete)
    {
        Queue<IUnitCommand> commands = new Queue<IUnitCommand>();
        Core.EventManager.Publish(new TurnEnterCommandEvent(gamePhase, commands));
        
        while (commands.Count > 0)
        {
            //TODO: 히스토리에 추가
            IUnitCommand command = commands.Dequeue();
            CameraSystem.EventHandler.Publish(CameraEventTrigger.OnUnitActivatePassive, new CameraEventContext(command));
            yield return command.Execute();
        }
        
        onComplete?.Invoke();
    }
    
    
}