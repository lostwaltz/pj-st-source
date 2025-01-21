using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public static class Extension
{
    public static void BindEvent(this GameObject go, Action<PointerEventData> action, EnumTypes.UIEvent type = EnumTypes.UIEvent.Click)
    {
        UIBase.BindEvent(go, action, type);
    }
    public static float Clamp(this ref float value, float min, float max)
    {
        return value = Mathf.Clamp(value, min, max);
    }
    public static int Clamp(this ref int value, int min, int max)
    {
        return value = Mathf.Clamp(value, min, max);
    }
    
    public static T GetOrAdd<T>(this GameObject gameObject) where T : Component {
        T component = gameObject.GetComponent<T>();
        if (!component) component = gameObject.AddComponent<T>();

        return component;
    }
    
    public static float ToLogarithmicVolume(this float sliderValue)
    {
        return Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20;
    }

    public static float ToLogarithmicFraction(this float fraction)
    {
        return Mathf.Log10(1 + 9 * fraction) / Mathf.Log10(10);
    }

    public static float Abs(this float value)
    {
        return value = Mathf.Abs(value);
    }
    
    public static int Abs(this int value)
    {
        return value = Mathf.Abs(value);
    }

    public static Vector3 AddRandom(this Vector3 value, float x, float y, float z, bool abs = false)
    {
        return value += abs
            ? new Vector3(Random.Range(0, x), Random.Range(0, x), Random.Range(0, x))
            : new Vector3(Random.Range(-x, x), Random.Range(-y, y), Random.Range(-z, z));
    }
    public static Vector3 AddRandom(this Vector3 value, float amount, bool abs = false)
    {
        return value += abs
            ? new Vector3(Random.Range(0, amount), Random.Range(0, amount), Random.Range(0, amount))
            : new Vector3(Random.Range(-amount, amount), Random.Range(-amount, amount), Random.Range(-amount, amount));
    }
}