using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager
{
    PlayerInputAction input;

    /// <summary>
    /// 주의 로컬 씬에서 구독한 것들은 모두 OnDisable에서 구독 취소해주십시오
    /// </summary>
    public PlayerInputAction.PlayerActions actions;


    public event Action OnClickReceived;
    public event Action OnEscapeReceived;
    public event Action OnEndActionReceived;
    public event Action<int> OnNumberReceived;
    public event Action<int> OnRotateReceived;
    public event Action OnRotateEndReceived;
    public event Action<Vector2> OnMoveReceived;
    
    

    public InputManager()
    {
        input = new PlayerInputAction();
        actions = input.Player;
    }

    public void Init()
    {
        actions.Enable();

        actions.Click.started += CallClickEvent;
        actions.Esc.started += CallEscapeEvent;
        actions.EndAction.started += CallEndActionEvent;
        
        actions.Number.started += CallNumberEvent;
        
        actions.Move.performed += CallMoveEvent;
        actions.Move.canceled += CallMoveEvent;

        actions.Rotate.performed += CallRotateEvent;
        actions.Rotate.canceled += CallRotateEndEvent;
    }

    public void End()
    {
        actions.Click.started -= CallClickEvent;
        actions.Esc.started -= CallEscapeEvent;
        actions.EndAction.started -= CallEndActionEvent;
        
        actions.Number.started -= CallNumberEvent;

        actions.Move.performed -= CallMoveEvent;
        actions.Move.canceled -= CallMoveEvent;

        actions.Rotate.performed -= CallRotateEvent;
        actions.Rotate.canceled -= CallRotateEndEvent;

        actions.Disable();
    }
    

    void CallClickEvent(InputAction.CallbackContext context)
    {
        OnClickReceived?.Invoke();
    }
    
    void CallEscapeEvent(InputAction.CallbackContext context)
    {
        OnEscapeReceived?.Invoke();
    }

    void CallNumberEvent(InputAction.CallbackContext context)
    {
        OnNumberReceived?.Invoke((int)context.ReadValue<Single>());
    }
    
    void CallEndActionEvent(InputAction.CallbackContext context)
    {
        OnEndActionReceived?.Invoke();
    }

    void CallMoveEvent(InputAction.CallbackContext context)
    {
        if(context.canceled)
            OnMoveReceived?.Invoke(Vector2.zero);
        else
            OnMoveReceived?.Invoke(context.ReadValue<Vector2>());
    }

    void CallRotateEvent(InputAction.CallbackContext context)
    {
        // 값 유형 Q : -1, E : +1
        int direction = (int)context.ReadValue<Single>();
        OnRotateReceived?.Invoke(direction);
    }

    void CallRotateEndEvent(InputAction.CallbackContext context)
    {
        OnRotateEndReceived?.Invoke();
    }
}

