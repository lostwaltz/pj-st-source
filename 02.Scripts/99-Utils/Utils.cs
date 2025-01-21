using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public abstract class Utils
{
    public static readonly StringBuilder Str = new();
    
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        return FindChild<Transform>(go, name, recursive).gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if(go == null) return null;

        if (recursive == false)
        {
            for (var i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);

                if (!string.IsNullOrEmpty(name) && transform.name != name) continue;

                T component = transform.GetComponent<T>();

                if (component != null) return component;
            }
        }
        else
        {
            return go.GetComponentsInChildren<T>()
                .FirstOrDefault(component => string.IsNullOrEmpty(name) || component.name == name);
        }

        return null;
    }

    public static Action CheckCondition(bool condition, Action successAction, Action failAction = null)
    {
        Action resultAction = condition ? successAction : failAction; 
        
        resultAction?.Invoke();

        return resultAction;
    }

    public static bool ToggleTrigger(ref bool state,  Action successAction, Action failAction)
    {
        state = !state;
        CheckCondition(state, successAction, failAction);

        return state;
    }
}