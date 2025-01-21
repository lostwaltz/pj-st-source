using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;


public class EventManager
{
    private static readonly Dictionary<Type, List<Delegate>> EventHandlers = new();
    
    public EventManager()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EventHandlers.Clear();
        OnClearEvent?.Invoke();
    }
    
    public event Action OnClearEvent;
    
    public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : EventArgs
    {
        Type eventType = typeof(TEvent);
        
        if (false == EventHandlers?.ContainsKey(eventType))
            EventHandlers[eventType] = new List<Delegate>();

        EventHandlers?[eventType].Add(handler);
    }

    public void Unsubscribe<TEvent>(Action<TEvent> handler) where TEvent : EventArgs
    {
        Type eventType = typeof(TEvent);
        
        if (EventHandlers.TryGetValue(eventType, out var eventHandler))
            eventHandler.Remove(handler);
    }
    
    public void Publish<TEvent>(TEvent eventArgs) where TEvent : EventArgs
    {
        Type eventType = typeof(TEvent);
    
        if (false == EventHandlers?.ContainsKey(eventType))
            return;
    
        var handlers = EventHandlers?[eventType];
        if (handlers == null) return;
    
        for (var i = 0; i < handlers.Count; i++)
        {
            if (false == handlers[i] is Action<TEvent>)
            {
                Debug.LogError("publish error : sub event type is not " + typeof(TEvent));
                continue;
            }

            ((Action<TEvent>)handlers[i])(eventArgs);
        }
    }
}

