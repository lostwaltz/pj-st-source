using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameCommandUndoHandler : MonoBehaviour
{
    private Stack<Stack<Stack<IUnitCommand>>> History => GameManager.Instance.CommandController.gameHistory;
    
    private readonly Stack<Stack<Stack<IUnitCommand>>> redoHistory = new();
    private Stack<Stack<IUnitCommand>> currentTurnHistory;
    
    public void OnEnterUndoMode()
    {
        if(History.Count <= 0) return;
        
        currentTurnHistory = History.Pop();
        GameManager.Instance.CommandController.HistoryChanged?.Invoke(History.Count);    

        UndoLastTurn();
    }
    
    public void UndoLastTurn()
    {
        if (currentTurnHistory == null || currentTurnHistory.Count == 0)
        {
            Debug.LogWarning("No turn history to undo.");
            return;
        }

        var deepCopiedTurnHistory = new Stack<Stack<IUnitCommand>>();
        foreach (var unitCommandStack in currentTurnHistory)
        {
            var deepCopiedUnitStack = new Stack<IUnitCommand>(unitCommandStack);
            deepCopiedTurnHistory.Push(new Stack<IUnitCommand>(deepCopiedUnitStack.Reverse()));
        }
        redoHistory.Push(deepCopiedTurnHistory);

        while (currentTurnHistory.Count > 0)
        {
            var unitCommandStack = currentTurnHistory.Pop();

            while (unitCommandStack.Count > 0)
            {
                var command = unitCommandStack.Pop();
                // command.Undo();
            }
        }
    }
}