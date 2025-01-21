using System.Collections.Generic;
using Constants;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class UIManager
{
    protected readonly Dictionary<string, UIBase> uiDic = new();

    public List<UICursorDetector> CursorDetectors = new();
    
    public virtual void Init()
    {
        Core.SceneLoadManager.OnSceneLoadComplete -= Clear;
        Core.SceneLoadManager.OnSceneLoadComplete += Clear;
    }
    
    public T GetUI<T>() where T : UIBase
    {
        var uiName = typeof(T).Name;

        if (IsExist<T>())
            return uiDic[uiName] as T;
        else
            return CreateUI<T>();
    }

    protected T CreateUI<T>() where T : UIBase
    {
        var uiName = typeof(T).Name;

        T uiRes = Resources.Load<T>(Utils.Str.Clear().Append(Path.UIPath).Append(uiName).ToString());
        var uiObj = Object.Instantiate(uiRes);

        if (IsExist<T>())
            uiDic[uiName] = uiObj;
        else
            uiDic.Add(uiName, uiObj);
        
        return uiObj;
    }

    protected bool IsExist<T>()
    {
        var uiName = typeof(T).Name;
        return uiDic.ContainsKey(uiName) && uiDic[uiName] != null;
    }

    public T OpenUI<T>() where T : UIBase
    {
        var ui = GetUI<T>(); 
        ui.Open();

        return ui;
    }
    
    public T CloseUI<T>() where T : UIBase
    {
        var ui = GetUI<T>();
        ui.Close();

        return ui;
    }
    
    public void DestroyUI<T>() where T : UIBase
    {
        var uiName = typeof(T).Name;

        if (IsExist<T>())
        {
            var ui = uiDic[uiName];
            
            Object.Destroy(ui.gameObject);
            
            uiDic.Remove(uiName);
        }
    }
    
    public void ReleaseUI<T>() where T : UIBase
    {
        uiDic.Remove(typeof(T).Name);
    }

    protected void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Clear();
    }
    
    public void Clear()
    {
        uiDic.Clear();
    }

    public void ClearDetector()
    {
        foreach (var detector in CursorDetectors)
        {
            detector.Release();
        }
        
        CursorDetectors.Clear();
    }
}