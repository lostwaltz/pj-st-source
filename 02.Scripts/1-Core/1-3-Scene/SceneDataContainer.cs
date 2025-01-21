using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SceneData", menuName = "SOs/Scene/SceneDatas")]
public class SceneDataContainer : ScriptableObject
{
    public List<SceneData> sceneDataList;
}


[Serializable]
public class SceneData
{
    public string sceneName;
}
