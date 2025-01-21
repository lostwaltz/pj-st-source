using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFactory
{
    public static T CreateFactory<T>() where T : SceneBase, new()
    {
        return new T();
    }
}
