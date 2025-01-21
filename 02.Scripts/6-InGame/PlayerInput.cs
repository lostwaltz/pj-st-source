using System;
using UnityEngine;
using EnumTypes;

public class PlayerInput : MonoBehaviour
{
    public event Action OnClickPressed;
    public event Action OnEscapePressed;
    public event Action<int> OnSkillPressed;
    public event Action OnEndActionPressed;
    
    private void OnEnable()
    {
        Core.InputManager.OnEscapeReceived += OnEscapeCalled;
        Core.InputManager.OnNumberReceived += OnSkillCalled;
        Core.InputManager.OnClickReceived += OnClickCalled;
        Core.InputManager.OnEndActionReceived += OnEndActionCalled;
    }

    private void OnDisable() 
    {
        Core.InputManager.OnEscapeReceived -= OnEscapeCalled;
        Core.InputManager.OnNumberReceived -= OnSkillCalled;
        Core.InputManager.OnClickReceived -= OnClickCalled;
        Core.InputManager.OnEndActionReceived -= OnEndActionCalled;
    }
    
    void OnEscapeCalled()
    {
        OnEscapePressed?.Invoke();
    }

    void OnSkillCalled(int num)
    {
        num--; // 입력받는 값이 1 ~ 4이므로 인덱스에 맞게 -1 처리
        OnSkillPressed?.Invoke(num);
    }

    void OnClickCalled()
    {
        OnClickPressed?.Invoke();
    }

    void OnEndActionCalled()
    {
        OnEndActionPressed?.Invoke();;
    }
}