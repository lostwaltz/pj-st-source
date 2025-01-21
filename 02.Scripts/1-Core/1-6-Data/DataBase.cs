using System;
using System.Collections.Generic;
using UnityEngine;

public class DataBase<T>  where T : DataModel
{
    public List<T> DataBaseList { get; private set; }
    public Dictionary<int ,T> DataBaseDictionary { get; private set; }

    public DataBase(List<T> list)
    {
        GenerateDbFromList(list);
    }
    public DataBase(string path = "JSON/")
    {
        var jsonData = Resources.Load<TextAsset>(path).text;
        DataBaseList = JsonUtility.FromJson<Wrapper<T>>(jsonData).Items;
        DataBaseDictionary = new Dictionary<int, T>();
        foreach (var item in DataBaseList)
        {
            DataBaseDictionary.Add(item.Key, item);
        }
    }

    private void GenerateDbFromList(List<T> list)
    {
        DataBaseList = new List<T>(list);
        
        foreach (var data in list)
            DataBaseDictionary.Add(data.Key, data);
    }

    public T GetByKey(int key)
    {
        DataBaseDictionary.TryGetValue(key, out T data);
        
        return data;
    }
    public T GetByIndex(int index)
    {
        if (index >= 0 && index < DataBaseList.Count)
        {
            return DataBaseList[index];
        }
        return null;
    }
    
    // Wrapper class is dependent on the ExcelToJsonWizard functionality.
    [Serializable]
    private class Wrapper<TY>
    {
        public List<TY> Items;
    }
}
